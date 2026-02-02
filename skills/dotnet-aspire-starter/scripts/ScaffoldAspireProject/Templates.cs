static class Templates
{
    public static string DirectoryBuildProps => """
<Project>
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
    <EnforceCodeStyleInBuild>true</EnforceCodeStyleInBuild>
    <EnableNETAnalyzers>true</EnableNETAnalyzers>
    <AnalysisLevel>latest-all</AnalysisLevel>
  </PropertyGroup>
</Project>
""";

    public static string EditorConfig => """
# EditorConfig - Microsoft C# Coding Conventions
root = true

[*]
indent_style = space
indent_size = 4
end_of_line = lf
charset = utf-8
trim_trailing_whitespace = true
insert_final_newline = true

[*.{cs,csx}]
# Organize usings
dotnet_sort_system_directives_first = true
dotnet_separate_import_directive_groups = false

# Namespace preferences
csharp_style_namespace_declarations = file_scoped:warning

# Expression-bodied members
csharp_style_expression_bodied_methods = when_on_single_line:suggestion
csharp_style_expression_bodied_constructors = false:suggestion
csharp_style_expression_bodied_properties = true:suggestion

# Pattern matching
csharp_style_pattern_matching_over_is_with_cast_check = true:warning
csharp_style_pattern_matching_over_as_with_null_check = true:warning

# Null checking
csharp_style_throw_expression = true:suggestion
csharp_style_conditional_delegate_call = true:suggestion

# var preferences
csharp_style_var_for_built_in_types = true:suggestion
csharp_style_var_when_type_is_apparent = true:suggestion
csharp_style_var_elsewhere = true:suggestion

# Code block preferences
csharp_prefer_braces = true:warning

# Naming conventions
dotnet_naming_rule.interface_should_be_begins_with_i.severity = warning
dotnet_naming_rule.interface_should_be_begins_with_i.symbols = interface
dotnet_naming_rule.interface_should_be_begins_with_i.style = begins_with_i

dotnet_naming_symbols.interface.applicable_kinds = interface
dotnet_naming_symbols.interface.applicable_accessibilities = public, internal, private, protected

dotnet_naming_style.begins_with_i.required_prefix = I
dotnet_naming_style.begins_with_i.capitalization = pascal_case

[*.{json,yml,yaml}]
indent_size = 2

[*.md]
trim_trailing_whitespace = false
""";

    public static string VsCodeSettings(ProjectConfig config) => $$"""
{
  "editor.formatOnSave": true,
  "editor.tabSize": 4,
  "files.eol": "\n",
  "files.trimTrailingWhitespace": true,
  "files.insertFinalNewline": true,
  "typescript.tsdk": "node_modules/typescript/lib",
  "omnisharp.enableRoslynAnalyzers": true,
  "dotnet.defaultSolution": "${workspaceFolder}/{{config.Name}}.sln"
}
""";

    public static string VsCodeLaunch(ProjectConfig config) => $$"""
{
  "version": "0.2.0",
  "configurations": [
    {
      "name": "Run Aspire Host",
      "type": "coreclr",
      "request": "launch",
      "preLaunchTask": "build",
      "program": "${workspaceFolder}/{{config.Name}}.AppHost/bin/Debug/net10.0/{{config.Name}}.AppHost.dll",
      "args": [],
      "cwd": "${workspaceFolder}/{{config.Name}}.AppHost",
      "console": "internalConsole",
      "stopAtEntry": false
    }
  ]
}
""";

    public static string VsCodeTasks(ProjectConfig config) => $$"""
{
  "version": "2.0.0",
  "tasks": [
    {
      "label": "build",
      "command": "dotnet",
      "type": "process",
      "args": [
        "build",
        "${workspaceFolder}/{{config.Name}}.sln",
        "/property:GenerateFullPaths=true",
        "/consoleloggerparameters:NoSummary;ForceNoAlign"
      ],
      "problemMatcher": "$msCompile"
    },
    {
      "label": "watch",
      "command": "dotnet",
      "type": "process",
      "args": [
        "watch",
        "run",
        "--project",
        "${workspaceFolder}/{{config.Name}}.AppHost"
      ],
      "problemMatcher": "$msCompile"
    }
  ]
}
""";

    public static string CopilotInstructions(ProjectConfig config) => $$"""
# GitHub Copilot Instructions for {{config.Name}}

## Project Overview

{{config.Description}}

This is a .NET 10 Aspire solution with:
- **{{config.Name}}.AppHost**: Aspire orchestrator for local development
- **{{config.Name}}.ServiceDefaults**: Shared configuration (OpenTelemetry, Health Checks, Resilience)
- **{{config.Name}}.Api**: REST API backend with Entity Framework Core
- **{{config.Name}}.Web**: ASP.NET MVC + Razor frontend with TypeScript

## Coding Conventions

### C# Style
- Use file-scoped namespaces
- Use `var` when type is apparent
- Use expression-bodied members for single-line getters
- Prefer pattern matching over `is` with cast or `as` with null check
- Always use braces for control statements

### Entity Framework
- Use code-first migrations
- Define entity configurations in separate configuration classes
- Use SQLite for local development, configure production database separately

### API Controllers
- Return `ActionResult<T>` for typed responses
- Use `[ApiController]` attribute
- Follow REST conventions for route naming
- Include OpenAPI documentation attributes

### Views and TypeScript
- Use Razor views with tag helpers
- Write TypeScript in `Scripts/` folder
- Compile TypeScript with `npm run build`
- CSS goes in `wwwroot/css/`

## Architecture Patterns

### Service Layer
Create services in `{{config.Name}}.Api/Services/` for business logic.
Inject services into controllers.

### Repository Pattern (Optional)
For complex data access, create repositories in `{{config.Name}}.Api/Repositories/`.

### DTOs
Create Data Transfer Objects in `{{config.Name}}.Api/Models/Dtos/` for API contracts.

## Telemetry

OpenTelemetry is configured in ServiceDefaults. Custom metrics example:
```csharp
private static readonly Counter<long> RequestCounter = 
    Metrics.CreateCounter<long>("{{config.Name.ToLowerInvariant()}}.requests");
```

## Authentication

OIDC is prepared but not enabled. To enable:
1. Uncomment auth configuration in Api/Program.cs and Web/Program.cs
2. Configure Auth section in appsettings.json
3. Add [Authorize] attributes to protected endpoints
""";

    public static string ArchitectureDoc(ProjectConfig config) => $$"""
# {{config.Name}} Architecture

## Overview

{{config.Description}}

## Solution Structure

```
{{config.Name}}/
├── {{config.Name}}.AppHost/              # Aspire orchestrator
├── {{config.Name}}.ServiceDefaults/      # Shared configuration
├── {{config.Name}}.Api/                  # REST API
└── {{config.Name}}.Web/                  # MVC Frontend
```

## Service Communication

The Web frontend communicates with the API using HTTP clients configured with:
- **Service Discovery**: URLs resolved via Aspire
- **Resilience**: Polly policies for retry, circuit breaker, timeout
- **Observability**: Distributed tracing via OpenTelemetry

## Database

Entity Framework Core with SQLite for local development.

### Creating Entities

1. Add entity class to `{{config.Name}}.Api/Models/`
2. Add DbSet to ApplicationDbContext
3. Create migration: `dotnet ef migrations add <Name> --project {{config.Name}}.Api`
4. Apply migration: `dotnet ef database update --project {{config.Name}}.Api`

## Observability

### Logs
Structured logging via ILogger with OpenTelemetry export.

### Metrics
Runtime metrics, HTTP metrics, and custom counters.

### Traces
Distributed tracing across Web → API calls.

### Local Export
Console export enabled by default. Set `OTEL_EXPORTER_OTLP_ENDPOINT` for OTLP export.

## Authentication (Prepared)

OIDC configuration is prepared but commented out. When enabled:
- Web uses cookie authentication with OIDC challenge
- API validates JWT bearer tokens
- Tokens can be propagated from Web to API calls

## Resilience

Polly policies configured in appsettings.json:
- **Retry**: Exponential backoff for transient failures
- **Circuit Breaker**: Prevent cascade failures
- **Timeout**: Limit request duration
""";
}
