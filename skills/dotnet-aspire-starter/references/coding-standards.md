# Microsoft C# Coding Standards

This document outlines the coding standards enforced in scaffolded .NET Aspire projects.

## Overview

All projects include:
- **EditorConfig**: Enforces style rules across all editors
- **.NET Analyzers**: Static analysis with `TreatWarningsAsErrors`
- **Nullable Reference Types**: Enabled by default

##Organization

### File-Scoped Namespaces

Use file-scoped namespaces to reduce indentation:

```csharp
// ✅ Correct
namespace TaskManager.Api.Controllers;

public class TodoController : ControllerBase
{
    // ...
}
```

```csharp
// ❌ Avoid
namespace TaskManager.Api.Controllers
{
    public class TodoController : ControllerBase
    {
        // ...
    }
}
```

### Using Directives

- Sort `using` directives alphabetically
- Place `System` namespaces first
- Separate groups with a blank line

```csharp
using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using TaskManager.Api.Data;
using TaskManager.Api.Models;
```

## Naming Conventions

### PascalCase
- Classes, structs, enums
- Methods, properties
- Public fields
- Namespaces

### camelCase
- Private fields (with `_` prefix)
- Local variables
- Method parameters

### Interfaces
Prefix with `I`:

```csharp
public interface ITaskRepository
{
    Task<Todo> GetByIdAsync(int id);
}
```

## Code Style

### var Keyword

Use `var` when the type is obvious:

```csharp
// ✅ Type is apparent
var tasks = new List<TodoItem>();
var context = new ApplicationDbContext(options);
var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

// ✅ Not obvious - specify type
ApplicationDbContext context = GetContext();
int? optionalId = TryGetId();
```

### Expression-Bodied Members

Use expression bodies for single-statement members:

```csharp
// Properties
public string FullName => $"{FirstName} {LastName}";

// Methods (when single line)
public int GetAge() => DateTime.Now.Year - BirthYear;

// Constructors (generally avoid)
public class Person
{
    public Person(string name) => Name = name;  // Prefer full body
}
```

### Pattern Matching

Prefer pattern matching:

```csharp
// ✅ Pattern matching
if (obj is TodoItem item)
{
    Console.WriteLine(item.Title);
}

// ❌ Old style
if (obj is TodoItem)
{
    var item = (TodoItem)obj;
    Console.WriteLine(item.Title);
}
```

### Null Checking

Use modern null-checking patterns:

```csharp
// ✅ Null-coalescing operator
string name = input ?? "Default";

// ✅ Null-conditional operator
int? length = text?.Length;

// ✅ Throw expression
string name = input ?? throw new ArgumentNullException(nameof(input));

// ✅ Null-conditional with delegate
Action callback = GetCallback();
callback?.Invoke();
```

### Braces

Always use braces for control statements:

```csharp
// ✅ With braces
if (condition)
{
    DoSomething();
}

// ❌ Without braces
if (condition)
    DoSomething();
```

## Async/Await

### Naming
Suffix async methods with `Async`:

```csharp
public async Task<TodoItem> GetTodoAsync(int id)
{
    return await _context.Todos.FindAsync(id);
}
```

### ConfigureAwait

In library code (not ASP.NET Core controllers), use `ConfigureAwait(false)`:

```csharp
// In services/repositories
public async Task<TodoItem> GetTodoAsync(int id)
{
    return await _context.Todos
        .FindAsync(id)
        .ConfigureAwait(false);
}

// In controllers - not needed (ASP.NET Core doesn't use SynchronizationContext)
[HttpGet("{id}")]
public async Task<ActionResult<TodoItem>> GetTodo(int id)
{
    var todo = await _repository.GetTodoAsync(id);
    return Ok(todo);
}
```

## Entity Framework Core

### DbContext Configuration

Configure entities in separate configuration classes:

```csharp
// Models/TodoItem.cs
public class TodoItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool IsComplete { get; set; }
}

// Data/Configuration/TodoItemConfiguration.cs
public class TodoItemConfiguration : IEntityTypeConfiguration<TodoItem>
{
    public void Configure(EntityTypeBuilder<TodoItem> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Title).IsRequired().HasMaxLength(200);
        builder.HasIndex(t => t.IsComplete);
    }
}

// Data/ApplicationDbContext.cs
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);
    modelBuilder.ApplyConfiguration(new TodoItemConfiguration());
}
```

### Navigation Properties

Use nullable reference types correctly:

```csharp
public class TodoItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    
    // Optional foreign key
    public int? CategoryId { get; set; }
    public Category? Category { get; set; }  // Nullable - may not be loaded
    
    // Required foreign key
    public int UserId { get; set; }
    public User User { get; set; } = null!;  // Not null when loaded, but EF sets it
}
```

## API Controllers

### Controller Structure

```csharp
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
        return await _context.Todos.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItem>> GetTodo(int id)
    {
        var todo = await _context.Todos.FindAsync(id);

        if (todo == null)
        {
            return NotFound();
        }

        return todo;
    }

    [HttpPost]
    public async Task<ActionResult<TodoItem>> CreateTodo(TodoItem todo)
    {
        _context.Todos.Add(todo);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTodo), new { id = todo.Id }, todo);
    }
}
```

### Return Types

Use `ActionResult<T>` for typed responses:

```csharp
// ✅ Typed response
[HttpGet("{id}")]
public async Task<ActionResult<TodoItem>> GetTodo(int id)
{
    var todo = await _context.Todos.FindAsync(id);
    if (todo == null)
        return NotFound();
    
    return todo;  // Implicit conversion
}

// ❌ Untyped
[HttpGet("{id}")]
public async Task<IActionResult> GetTodo(int id)
{
    var todo = await _context.Todos.FindAsync(id);
    if (todo == null)
        return NotFound();
    
    return Ok(todo);
}
```

## Logging

Use structured logging with ILogger:

```csharp
// ✅ Structured logging
_logger.LogInformation("Processing todo {TodoId} for user {UserId}", 
    todoId, userId);

// ❌ String interpolation/concatenation
_logger.LogInformation($"Processing todo {todoId} for user {userId}");
```

### Log Levels

- **Trace**: Very detailed, may include sensitive data
- **Debug**: Development diagnostics
- **Information**: General flow (e.g., "User logged in")
- **Warning**: Unexpected events that don't stop the request
- **Error**: Errors and exceptions
- **Critical**: Failures requiring immediate attention

## Testing

### Test Method Naming

Use descriptive names:

```csharp
[Fact]
public async Task GetTodo_WithValidId_ReturnsTodo()
{
    // Arrange
    var controller = new TodosController(_context, _logger);
    
    // Act
    var result = await controller.GetTodo(1);
    
    // Assert
    var actionResult = Assert.IsType<ActionResult<TodoItem>>(result);
    var todo = Assert.IsType<TodoItem>(actionResult.Value);
    Assert.Equal(1, todo.Id);
}
```

### AAA Pattern

Structure tests with Arrange-Act-Assert:

```csharp
[Fact]
public async Task CreateTodo_WithValidData_CreatesTodo()
{
    // Arrange
    var todo = new TodoItem { Title = "Test" };
    var controller = new TodosController(_context, _logger);
    
    // Act
    var result = await controller.CreateTodo(todo);
    
    // Assert
    Assert.IsType<CreatedAtActionResult>(result.Result);
    Assert.Equal(1, _context.Todos.Count());
}
```

## Documentation

### XML Documentation

Document public APIs:

```csharp
/// <summary>
/// Retrieves a todo item by its unique identifier.
/// </summary>
/// <param name="id">The unique identifier of the todo item.</param>
/// <returns>The todo item if found; otherwise, 404 Not Found.</returns>
/// <response code="200">Returns the requested todo item.</response>
/// <response code="404">If the todo item is not found.</response>
[HttpGet("{id}")]
[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
public async Task<ActionResult<TodoItem>> GetTodo(int id)
{
    var todo = await _context.Todos.FindAsync(id);
    if (todo == null)
        return NotFound();
    
    return todo;
}
```

## Analyzer Configuration

The scaffolded projects include these analyzer rules:

| Rule | Severity | Description |
|------|----------|-------------|
| `CA1848` | Warning | Use LoggerMessage delegates for performance |
| `CA2007` | None | ConfigureAwait not required in ASP.NET Core |
| `IDE0055` | Warning | Fix formatting |
| `IDE0161` | Warning | Use file-scoped namespaces |
| `CA1062` | Warning | Validate public method arguments |
| `CA1303` | None | Don't require literal localization in samples |

See `.editorconfig` in generated projects for the complete configuration.
