#!/usr/bin/env bun
/**
 * zip-folder.ts
 *
 * Zips a folder into a ZIP file using the folder's name as the filename.
 * Directories listed in zip-ignore.json are excluded at any depth.
 *
 * Usage: bun zip-folder.ts <folder-path> [config-path] [--dest <dest-dir>]
 *   folder-path  - Path to the folder to zip
 *   config-path  - (Optional) Path to ignore config JSON. Defaults to zip-ignore.json
 *                  in the same directory as this script.
 *   --dest       - (Optional) Destination directory for the ZIP file. Defaults to cwd.
 */

import { resolve, basename, join, dirname, relative } from "path";
import { existsSync, statSync, readdirSync, readFileSync } from "fs";
import { fileURLToPath } from "url";
import JSZip from "jszip";
import { writeFileSync } from "fs";

// ---------------------------------------------------------------------------
// Resolve args
// ---------------------------------------------------------------------------

const args = process.argv.slice(2);

const destFlagIndex = args.indexOf("--dest");
let destArg: string | undefined;
if (destFlagIndex !== -1) {
  destArg = args[destFlagIndex + 1];
  args.splice(destFlagIndex, 2);
}

const [folderArg, configArg] = args;

if (!folderArg) {
  console.error("❌  Usage: bun zip-folder.ts <folder-path> [config-path] [--dest <dest-dir>]");
  process.exit(1);
}

const expandedFolderArg = folderArg.startsWith("~/")
  ? folderArg.replace("~", process.env.HOME ?? "~")
  : folderArg;
const folderPath = resolve(expandedFolderArg);

try {
  if (!statSync(folderPath).isDirectory()) {
    console.error(`❌  Not a directory: ${folderPath}`);
    process.exit(1);
  }
} catch (err: any) {
  if (err.code === "ENOENT") {
    console.error(`❌  Path does not exist: ${folderPath}`);
  } else if (err.code === "EACCES") {
    console.error(`❌  Permission denied: ${folderPath}`);
    console.error(`    Grant Full Disk Access to your terminal in System Settings → Privacy & Security.`);
  } else {
    console.error(`❌  Cannot access path: ${folderPath} (${err.code ?? err.message})`);
  }
  process.exit(1);
}

// ---------------------------------------------------------------------------
// Load ignore config
// ---------------------------------------------------------------------------

const scriptDir = dirname(fileURLToPath(import.meta.url));
const configPath = configArg
  ? resolve(configArg)
  : join(scriptDir, "zip-ignore.json");

let ignoreDirs: string[] = ["node_modules"];

if (existsSync(configPath)) {
  try {
    const raw = readFileSync(configPath, "utf-8");
    const parsed = JSON.parse(raw);
    if (Array.isArray(parsed) && parsed.every((x) => typeof x === "string")) {
      ignoreDirs = parsed;
    } else {
      console.warn("⚠️  Config must be a JSON array of strings. Using defaults.");
    }
  } catch {
    console.warn(`⚠️  Could not parse config at ${configPath}. Using defaults.`);
  }
} else {
  console.warn(`⚠️  No config found at ${configPath}. Using default: ["node_modules"]`);
}

// ---------------------------------------------------------------------------
// Print summary
// ---------------------------------------------------------------------------

const folderName = basename(folderPath).replace(/^\./, "");
const destDir = destArg ? resolve(destArg) : process.cwd();
const outputZip = resolve(destDir, `${folderName}.zip`);

console.log("");
console.log(`📦  Zipping folder : ${folderPath}`);
console.log(`💾  Output file    : ${outputZip}`);
console.log(`🚫  Ignoring dirs  : ${ignoreDirs.map((d) => `\x1b[33m${d}\x1b[0m`).join(", ")}`);
console.log("");

// ---------------------------------------------------------------------------
// Walk the directory tree, skipping ignored dirs
// ---------------------------------------------------------------------------

const ignoredPaths: string[] = [];
let fileCount = 0;

/**
 * Recursively walks a directory, collecting file paths while pruning ignored dirs.
 * @param dir   Absolute path of the current directory being walked.
 * @param zip   JSZip instance to add files/folders to.
 * @param base  Relative base path within the zip archive.
 */
function walk(dir: string, zip: JSZip, base: string): void {
  const entries = readdirSync(dir, { withFileTypes: true });

  for (const entry of entries) {
    const fullPath = join(dir, entry.name);
    const zipPath = join(base, entry.name);

    if (entry.isDirectory()) {
      if (ignoreDirs.includes(entry.name)) {
        // Record the ignored path for display (relative to input folder)
        ignoredPaths.push(relative(folderPath, fullPath));
        continue;
      }
      walk(fullPath, zip.folder(entry.name)!, zipPath);
    } else {
      zip.file(entry.name, readFileSync(fullPath));
      fileCount++;
    }
  }
}

// ---------------------------------------------------------------------------
// Build & write the ZIP
// ---------------------------------------------------------------------------

const zip = new JSZip();
const rootFolder = zip.folder(folderName)!;

console.log("🔍  Scanning...\n");
walk(folderPath, rootFolder, folderName);

// Print each ignored path
if (ignoredPaths.length > 0) {
  console.log(`🚫  Skipped ${ignoredPaths.length} ignored director${ignoredPaths.length === 1 ? "y" : "ies"}:`);
  for (const p of ignoredPaths) {
    console.log(`    \x1b[90m↳ ${p}\x1b[0m`);
  }
  console.log("");
}

console.log(`📄  Files to compress : ${fileCount}`);
console.log("⏳  Compressing...\n");

const content = await zip.generateAsync({
  type: "nodebuffer",
  compression: "DEFLATE",
  compressionOptions: { level: 6 },
});

writeFileSync(outputZip, content);

const sizeMB = (content.byteLength / 1024 / 1024).toFixed(2);
console.log(`✅  Done! \x1b[32m${outputZip}\x1b[0m (${sizeMB} MB)`);