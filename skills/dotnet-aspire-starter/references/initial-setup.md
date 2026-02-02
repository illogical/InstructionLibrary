# Initial Setup Guide

After scaffolding your .NET Aspire project, follow these steps to complete the initial configuration.

## Prerequisites

Verify requirements are installed:

```bash
# .NET 10 SDK
dotnet --version  # Should be 10.0.x or higher

# Aspire workload
dotnet workload list | grep aspire  # Should show aspire installed

# Node.js (for TypeScript)
node --version  # Should be 18.x or higher
npm --version
```

Install missing components:

```bash
# Install Aspire workload
dotnet workload install aspire

# Update Aspire workload
dotnet workload update
```

## Step 1: Restore Dependencies

Navigate to your project and restore .NET packages:

```bash
cd YourProject
dotnet restore
```

Install npm packages for TypeScript compilation:

```bash
cd YourProject.Web
npm install
cd ..
```

## Step 2: Configure Database

The scaffolded project uses SQLite by default. The connection string is in `YourProject.Api/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=yourproject.db"
  }
}
```

### For PostgreSQL

```bash
dotnet add YourProject.Api package Npgsql.EntityFrameworkCore.PostgreSQL
```

Update `Program.cs`:

```csharp
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
```

Update `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=yourproject;Username=postgres;Password=yourpassword"
  }
}
```

### For SQL Server

```bash
dotnet add YourProject.Api package Microsoft.EntityFrameworkCore.SqlServer
```

Update `Program.cs`:

```csharp
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
```

Update `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=yourproject;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

## Step 3: Create Your First Entity

Create a model in `YourProject.Api/Models/`:

```csharp
namespace YourProject.Api.Models;

public class TodoItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsComplete { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
```

Add the DbSet to `ApplicationDbContext.cs`:

```csharp
public DbSet<TodoItem> TodoItems => Set<TodoItem>();
```

Optional: Create entity configuration in `YourProject.Api/Data/Configurations/`:

```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YourProject.Api.Models;

namespace YourProject.Api.Data.Configurations;

public class TodoItemConfiguration : IEntityTypeConfiguration<TodoItem>
{
    public void Configure(EntityTypeBuilder<TodoItem> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Title).IsRequired().HasMaxLength(200);
        builder.Property(t => t.Description).HasMaxLength(2000);
        builder.HasIndex(t => t.IsComplete);
        builder.HasIndex(t => t.CreatedAt);
    }
}
```

Apply configuration in `ApplicationDbContext.cs`:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);
    modelBuilder.ApplyConfiguration(new TodoItemConfiguration());
}
```

## Step 4: Create and Apply Migration

```bash
# Create migration
dotnet ef migrations add InitialCreate --project YourProject.Api

# Apply migration
dotnet ef database update --project YourProject.Api
```

Verify the database file is created (`yourproject.db` in the Api project directory).

## Step 5: Create Your First Controller

Create `YourProject.Api/Controllers/TodosController.cs`:

```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YourProject.Api.Data;
using YourProject.Api.Models;

namespace YourProject.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodosController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<TodosController> _logger;

    public TodosController(ApplicationDbContext context, ILogger<TodosController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodos()
    {
        return await _context.TodoItems.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItem>> GetTodo(int id)
    {
        var todo = await _context.TodoItems.FindAsync(id);

        if (todo == null)
        {
            return NotFound();
        }

        return todo;
    }

    [HttpPost]
    public async Task<ActionResult<TodoItem>> CreateTodo(TodoItem todo)
    {
        _context.TodoItems.Add(todo);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Created todo {TodoId} with title {Title}", todo.Id, todo.Title);

        return CreatedAtAction(nameof(GetTodo), new { id = todo.Id }, todo);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTodo(int id, TodoItem todo)
    {
        if (id != todo.Id)
        {
            return BadRequest();
        }

        _context.Entry(todo).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        _logger.LogInformation("Updated todo {TodoId}", id);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodo(int id)
    {
        var todo = await _context.TodoItems.FindAsync(id);
        if (todo == null)
        {
            return NotFound();
        }

        _context.TodoItems.Remove(todo);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Deleted todo {TodoId}", id);

        return NoContent();
    }
}
```

## Step 6: Build and Run

Build the solution:

```bash
dotnet build
```

Compile TypeScript:

```bash
cd YourProject.Web
npm run build
cd ..
```

Run the Aspire AppHost:

```bash
dotnet run --project YourProject.AppHost
```

Open the Aspire Dashboard (typically http://localhost:15000) to view:
- Running services (Web, API)
- Logs and traces
- Metrics

Access your application:
- Web: https://localhost:7001 (check AppHost output for actual port)
- API: https://localhost:7002/swagger (check AppHost output for actual port)

## Step 7: Verify Everything Works

### Test the API

```bash
# Get all todos (should be empty)
curl https://localhost:7002/api/todos

# Create a todo
curl -X POST https://localhost:7002/api/todos \
  -H "Content-Type: application/json" \
  -d '{"title":"Test Todo","description":"This is a test","isComplete":false}'

# Get all todos (should show the created todo)
curl https://localhost:7002/api/todos
```

### Test the Web

1. Open the Web URL in a browser
2. Verify the home page loads
3. Open browser dev tools and check console for "Application initialized"

## Step 8: Configure VS Code (Optional)

The scaffolder creates `.vscode/` configurations. Open the project in VS Code:

```bash
code .
```

Press F5 to start debugging, or use the Run menu.

## Next Steps

- Create additional entities and controllers
- Add views and TypeScript for your frontend
- Configure authentication (see [OIDC Preparation](./oidc-preparation.md))
- Set up Grafana for telemetry (see [Telemetry Setup](./telemetry-setup.md))
- Customize Polly resilience policies (see [Polly Configuration](./polly-configuration.md))

## Troubleshooting

### Port Already in Use

If ports are in use, update `launchSettings.json` in Api and Web projects:

```json
{
  "profiles": {
    "https": {
      "applicationUrl": "https://localhost:7010;http://localhost:5010"
    }
  }
}
```

### EF Migrations Error

```bash
# Install EF tools globally
dotnet tool install --global dotnet-ef

# Update EF tools
dotnet tool update --global dotnet-ef
```

### TypeScript Build Errors

```bash
# Clean and reinstall
cd YourProject.Web
rm -rf node_modules package-lock.json
npm install
npm run build
```

### Aspire Dashboard Not Opening

Check the AppHost output for the dashboard URL. It may be different than http://localhost:15000.

If the dashboard doesn't start, ensure Aspire workload is properly installed:

```bash
dotnet workload update
dotnet workload install aspire
```
