# OpenTelemetry & Grafana Setup

OpenTelemetry is pre-configured in all scaffolded projects for comprehensive observability.

## What's Included

The ServiceDefaults project configures:

| Component | Purpose | Configuration |
|-----------|---------|---------------|
| **Logging** | Structured logs | ILogger with OpenTelemetry export |
| **Metrics** | Performance counters | ASP.NET Core, HTTP, Runtime metrics |
| **Traces** | Distributed tracing | Request tracking across services |
| **OTLP Export** | Protocol | OpenTelemetry Protocol (OTLP) for Grafana, Jaeger, etc. |

## Local Development

By default, telemetry exports to the console. View logs and metrics in the terminal:

```bash
dotnet run --project YourProject.AppHost
```

You'll see structured logs like:

```
info: YourProject.Api[0]
      Processing request GET /api/todos
      => RequestId:0HN4...
      => RequestPath:/api/todos
      => UserId:user@example.com
```

## Grafana Integration

### Step 1: Run Grafana Stack with Docker

Create `docker-compose.yml`:

```yaml
version: '3.8'

services:
  tempo:
    image: grafana/tempo:latest
    command: [ "-config.file=/etc/tempo.yaml" ]
    volumes:
      - ./tempo.yaml:/etc/tempo.yaml
      - tempo-data:/tmp/tempo
    ports:
      - "3200:3200"   # Tempo
      - "4317:4317"   # OTLP gRPC

  prometheus:
    image: prom/prometheus:latest
    command:
      - --config.file=/etc/prometheus/prometheus.yml
      - --enable-feature=exemplar-storage
    volumes:
      - ./prometheus.yml:/etc/prometheus/prometheus.yml
    ports:
      - "9090:9090"

  grafana:
    image: grafana/grafana:latest
    volumes:
      - grafana-data:/var/lib/grafana
      - ./grafana-datasources.yml:/etc/grafana/provisioning/datasources/datasources.yaml
    environment:
      - GF_AUTH_ANONYMOUS_ENABLED=true
      - GF_AUTH_ANONYMOUS_ORG_ROLE=Admin
    ports:
      - "3000:3000"

volumes:
  tempo-data:
  grafana-data:
```

Create `tempo.yaml`:

```yaml
server:
  http_listen_port: 3200

distributor:
  receivers:
    otlp:
      protocols:
        grpc:
          endpoint: 0.0.0.0:4317

storage:
  trace:
    backend: local
    local:
      path: /tmp/tempo/traces
```

Create `prometheus.yml`:

```yaml
global:
  scrape_interval: 15s

scrape_configs:
  - job_name: 'aspire-apps'
    static_configs:
      - targets: ['host.docker.internal:5000']  # Adjust port for your API
```

Create `grafana-datasources.yml`:

```yaml
apiVersion: 1

datasources:
  - name: Tempo
    type: tempo
    access: proxy
    url: http://tempo:3200

  - name: Prometheus
    type: prometheus
    access: proxy
    url: http://prometheus:9090
```

Start the stack:

```bash
docker-compose up -d
```

### Step 2: Configure OTLP Export

Update `appsettings.json` in both Api and Web projects:

```json
{
  "OpenTelemetry": {
    "ServiceName": "YourProject.Api",
    "ExportToConsole": false
  }
}
```

Set environment variable for OTLP endpoint:

```bash
export OTEL_EXPORTER_OTLP_ENDPOINT=http://localhost:4317
```

Or add to `launchSettings.json`:

```json
{
  "profiles": {
    "http": {
      "environmentVariables": {
        "OTEL_EXPORTER_OTLP_ENDPOINT": "http://localhost:4317"
      }
    }
  }
}
```

### Step 3: Access Grafana

1. Open http://localhost:3000
2. Navigate to **Explore**
3. Select **Tempo** datasource
4. Query for traces
5. Select **Prometheus** datasource for metrics

## Custom Metrics

### Creating Counters

```csharp
using System.Diagnostics.Metrics;

public class TodoService
{
    private static readonly Meter Meter = new("YourProject.Api");
    private static readonly Counter<long> TodoCreatedCounter = 
        Meter.CreateCounter<long>("todos.created", "Count of todos created");
    
    public async Task<TodoItem> CreateTodoAsync(TodoItem todo)
    {
        // ... save to database
        TodoCreatedCounter.Add(1, new KeyValuePair<string, object?>("user", userId));
        return todo;
    }
}
```

### Creating Histograms

Track value distributions (e.g., response times, sizes):

```csharp
private static readonly Histogram<double> RequestDuration = 
    Meter.CreateHistogram<double>("request.duration", "ms", "Request duration in milliseconds");

public async Task<IActionResult> ProcessRequest()
{
    var stopwatch = Stopwatch.StartNew();
    
    // ... process request
    
    RequestDuration.Record(stopwatch.ElapsedMilliseconds, 
        new KeyValuePair<string, object?>("endpoint", "/api/todos"));
    
    return Ok();
}
```

### Observable Gauges

Track current values (e.g., queue sizes, connection counts):

```csharp
private static int _activeConnections = 0;

private static readonly ObservableGauge<int> ActiveConnectionsGauge = 
    Meter.CreateObservableGauge("active.connections", 
        () => _activeConnections, 
        "Count of active connections");
```

## Custom Traces

### Adding Spans

```csharp
using System.Diagnostics;

public class TodoService
{
    private static readonly ActivitySource ActivitySource = new("YourProject.Api");
    
    public async Task<TodoItem> GetTodoAsync(int id)
    {
        using var activity = ActivitySource.StartActivity("GetTodo");
        activity?.SetTag("todo.id", id);
        
        var todo = await _context.Todos.FindAsync(id);
        
        if (todo == null)
        {
            activity?.SetStatus(ActivityStatusCode.Error, "Todo not found");
        }
        
        return todo;
    }
}
```

### Adding Events

```csharp
activity?.AddEvent(new ActivityEvent("CacheHit", 
    tags: new ActivityTagsCollection
    {
        { "cache.key", cacheKey },
        { "cache.ttl", ttl }
    }));
```

## Structured Logging

### Best Practices

```csharp
// ✅ Structured with named parameters
_logger.LogInformation("User {UserId} created todo {TodoId}", userId, todoId);

// ✅ Include context
_logger.LogWarning("Failed to send notification for todo {TodoId} to user {UserId}. " +
    "Retry attempt {RetryAttempt}", todoId, userId, retryAttempt);

// ✅ Log exceptions with context
_logger.LogError(ex, "Failed to process todo {TodoId}", todoId);

// ❌ String interpolation loses structure
_logger.LogInformation($"User {userId} created todo {todoId}");

// ❌ Concatenation loses structure
_logger.LogInformation("User " + userId + " created todo " + todoId);
```

### Log Scopes

Group related log entries:

```csharp
using (_logger.BeginScope("Processing batch {BatchId}", batchId))
{
    foreach (var item in batch)
    {
        _logger.LogInformation("Processing item {ItemId}", item.Id);
        // All logs in this scope will include BatchId
    }
}
```

## Production Configuration

### appsettings.Production.json

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore.Database.Command": "Warning"
    }
  },
  "OpenTelemetry": {
    "ServiceName": "YourProject.Api",
    "ExportToConsole": false
  }
}
```

### Environment Variables

```bash
# OTLP endpoint (Grafana Cloud, Azure Monitor, etc.)
OTEL_EXPORTER_OTLP_ENDPOINT=https://your-otlp-endpoint.com:4317

# Optional: Authentication headers
OTEL_EXPORTER_OTLP_HEADERS=Authorization=Bearer your-token

# Optional: Sampling rate (1.0 = 100%, 0.1 = 10%)
OTEL_TRACES_SAMPLER=traceidratio
OTEL_TRACES_SAMPLER_ARG=0.1
```

## Grafana Cloud

For managed Grafana:

1. Sign up at https://grafana.com/
2. Get your OTLP endpoint and credentials
3. Configure environment variables:

```bash
OTEL_EXPORTER_OTLP_ENDPOINT=https://otlp-gateway-prod-us-central-0.grafana.net/otlp
OTEL_EXPORTER_OTLP_HEADERS=Authorization=Basic <base64-encoded-credentials>
```

## Azure Monitor

For Azure Application Insights:

```bash
dotnet add package Azure.Monitor.OpenTelemetry.Exporter
```

Update ServiceDefaults/Extensions.cs:

```csharp
builder.Services.AddOpenTelemetry()
    .UseAzureMonitor(options =>
    {
        options.ConnectionString = builder.Configuration["ApplicationInsights:ConnectionString"];
    });
```

## Troubleshooting

### Telemetry Not Appearing

1. Check `OTEL_EXPORTER_OTLP_ENDPOINT` is set
2. Verify network connectivity to telemetry backend
3. Check console logs for export errors
4. Ensure ServiceDefaults is referenced in all projects

### High Cardinality Warnings

Avoid high-cardinality tags (unique values per request):

```csharp
// ❌ Don't include GUIDs or timestamps in metric tags
counter.Add(1, new KeyValuePair<string, object?>("request_id", Guid.NewGuid()));

// ✅ Use lower cardinality tags
counter.Add(1, new KeyValuePair<string, object?>("endpoint", "/api/todos"));
```

### Performance Impact

- **Logging**: Minimal overhead with structured logging
- **Metrics**: Negligible (aggregated in-memory)
- **Traces**: Use sampling in production (10-20% of requests)

Set sampling via environment variable:

```bash
OTEL_TRACES_SAMPLER=traceidratio
OTEL_TRACES_SAMPLER_ARG=0.1  # Sample 10% of traces
```
