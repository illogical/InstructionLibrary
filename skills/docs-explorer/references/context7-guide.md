# Context7 MCP Usage Guide

Detailed guide for using Context7 as the primary documentation source in DocsExplorer skill.

## What is Context7?

Context7 is a Model Context Protocol (MCP) server that provides LLM-optimized documentation for popular libraries, frameworks, and technologies. It's designed specifically for AI agents to efficiently retrieve and understand technical documentation.

**GitHub**: https://github.com/upstash/context7

## Key Features

- **LLM-Optimized**: Documentation formatted for language model consumption
- **Up-to-date**: Regularly updated from official sources
- **Comprehensive Coverage**: Popular libraries across Python, JavaScript, TypeScript, Go, Rust, and more
- **LLMS.txt Format**: Machine-readable documentation format
- **Fast Retrieval**: Optimized for quick lookups

## Supported Technologies

Context7 maintains documentation for:

### Web Frameworks
- React, Vue, Angular, Svelte
- Next.js, Nuxt, SvelteKit
- Express.js, Fastify, Koa
- FastAPI, Flask, Django
- Ruby on Rails, Laravel

### Languages & Tools
- Python (standard library + popular packages)
- JavaScript/TypeScript
- Go, Rust, C#
- PostgreSQL, MySQL, MongoDB, Redis, Convex, Supabase

### Cloud & Infrastructure
- AWS services, Azure
- Docker, Kubernetes
- Terraform, Ansible

### Testing & Development
- Jest, Vitest, Pytest
- Playwright, Cypress
- ESLint, Prettier

## Query Patterns

### Basic Query

```
Query: "[library] documentation"
Example: "React documentation"
Example: "FastAPI documentation"
```

### Specific Topic Query

```
Query: "[library] [specific topic]"
Example: "React hooks documentation"
Example: "FastAPI websockets configuration"
```

### API Reference Query

```
Query: "[library] [method/class/function] API"
Example: "Stripe checkout session API"
Example: "Django QuerySet filter API"
```

## Integration with MCP

When using Context7 through MCP:

1. **Primary lookup method**: Always try Context7 first
2. **Batch queries**: Execute multiple Context7 queries in parallel
3. **Handle failures gracefully**: If Context7 returns empty, fallback to websearch

```python
# Pseudo-code pattern
results = parallel_execute([
    context7_query("React hooks"),
    context7_query("TypeScript generics"),
    context7_query("FastAPI middleware")
])

for result in results:
    if result.empty:
        # Fallback to websearch for this technology
        fallback_results = websearch(f"{technology} official documentation")
```

## Response Format

Context7 typically returns:
- **Markdown-formatted content**: Clean, structured documentation
- **Code examples**: Syntax-highlighted snippets
- **Version information**: When available
- **Source links**: References to official documentation

## Best Practices

### Do's

✓ **Use specific queries**: "FastAPI CORS middleware" > "FastAPI"
✓ **Query in parallel**: Batch multiple technology lookups
✓ **Include context**: Add relevant keywords to narrow results
✓ **Verify versions**: Check if version-specific docs are needed

### Don'ts

✗ **Don't query sequentially**: Always batch parallel requests
✗ **Don't be too vague**: "web framework" won't return useful results
✗ **Don't assume coverage**: Have fallback ready for niche libraries
✗ **Don't ignore context**: "useState" alone is ambiguous, use "React useState"

## Coverage Limitations

Context7 may not have:
- **Very new libraries** (released in last few weeks)
- **Niche/specialized tools** (internal company libraries)
- **Legacy versions** (old, unsupported versions)
- **Private/proprietary tools** (closed-source, paid-only docs)

**When to fallback**:
- Empty results from Context7
- Documentation is outdated (check version numbers)
- Need for very specific/advanced topics not covered

## Example Workflows

### Single Technology Deep Dive

```
User: "Explain React Server Components"

Workflow:
1. Context7 query: "React Server Components documentation"
2. Parse response for:
   - Overview and concepts
   - Usage patterns
   - Code examples
   - Best practices
3. Present structured summary with examples
4. Include link to official React docs for deeper exploration
```

### Multiple Related Technologies

```
User: "Authentication with Next.js and NextAuth"

Workflow (parallel):
1. Context7 query: "Next.js authentication"
2. Context7 query: "NextAuth.js configuration"
3. Consolidate results:
   - How they integrate
   - Configuration steps
   - Code examples
4. Present unified authentication guide
```

### Comparison Query

```
User: "Compare Express.js vs Fastify performance"

Workflow (parallel):
1. Context7 query: "Express.js performance optimization"
2. Context7 query: "Fastify performance benchmarks"
3. Supplement with websearch: "Express vs Fastify benchmark comparison"
4. Present: Key differences + performance data + use cases
```

## Advanced Filtering

When available through Context7 MCP:

### By Version

Some queries support version filtering:
```
Query: "React 18 new features"
Query: "Python 3.11 documentation"
```

### By Module/Package

For large frameworks:
```
Query: "Django REST framework serializers"
Query: "AWS SDK S3 documentation"
Query: "Lodash array methods"
```

### By Use Case

Task-oriented queries:
```
Query: "FastAPI background tasks"
Query: "React form validation"
Query: "PostgreSQL full-text search"
```

## Token Configuration

Context7 requires an API token. In DocsExplorer skill:
- **Assume token is pre-configured** in MCP settings
- **Do not prompt user for token** during documentation lookup
- **Report connection errors** if MCP unavailable

## Troubleshooting Context7

### Empty Results

**Problem**: Context7 returns no documentation

**Solutions**:
1. Try broader query: "FastAPI middleware" → "FastAPI documentation"
2. Check spelling and library name accuracy
3. Fallback to websearch immediately

### Outdated Information

**Problem**: Documentation version doesn't match current library version

**Solutions**:
1. Mention version mismatch in response
2. Supplement with websearch for latest docs
3. Provide link to official documentation

### Connection Issues

**Problem**: Cannot connect to Context7 MCP

**Solutions**:
1. Report connection error to user
2. Fallback to websearch/webfetch exclusively
3. Suggest checking MCP configuration (only if persistent)

## Performance Tips

1. **Batch aggressively**: Context7 handles parallel requests efficiently
2. **Cache-friendly queries**: Consistent query patterns improve performance
3. **Specificity balance**: Too specific may miss results, too broad returns noise
4. **Combine with skill tool**: Use Skill tool to access other documentation skills when needed

## Context7 vs Other Sources

| Source | Pros | Cons | When to Use |
|--------|------|------|-------------|
| **Context7** | LLM-optimized, fast, comprehensive | May lack very new/niche libraries | Primary source, always try first |
| **Websearch** | Finds anything, very current | Requires filtering, quality varies | Fallback when Context7 lacks coverage |
| **Webfetch** | Direct access to official docs | May need HTML parsing, slower | When official doc URLs are known |
| **GitHub** | Source of truth, always current | Format not LLM-optimized | For very new libraries, examples |

## Integration Checklist

When using Context7 in DocsExplorer:

- [ ] Query Context7 first for all standard libraries/frameworks
- [ ] Execute queries in parallel for multiple technologies
- [ ] Include specific topics/features in query when known
- [ ] Implement fallback to websearch for empty results
- [ ] Prefer LLMS.txt and markdown formats in results
- [ ] Include source attribution and links in response
- [ ] Handle connection errors gracefully
- [ ] Report version information when available

---

**Last Updated**: February 2026
**Context7 Version**: Latest (check GitHub for updates)
