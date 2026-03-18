# zip-folder

A Bun TypeScript script that zips a folder into a ZIP file, using the folder's name as the filename. Configured ignore directories (e.g. `node_modules`) are reliably excluded at any depth with clear console output showing exactly what was skipped.

## Requirements

- [Bun](https://bun.sh) v1.0+
- [jszip](https://www.npmjs.com/package/jszip)

```bash
bun add jszip
```

## Usage

```bash
bun zip-folder.ts <folder-path> [config-path] [--dest <dest-dir>]
```

| Argument      | Required | Description                                                                 |
|---------------|----------|-----------------------------------------------------------------------------|
| `folder-path` | ✅       | Path to the folder you want to zip                                          |
| `config-path` | ❌       | Path to a custom ignore config JSON. Defaults to `zip-ignore.json` in the same directory as the script. |
| `--dest`      | ❌       | Destination directory for the output ZIP file. Defaults to the current working directory. |

The output ZIP is named after the target folder (e.g. `VaultPad.zip`) and written to the destination directory.

### Examples

```bash
# Basic usage
bun zip-folder.ts ~/projects/VaultPad

# Custom config location
bun zip-folder.ts ~/projects/VaultPad ~/configs/my-zip-ignore.json

# Custom destination directory
bun zip-folder.ts ~/projects/VaultPad --dest ~/Desktop

# Custom config and destination
bun zip-folder.ts ~/projects/VaultPad ~/configs/my-zip-ignore.json --dest /tmp/output

# Zip a relative path
bun zip-folder.ts ./my-app
```

## Configuration

Ignored directories are loaded from a JSON file — an array of directory name strings. Any directory matching a name in the list will be skipped at **any depth** in the folder tree.

**Default `zip-ignore.json`:**

```json
["node_modules", ".git", "dist", ".next", ".nuxt", "build", "coverage", ".turbo"]
```

Place `zip-ignore.json` in the same directory as `zip-folder.ts`, or pass a custom path as the second argument.

## Output

```
📦  Zipping folder : /Users/molt/.openclaw/workspace/repos/VaultPad
💾  Output file    : /Users/molt/VaultPad.zip
🚫  Ignoring dirs  : node_modules, .git, dist, .next

🔍  Scanning...

🚫  Skipped 3 ignored directories:
    ↳ node_modules
    ↳ packages/ui/node_modules
    ↳ packages/api/node_modules

📄  Files to compress : 312
⏳  Compressing...

✅  Done! /Users/molt/VaultPad.zip (1.24 MB)
```

## Files

| File              | Description                              |
|-------------------|------------------------------------------|
| `zip-folder.ts`   | Main script                              |
| `zip-ignore.json` | List of directory names to exclude       |