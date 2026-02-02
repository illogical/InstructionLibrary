using System.Diagnostics;
using System.Text.Json;

// ============================================================================
// Configuration
// ============================================================================

record ProjectConfig(string Name, string Description, string OutputPath)
{
    public string Namespace => Name.Replace(" ", "").Replace("-", "");
}

// ============================================================================
// Main Entry Point
// ============================================================================

var config = ParseArgs(args);
await ScaffoldProject(config);

// ============================================================================
// Argument Parsing
// ============================================================================

ProjectConfig ParseArgs(string[] arguments)
{
    string name = "";
    string description = "";
    string output = ".";
    
    for (int i = 0; i < arguments.Length; i++)
    {
        switch (arguments[i])
        {
            case "--name" when i + 1 < arguments.Length:
                name = arguments[++i];
                break;
            case "--description" when i + 1 < arguments.Length:
                description = arguments[++i];
                break;
            case "--output" when i + 1 < arguments.Length:
                output = arguments[++i];
                break;
            case "--help" or "-h":
                Console.WriteLine("""
                .NET Aspire Starter Project Scaffolder

                Usage:
                    ScaffoldAspireProject --name <name> --description <desc> --output <path>

                Options:
                    --name          Project name (PascalCase, e.g., 'TaskManager')
                    --description   Brief project description
                    --output        Output directory (default: current directory)
                    --help, -h      Show this help message

                Example:
                    ScaffoldAspireProject \
                        --name "TaskManager" \
                        --description "A task management application for teams" \
                        --output "./projects"
                """);
                Environment.Exit(0);
                break;
        }
    }
    
    if (string.IsNullOrWhiteSpace(name))
    {
        Console.Error.WriteLine("Error: --name is required");
        Environment.Exit(1);
    }
    
    if (string.IsNullOrWhiteSpace(description))
    {
        Console.Error.WriteLine("Error: --description is required");
        Environment.Exit(1);
    }
    
    return new ProjectConfig(name, description, Path.GetFullPath(output));
}

// ============================================================================
// Scaffolding Logic
// ============================================================================

async Task ScaffoldProject(ProjectConfig config)
{
    var root = Path.Combine(config.OutputPath, config.Name);
    
    Console.WriteLine($"\n🚀 Scaffolding {config.Name}...");
    Console.WriteLine($"   Output: {root}\n");
    
    // Step 1: Run dotnet new aspire-starter to create base project
    Console.WriteLine("📦 Creating base Aspire project structure...");
    await RunCommand("dotnet", new[] { "new", "aspire-starter", "--name", config.Name, "--output", root });
    
    // Step 2: Enhance generated project with our configurations and documentation
    Console.WriteLine("✨ Adding skill-specific enhancements...");
    await EnhanceProject(config, root);
    
    Console.WriteLine($"\n✅ Successfully created {config.Name}!");
    Console.WriteLine($"\nNext steps:");
    Console.WriteLine($"  1. cd {root}");
    Console.WriteLine($"  2. dotnet workload install aspire  # if not already installed");
    Console.WriteLine($"  3. dotnet restore");
    Console.WriteLine($"  4. dotnet run --project {config.Name}.AppHost");
    Console.WriteLine();
}

async Task EnhanceProject(ProjectConfig config, string projectRoot)
{
    // Create or enhance key files with our standards and configuration
    
    // Ensure .editorconfig exists with our standards
    var editorConfigPath = Path.Combine(projectRoot, ".editorconfig");
    if (!File.Exists(editorConfigPath))
    {
        await WriteFile(editorConfigPath, Templates.EditorConfig);
    }
    
    // Ensure Directory.Build.props exists with our standards
    var dirBuildPropsPath = Path.Combine(projectRoot, "Directory.Build.props");
    if (!File.Exists(dirBuildPropsPath))
    {
        await WriteFile(dirBuildPropsPath, Templates.DirectoryBuildProps);
    }
    
    // Add documentation and context files
    await WriteFile(Path.Combine(projectRoot, ".github", "copilot-instructions.md"), 
        Templates.CopilotInstructions(config));
    await WriteFile(Path.Combine(projectRoot, "docs", "ARCHITECTURE.md"), 
        Templates.ArchitectureDoc(config));
    
    // Enhance VS Code configuration
    await WriteFile(Path.Combine(projectRoot, ".vscode", "settings.json"), 
        Templates.VsCodeSettings(config));
    await WriteFile(Path.Combine(projectRoot, ".vscode", "launch.json"), 
        Templates.VsCodeLaunch(config));
    await WriteFile(Path.Combine(projectRoot, ".vscode", "tasks.json"), 
        Templates.VsCodeTasks(config));
    
    Console.WriteLine($"  ✓ Added coding standards (.editorconfig, Directory.Build.props)");
    Console.WriteLine($"  ✓ Added AI context (.github/copilot-instructions.md, docs/ARCHITECTURE.md)");
    Console.WriteLine($"  ✓ Enhanced VS Code configuration");
}

async Task RunCommand(string fileName, string[] args)
{
    var process = new Process
    {
        StartInfo = new ProcessStartInfo
        {
            FileName = fileName,
            Arguments = string.Join(" ", args),
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        }
    };

    process.Start();
    var output = await process.StandardOutput.ReadToEndAsync();
    var error = await process.StandardError.ReadToEndAsync();
    await process.WaitForExitAsync();

    if (process.ExitCode != 0)
    {
        Console.Error.WriteLine($"Error running {fileName}: {error}");
        if (!string.IsNullOrEmpty(error))
            throw new InvalidOperationException($"Command failed: {error}");
    }
    
    if (!string.IsNullOrEmpty(output))
        Console.WriteLine(output);
}

async Task WriteFile(string path, string content)
{
    var dir = Path.GetDirectoryName(path);
    if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
    {
        Directory.CreateDirectory(dir);
    }
    await File.WriteAllTextAsync(path, content.TrimStart());
    Console.WriteLine($"  Created: {path}");
}
