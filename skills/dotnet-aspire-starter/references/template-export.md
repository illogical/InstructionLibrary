# Template Export Guide

Convert your scaffolded project into reusable VS Code and Visual Studio templates.

## Creating a `dotnet new` Template

Make your project available as a `dotnet new` template for quick project creation.

### Step 1: Create Template Configuration

In your project root, create `.template.config/template.json`:

```json
{
  "$schema": "http://json.schemastore.org/template",
  "author": "Your Name",
  "classifications": [ "Web", "Aspire", "API" ],
  "identity": "YourCompany.AspireStarter",
  "name": ".NET Aspire Starter",
  "shortName": "aspire-starter",
  "tags": {
    "language": "C#",
    "type": "project"
  },
  "sourceName": "YourProject",
  "preferNameDirectory": true,
  "symbols": {
    "ProjectName": {
      "type": "parameter",
      "datatype": "string",
      "defaultValue": "MyAspireProject",
      "replaces": "YourProject",
      "fileRename": "YourProject"
    },
    "Description": {
      "type": "parameter",
      "datatype": "string",
      "defaultValue": "A new Aspire project",
      "replaces": "PROJECT_DESCRIPTION"
    }
  },
  "sources": [
    {
      "modifiers": [
        {
          "exclude": [
            "**/bin/**",
            "**/obj/**",
            "**/*.db",
            "**/*.db-*",
            "**/node_modules/**",
            "**/.vs/**",
            "**/.vscode/**"
          ]
        }
      ]
    }
  ]
}
```

### Step 2: Test Template Locally

Install the template from your project directory:

```bash
dotnet new install ./
```

Verify it appears in the template list:

```bash
dotnet new list | grep aspire
```

Create a new project from the template:

```bash
dotnet new aspire-starter --name "TestProject" --Description "Testing the template"
```

### Step 3: Uninstall for Updates

When making changes to the template:

```bash
# Uninstall
dotnet new uninstall YourCompany.AspireStarter

# Reinstall
dotnet new install ./
```

### Step 4: Package for Distribution

Create a NuGet package for sharing:

Create `template.csproj`:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <PackageType>Template</PackageType>
    <PackageVersion>1.0.0</PackageVersion>
    <PackageId>YourCompany.AspireStarter.Template</PackageId>
    <Title>.NET Aspire Starter Template</Title>
    <Authors>Your Name</Authors>
    <Description>Production-ready .NET Aspire project template with MVC, API, EF Core, OpenTelemetry, and Polly</Description>
    <PackageTags>aspire;dotnet;template;mvc;api</PackageTags>
    <IncludeContentInPack>true</IncludeContentInPack>
    <IncludeBuildOutput>false</IncludeBuildOutput>
    <ContentTargetFolders>content</ContentTargetFolders>
    <NoWarn>$(NoWarn);NU5128</NoWarn>
  </PropertyGroup>

  <ItemGroup>
    <Content Include="**\*" Exclude="**\bin\**;**\obj\**;**\*.user;**\.vs\**" />
    <Compile Remove="**\*" />
  </ItemGroup>
</Project>
```

Pack the template:

```bash
dotnet pack
```

Install from the package:

```bash
dotnet new install ./bin/Debug/YourCompany.AspireStarter.Template.1.0.0.nupkg
```

Publish to NuGet.org:

```bash
dotnet nuget push ./bin/Debug/YourCompany.AspireStarter.Template.1.0.0.nupkg --api-key YOUR_API_KEY --source https://api.nuget.org/v3/index.json
```

### Step 5: Use the Published Template

Others can install your template:

```bash
dotnet new install YourCompany.AspireStarter.Template
dotnet new aspire-starter --name "MyNewProject"
```

## Creating a VS Code Extension

Package your template as a VS Code extension for one-click project creation.

### Prerequisites

```bash
npm install -g yo generator-code vsce
```

### Step 1: Generate Extension Scaffold

```bash
yo code
```

Choose:
- **New Extension**
- Name: `aspire-starter-template`
- Description: `.NET Aspire project template`
- TypeScript

### Step 2: Add Template Files

Copy your project to `templates/aspire-starter/`:

```bash
mkdir -p aspire-starter-template/templates/aspire-starter
cp -r YourProject/* aspire-starter-template/templates/aspire-starter/
```

### Step 3: Create Extension Command

Edit `src/extension.ts`:

```typescript
import * as vscode from 'vscode';
import * as fs from 'fs';
import * as path from 'path';

export function activate(context: vscode.ExtensionContext) {
    let disposable = vscode.commands.registerCommand('aspire-starter.create', async () => {
        const projectName = await vscode.window.showInputBox({
            prompt: 'Project Name',
            value: 'MyAspireProject'
        });

        if (!projectName) {
            return;
        }

        const description = await vscode.window.showInputBox({
            prompt: 'Project Description',
            value: 'A new Aspire project'
        });

        const targetFolder = await vscode.window.showOpenDialog({
            canSelectFolders: true,
            canSelectFiles: false,
            canSelectMany: false,
            openLabel: 'Select Folder'
        });

        if (!targetFolder || !targetFolder[0]) {
            return;
        }

        const targetPath = path.join(targetFolder[0].fsPath, projectName);
        const templatePath = path.join(context.extensionPath, 'templates', 'aspire-starter');

        // Copy template
        await copyRecursive(templatePath, targetPath);

        // Replace placeholders
        await replacePlaceholders(targetPath, projectName, description || '');

        vscode.window.showInformationMessage(`Created ${projectName} at ${targetPath}`);
        
        // Open the new project
        vscode.commands.executeCommand('vscode.openFolder', vscode.Uri.file(targetPath), true);
    });

    context.subscriptions.push(disposable);
}

async function copyRecursive(src: string, dest: string) {
    const stats = fs.statSync(src);
    
    if (stats.isDirectory()) {
        fs.mkdirSync(dest, { recursive: true });
        const entries = fs.readdirSync(src);
        
        for (const entry of entries) {
            if (entry === 'bin' || entry === 'obj' || entry === 'node_modules' || entry === '.vs') {
                continue;
            }
            await copyRecursive(path.join(src, entry), path.join(dest, entry));
        }
    } else {
        fs.copyFileSync(src, dest);
    }
}

async function replacePlaceholders(dir: string, projectName: string, description: string) {
    const entries = fs.readdirSync(dir);
    
    for (const entry of entries) {
        const fullPath = path.join(dir, entry);
        const stats = fs.statSync(fullPath);
        
        if (stats.isDirectory()) {
            // Rename directories
            if (entry.includes('YourProject')) {
                const newName = entry.replace(/YourProject/g, projectName);
                const newPath = path.join(dir, newName);
                fs.renameSync(fullPath, newPath);
                await replacePlaceholders(newPath, projectName, description);
            } else {
                await replacePlaceholders(fullPath, projectName, description);
            }
        } else if (stats.isFile()) {
            // Rename files
            if (entry.includes('YourProject')) {
                const newName = entry.replace(/YourProject/g, projectName);
                const newPath = path.join(dir, newName);
                
                // Replace content
                let content = fs.readFileSync(fullPath, 'utf8');
                content = content.replace(/YourProject/g, projectName);
                content = content.replace(/PROJECT_DESCRIPTION/g, description);
                
                fs.writeFileSync(newPath, content);
                fs.unlinkSync(fullPath);
            } else {
                // Just replace content
                let content = fs.readFileSync(fullPath, 'utf8');
                content = content.replace(/YourProject/g, projectName);
                content = content.replace(/PROJECT_DESCRIPTION/g, description);
                fs.writeFileSync(fullPath, content);
            }
        }
    }
}
```

### Step 4: Update package.json

Add command to `package.json`:

```json
{
  "contributes": {
    "commands": [
      {
        "command": "aspire-starter.create",
        "title": "Create New .NET Aspire Project"
      }
    ]
  }
}
```

### Step 5: Package Extension

```bash
vsce package
```

This creates `aspire-starter-template-1.0.0.vsix`.

Install locally:

```bash
code --install-extension aspire-starter-template-1.0.0.vsix
```

Or publish to VS Code Marketplace:

```bash
vsce publish
```

## Visual Studio Project Template (Windows)

### Step 1: Export as Template

1. Open your project in Visual Studio
2. **Project** → **Export Template**
3. Choose **Project template**
4. Fill in template details:
   - Name: .NET Aspire Starter
   - Description: Production-ready Aspire project
   - Icon and Preview images (optional)
5. Finish the wizard

The template is saved to `%USERPROFILE%\Documents\Visual Studio 2022\Templates\ProjectTemplates\`

### Step 2: Use the Template

1. **File** → **New** → **Project**
2. Search for your template name
3. Create a new project

### Step 3: Share the Template

Zip the template folder and share:

```
AspireStarter.zip
├── MyTemplate.vstemplate
├── __TemplateIcon.ico
└── (project files)
```

Others can extract to their ProjectTemplates folder.

## Maintaining Templates

### Version Control

Create a separate repository for your template:

```bash
git init aspire-starter-template
cd aspire-starter-template
git add .
git commit -m "Initial template"
git remote add origin https://github.com/yourusername/aspire-starter-template.git
git push -u origin main
```

### Update Process

1. Make changes to your template
2. Update version in `.template.config/template.json` or `package.json`
3. Test the template locally
4. Publish updates:
   - NuGet: `dotnet pack && dotnet nuget push ...`
   - VS Code: `vsce publish`
   - Visual Studio: Re-export template

### Documentation

Include a README in your template:

```markdown
# .NET Aspire Starter Template

## Usage

dotnet new install YourCompany.AspireStarter.Template
dotnet new aspire-starter --name "MyProject" --Description "My awesome project"

## Features

- ASP.NET MVC + Razor + TypeScript
- REST API with Entity Framework Core
- OpenTelemetry observability
- Polly resilience
- OIDC authentication ready

## Requirements

- .NET 10 SDK
- Aspire workload: `dotnet workload install aspire`
- Node.js 18+ (for TypeScript)

## Next Steps

See [docs/initial-setup.md](docs/initial-setup.md) for configuration steps.
```

## Reference

- [.NET Custom Templates](https://learn.microsoft.com/en-us/dotnet/core/tools/custom-templates)
- [VS Code Extension API](https://code.visualstudio.com/api)
- [Visual Studio Project Templates](https://learn.microsoft.com/en-us/visualstudio/ide/creating-project-and-item-templates)
