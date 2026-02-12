# Purpose
This is just a curated collection of Agent Skills from other repos and a mix of my own for only the sake of convenience for use with the Awesome Copilot extension making it simple to import prompts, skills, and instructions to other projects to inform LLMs on best practices while developing software.

# Folder Structure for Awesome-Copilot support

prompts/         Task-specific prompts (.prompt.md)
instructions/    Coding standards and best practices (.instructions.md)
agents/          AI personas and specialized modes (.agent.md)
collections/     Curated collections of related items (.collection.yml)
scripts/         Utility scripts for maintenance
skills/          AI capabilities for specialized tasks

# Custom Skills

This repository includes custom-built skills designed to extend AI agent capabilities in specific domains:

## DocsExplorer

**Description**: Documentation lookup specialist that proactively fetches up-to-date documentation for libraries, frameworks, and technologies.

**Key Features**:
- **Parallel Documentation Fetching**: Batches multiple technology lookups simultaneously for maximum speed
- **Context7 MCP Integration**: Uses LLM-optimized documentation as primary source
- **Smart Fallback Strategy**: Automatically falls back to websearch when Context7 lacks coverage
- **Machine-Readable Formats**: Prefers LLMS.txt and Markdown over HTML for better parsing
- **Multi-Technology Support**: Handles web frameworks, languages, cloud tools, and more

**Location**: `skills/docs-explorer/`

Use this skill when you need documentation for update-to-date documentation for a given library/framework. It will automatically fetch docs in parallel and provide structured, actionable information.

# References - Get Up-to-Date Skills from Their Sources:
https://github.com/github/awesome-copilot
https://github.com/muratcankoylan
https://github.com/anthropics/skills
https://github.com/Prat011/awesome-llm-skills
https://github.com/obra/superpowers
https://github.com/softaworks/agent-toolkit