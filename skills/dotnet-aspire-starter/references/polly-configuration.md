# Polly Resilience Configuration

Polly.NET is pre-configured in all HTTP clients for automatic resilience.

## Default Policies

The scaffolded projects use `AddStandardResilienceHandler()` which includes:

| Policy | Default Configuration | Purpose |
|--------|----------------------|---------|
| **Retry** | 3 attempts, exponential backoff (2s, 4s, 8s) | Transient HTTP failures (5xx, timeouts) |
| **Circuit Breaker** | 50% failure ratio, 30s break duration | Prevent cascade failures |
| **Timeout** | 30s per request | Limit hung requests |

## Customizing Policies

### Per-Client Configuration

Override defaults for specific HTTP clients:

```csharp
builder.Services.AddHttpClient("external-api", client =>
{
    client.BaseAddress = new Uri("https://external-api.com");
})
.AddStandardResilienceHandler(options =>
{
    // Customize retry
    options.Retry.MaxRetryAttempts = 5;
    options.Retry.Delay = TimeSpan.FromSeconds(1);
    options.Retry.BackoffType = Polly.Retry.DelayBackoffType.Exponential;
    
    // Customize circuit breaker
    options.CircuitBreaker.FailureRatio = 0.3; // Open at 30% failure rate
    options.CircuitBreaker.MinimumThroughput = 20; // Require 20 requests before evaluation
    options.CircuitBreaker.BreakDuration = TimeSpan.FromMinutes(1);
    
    // Customize timeout
    options.AttemptTimeout.Timeout = TimeSpan.FromSeconds(10);
    options.TotalRequestTimeout.Timeout = TimeSpan.FromSeconds(60);
});
```

### Configuration from appsettings.json

Read values from configuration:

```csharp
builder.Services.AddHttpClient("api")
    .AddStandardResilienceHandler(options =>
    {
        var pollyConfig = builder.Configuration.GetSection("Polly");
        
        options.Retry.MaxRetryAttempts = pollyConfig.GetValue<int>("Retry:MaxRetryAttempts");
        options.Retry.Delay = pollyConfig.GetValue<TimeSpan>("Retry:Delay");
        
        options.CircuitBreaker.FailureRatio = pollyConfig.GetValue<double>("CircuitBreaker:FailureRatio");
        options.CircuitBreaker.BreakDuration = pollyConfig.GetValue<TimeSpan>("CircuitBreaker:BreakDuration");
        
        options.AttemptTimeout.Timeout = pollyConfig.GetValue<TimeSpan>("Timeout:TimeoutDuration");
    });
```

## Advanced Patterns

### Fallback Values

Return default values when all retries fail:

```csharp
builder.Services.AddHttpClient("weather-api")
    .AddStandardResilienceHandler()
    .AddResilienceHandler("fallback", builder =>
    {
        builder.AddFallback(new FallbackStrategyOptions<HttpResponseMessage>
        {
            FallbackAction = args => Outcome.FromResultAsValueTask(
                new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent.Create(new { temp = 72, condition = "unknown" })
                })
        });
    });
```

### Hedging (Parallel Requests)

Send duplicate requests to reduce latency:

```csharp
builder.Services.AddHttpClient("fast-api")
    .AddStandardHedgingHandler(options =>
    {
        options.Delay = TimeSpan.FromMilliseconds(500);
        options.MaxHedgedAttempts = 2; // Total 3 requests (1 initial + 2 hedged)
    });
```

### Rate Limiting

Control outbound request rate:

```csharp
builder.Services.AddHttpClient("rate-limited-api")
    .AddStandardResilienceHandler()
    .AddResilienceHandler("rate-limit", builder =>
    {
        builder.AddRateLimiter(new SlidingWindowRateLimiter(
            new SlidingWindowRateLimiterOptions
            {
                PermitLimit = 100,
                Window = TimeSpan.FromMinutes(1)
            }));
    });
```

## Monitoring Resilience

### Telemetry Integration

Polly automatically exports telemetry to OpenTelemetry:

- **Metrics**: Retry counts, circuit breaker states, timeout frequency
- **Traces**: Resilience pipeline execution spans

View in Grafana:

```promql
# Retry rate
rate(resilience_retry_count_total[5m])

# Circuit breaker open percentage
rate(resilience_circuit_breaker_state{state="open"}[5m])

# Timeout frequency
rate(resilience_timeout_count_total[5m])
```

### Custom Logging

Add logging to resilience events:

```csharp
builder.Services.AddHttpClient("logged-api")
    .AddStandardResilienceHandler(options =>
    {
        options.Retry.OnRetry = args =>
        {
            var logger = args.Context.ServiceProvider.GetRequiredService<ILogger<Program>>();
            logger.LogWarning("Retry attempt {Attempt} after {Delay}ms due to {Exception}", 
                args.AttemptNumber, args.RetryDelay.TotalMilliseconds, args.Outcome.Exception?.Message);
            return default;
        };
    });
```

## Testing with Polly

### Disable Resilience in Tests

```csharp
// In test setup
builder.Services.AddHttpClient("api", client =>
{
    client.BaseAddress = new Uri("http://localhost");
})
.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler()); // No resilience
```

### Simulate Failures

```csharp
// Test that retries work
var mockHandler = new Mock<HttpMessageHandler>();
mockHandler
    .SetupSequence<Task<HttpResponseMessage>>(/* ... */)
    .ThrowsAsync(new HttpRequestException("Transient error"))
    .ThrowsAsync(new HttpRequestException("Transient error"))
    .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

builder.Services.AddHttpClient("api")
    .ConfigurePrimaryHttpMessageHandler(() => mockHandler.Object)
    .AddStandardResilienceHandler();
```

## Performance Considerations

- **Memory**: Polly allocates minimal memory per request
- **Latency**: Retry adds latency on failures (intended behavior)
- **Throughput**: Circuit breaker prevents overloading failing services

### Optimize for Latency-Sensitive APIs

```csharp
builder.Services.AddHttpClient("low-latency-api")
    .AddStandardResilienceHandler(options =>
    {
        // Fast retries
        options.Retry.MaxRetryAttempts = 2;
        options.Retry.Delay = TimeSpan.FromMilliseconds(100);
        
        // Short timeouts
        options.AttemptTimeout.Timeout = TimeSpan.FromSeconds(2);
        options.TotalRequestTimeout.Timeout = TimeSpan.FromSeconds(5);
        
        // Aggressive circuit breaker
        options.CircuitBreaker.FailureRatio = 0.2;
        options.CircuitBreaker.BreakDuration = TimeSpan.FromSeconds(10);
    });
```

## Common Scenarios

### External API with Unreliable Network

```csharp
builder.Services.AddHttpClient("unreliable-external")
    .AddStandardResilienceHandler(options =>
    {
        options.Retry.MaxRetryAttempts = 5;
        options.Retry.Delay = TimeSpan.FromSeconds(2);
        options.CircuitBreaker.BreakDuration = TimeSpan.FromMinutes(2);
    });
```

### Internal Microservice

```csharp
builder.Services.AddHttpClient("internal-service")
    .AddStandardResilienceHandler(options =>
    {
        options.Retry.MaxRetryAttempts = 3;
        options.Retry.Delay = TimeSpan.FromMilliseconds(500);
        options.CircuitBreaker.FailureRatio = 0.5;
        options.CircuitBreaker.BreakDuration = TimeSpan.FromSeconds(30);
    });
```

### Third-Party API with Rate Limits

```csharp
builder.Services.AddHttpClient("rate-limited-third-party")
    .AddStandardResilienceHandler(options =>
    {
        options.Retry.MaxRetryAttempts = 3;
        options.Retry.Delay = TimeSpan.FromSeconds(5); // Respect rate limit windows
    })
    .AddResilienceHandler("rate-limiter", builder =>
    {
        builder.AddRateLimiter(new SlidingWindowRateLimiter(
            new SlidingWindowRateLimiterOptions
            {
                PermitLimit = 100,
                Window = TimeSpan.FromMinutes(1)
            }));
    });
```

## Troubleshooting

### Circuit Breaker Always Open

- Reduce `FailureRatio` threshold
- Increase `MinimumThroughput` to require more data
- Check if downstream service is actually failing

### Retries Exhausted Too Quickly

- Increase `MaxRetryAttempts`
- Adjust `Delay` and backoff strategy
- Check if exceptions are transient

### Timeouts Too Aggressive

- Increase `AttemptTimeout` and `TotalRequestTimeout`
- Monitor actual response times in production
- Consider network latency to external services

## Reference

- [Polly Documentation](https://www.pollydocs.org/)
- [Microsoft.Extensions.Http.Resilience](https://learn.microsoft.com/en-us/dotnet/core/resilience/)
- [OpenTelemetry Polly Instrumentation](https://opentelemetry.io/docs/instrumentation/net/libraries/#polly)
