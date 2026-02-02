# Git Hooks Setup Guide

This guide explains how to set up optional git hooks for automating documentation reminders. Git hooks can help ensure documentation stays synchronized with code by providing reminders or enforcement at key points in the git workflow.

**Note**: Git hooks are **optional**. The maintain-dev-docs skill works perfectly well with just manual reminders and the check-reminders.ts script.

## Table of Contents

1. [Understanding Git Hooks](#understanding-git-hooks)
2. [Pre-Commit Hook](#pre-commit-hook)
3. [Pre-Push Hook](#pre-push-hook)
4. [Commit-Msg Hook](#commit-msg-hook)
5. [Setup Instructions](#setup-instructions)
6. [Bypassing Hooks](#bypassing-hooks)
7. [Team Setup](#team-setup)
8. [Troubleshooting](#troubleshooting)

---

## Understanding Git Hooks

### What Are Git Hooks?

Git hooks are scripts that Git executes automatically before or after specific events such as:
- Before committing (`pre-commit`)
- After committing (`post-commit`)
- Before pushing (`pre-push`)
- After receiving a commit message (`commit-msg`)

### Why Use Hooks for Documentation?

**Benefits**:
- **Automated reminders** - Don't forget to update docs
- **Consistency** - Everyone on team gets same reminders
- **Early detection** - Catch missing docs before they're pushed
- **Habit formation** - Makes documentation updates routine

**Drawbacks**:
- **Adds friction** - Slows down commit/push process slightly
- **Can be bypassed** - Developers can skip hooks if needed
- **Requires setup** - Each team member must install hooks
- **May interrupt flow** - Some developers prefer manual checking

### When to Use Hooks

**Use git hooks if**:
- Team frequently forgets to update documentation
- Documentation drift is a recurring problem
- Team agrees to documentation-first workflow
- You want automated enforcement

**Don't use git hooks if**:
- Team is small (1-2 people) and disciplined about docs
- Manual reminders with check-reminders.ts script are sufficient
- Team prefers flexibility over enforcement
- Hooks would cause more frustration than value

---

## Pre-Commit Hook

### Purpose

The pre-commit hook runs **before a commit is created**. It can:
- Remind you to update documentation
- Check if documentation files have been modified
- Prompt you to run the reminder checklist
- Prevent commits if documentation is missing (optional)

### Hook Script (Reminder Mode)

Create `.git/hooks/pre-commit`:

```bash
#!/bin/bash

# Pre-commit hook for documentation reminders
# This hook REMINDS but doesn't block commits

echo ""
echo "🔍 Pre-Commit Documentation Check"
echo "=================================="
echo ""

# Check if any code files were modified
CODE_FILES_MODIFIED=$(git diff --cached --name-only | grep -E '\.(ts|js|jsx|tsx|py|java|go|rs|c|cpp|cs)$')

if [ -n "$CODE_FILES_MODIFIED" ]; then
    echo "✏️  Code files modified in this commit:"
    echo "$CODE_FILES_MODIFIED" | sed 's/^/   - /'
    echo ""

    # Check if any documentation files were also modified
    DOC_FILES_MODIFIED=$(git diff --cached --name-only | grep -E '^docs/')

    if [ -z "$DOC_FILES_MODIFIED" ]; then
        echo "⚠️  No documentation files modified."
        echo ""
        echo "📋 Documentation Reminder Checklist:"
        echo "   - Does specification.md need updating? (new tech/services)"
        echo "   - Does tasks.md need updating? (new/completed tasks)"
        echo "   - Does PHASE#.md need updating? (implementation details)"
        echo "   - Does README.md need updating? (user-facing changes)"
        echo ""
        echo "💡 Run: npm run check-docs (or ts-node docs/check-reminders.ts)"
        echo ""

        # Optional: Prompt user
        read -p "Continue with commit? (y/n) " -n 1 -r
        echo ""
        if [[ ! $REPLY =~ ^[Yy]$ ]]; then
            echo "❌ Commit aborted. Update documentation and try again."
            exit 1
        fi
        echo "✅ Proceeding with commit..."
    else
        echo "✅ Documentation files also modified:"
        echo "$DOC_FILES_MODIFIED" | sed 's/^/   - /'
        echo ""
        echo "✅ Documentation updated. Great job!"
    fi
else
    echo "ℹ️  No code files modified in this commit."
fi

echo ""
exit 0
```

Make it executable:
```bash
chmod +x .git/hooks/pre-commit
```

### Hook Script (Enforcement Mode)

If you want to **prevent commits** without documentation updates:

```bash
#!/bin/bash

# Pre-commit hook for documentation enforcement
# This hook BLOCKS commits if documentation is not updated

echo ""
echo "🔍 Pre-Commit Documentation Check (Enforcement Mode)"
echo "====================================================="
echo ""

# Check if any code files were modified
CODE_FILES_MODIFIED=$(git diff --cached --name-only | grep -E '\.(ts|js|jsx|tsx|py|java|go|rs|c|cpp|cs)$')

if [ -n "$CODE_FILES_MODIFIED" ]; then
    echo "✏️  Code files modified in this commit."
    echo ""

    # Check if any documentation files were also modified
    DOC_FILES_MODIFIED=$(git diff --cached --name-only | grep -E '^docs/')

    if [ -z "$DOC_FILES_MODIFIED" ]; then
        echo "❌ ERROR: Code modified but no documentation updated!"
        echo ""
        echo "📋 Required Actions:"
        echo "   1. Review your changes"
        echo "   2. Update relevant documentation files in /docs/"
        echo "   3. Stage documentation changes: git add docs/"
        echo "   4. Try commit again"
        echo ""
        echo "💡 Or bypass this check: git commit --no-verify"
        echo ""
        exit 1
    else
        echo "✅ Documentation files updated:"
        echo "$DOC_FILES_MODIFIED" | sed 's/^/   - /'
        echo ""
        echo "✅ Commit approved!"
    fi
fi

echo ""
exit 0
```

**Note**: Enforcement mode can be frustrating. Start with reminder mode and only enforce if necessary.

---

## Pre-Push Hook

### Purpose

The pre-push hook runs **before pushing to remote**. It can:
- Run the full reminder checklist
- Check documentation completeness
- Verify critical files exist
- Run automated documentation validation (future enhancement)

### Hook Script

Create `.git/hooks/pre-push`:

```bash
#!/bin/bash

# Pre-push hook for comprehensive documentation check
# Runs the full reminder checklist before pushing

echo ""
echo "🚀 Pre-Push Documentation Check"
echo "================================"
echo ""

# Check if docs folder exists
if [ ! -d "docs" ]; then
    echo "⚠️  Warning: /docs/ folder doesn't exist!"
    echo "Consider creating documentation for this project."
    echo ""
    read -p "Continue with push? (y/n) " -n 1 -r
    echo ""
    if [[ ! $REPLY =~ ^[Yy]$ ]]; then
        exit 1
    fi
    exit 0
fi

# Check if check-reminders script exists
if [ -f "scripts/check-reminders.ts" ]; then
    echo "📋 Running documentation reminder checklist..."
    echo ""

    # Run the reminder script
    npx ts-node scripts/check-reminders.ts

    echo ""
    echo "👆 Please review the checklist above."
    echo ""
    read -p "Have you verified all documentation is up to date? (y/n) " -n 1 -r
    echo ""
    if [[ ! $REPLY =~ ^[Yy]$ ]]; then
        echo "❌ Push aborted. Update documentation and try again."
        exit 1
    fi
    echo "✅ Documentation verified. Proceeding with push..."
else
    # Fallback if script doesn't exist - check for critical files
    echo "📋 Checking for critical documentation files..."
    echo ""

    MISSING_FILES=()

    [ ! -f "docs/specification.md" ] && MISSING_FILES+=("docs/specification.md")
    [ ! -f "docs/tasks.md" ] && MISSING_FILES+=("docs/tasks.md")
    [ ! -f "docs/README.md" ] && MISSING_FILES+=("docs/README.md")

    if [ ${#MISSING_FILES[@]} -gt 0 ]; then
        echo "⚠️  Missing critical documentation files:"
        for file in "${MISSING_FILES[@]}"; do
            echo "   - $file"
        done
        echo ""
        read -p "Continue with push anyway? (y/n) " -n 1 -r
        echo ""
        if [[ ! $REPLY =~ ^[Yy]$ ]]; then
            exit 1
        fi
    else
        echo "✅ Core documentation files exist."
    fi
fi

echo ""
echo "✅ Push approved!"
echo ""
exit 0
```

Make it executable:
```bash
chmod +x .git/hooks/pre-push
```

---

## Commit-Msg Hook

### Purpose

The commit-msg hook runs **after you write a commit message**. It can:
- Check if commit mentions documentation updates
- Suggest documentation keywords
- Enforce commit message format (optional)

### Hook Script

Create `.git/hooks/commit-msg`:

```bash
#!/bin/bash

# Commit-msg hook to encourage documentation mentions in commit messages

COMMIT_MSG_FILE=$1
COMMIT_MSG=$(cat "$COMMIT_MSG_FILE")

# Check if code files are being committed
CODE_FILES=$(git diff --cached --name-only | grep -E '\.(ts|js|jsx|tsx|py|java|go|rs|c|cpp|cs)$')

if [ -n "$CODE_FILES" ]; then
    # Check if documentation files are also being committed
    DOC_FILES=$(git diff --cached --name-only | grep -E '^docs/')

    if [ -n "$DOC_FILES" ]; then
        # Documentation was updated - check if commit message mentions it
        if ! echo "$COMMIT_MSG" | grep -qiE 'doc|documentation|spec|readme|phase|task'; then
            echo ""
            echo "💡 Tip: Consider mentioning documentation updates in commit message."
            echo ""
            echo "Example:"
            echo "  Add user authentication feature"
            echo "  "
            echo "  - Implements JWT-based authentication"
            echo "  - Updates docs/PHASE0.md with auth specs"
            echo "  - Updates docs/tasks.md with completion status"
            echo ""
            # This is just a suggestion, don't block the commit
        fi
    fi
fi

exit 0
```

Make it executable:
```bash
chmod +x .git/hooks/commit-msg
```

---

## Setup Instructions

### Quick Setup (Single Project)

1. **Navigate to your project**:
   ```bash
   cd /path/to/your/project
   ```

2. **Create hook file(s)**:
   ```bash
   # Pre-commit hook (reminder mode)
   curl -o .git/hooks/pre-commit https://[url-to-hook-script]
   chmod +x .git/hooks/pre-commit

   # Or create manually with content from above
   nano .git/hooks/pre-commit
   # Paste content, save, exit
   chmod +x .git/hooks/pre-commit
   ```

3. **Test the hook**:
   ```bash
   # Make a change to code
   echo "// test" >> src/index.ts
   git add src/index.ts
   git commit -m "test commit"

   # Hook should trigger and show reminder
   ```

4. **Adjust as needed**:
   - Edit hook scripts to match your preferences
   - Choose reminder vs. enforcement mode
   - Customize messages

### Setup for Team

**Challenge**: Git hooks are not version controlled. Each team member must install them manually.

**Solution**: Create a setup script.

#### Option 1: Setup Script in Repository

Create `scripts/setup-hooks.sh`:

```bash
#!/bin/bash

# Setup script for git hooks
# Run this after cloning the repository: bash scripts/setup-hooks.sh

echo "Setting up git hooks for documentation reminders..."
echo ""

# Create pre-commit hook
cat > .git/hooks/pre-commit << 'EOF'
#!/bin/bash
# [Insert pre-commit hook content here]
EOF

chmod +x .git/hooks/pre-commit
echo "✅ Pre-commit hook installed"

# Create pre-push hook
cat > .git/hooks/pre-push << 'EOF'
#!/bin/bash
# [Insert pre-push hook content here]
EOF

chmod +x .git/hooks/pre-push
echo "✅ Pre-push hook installed"

echo ""
echo "🎉 Git hooks setup complete!"
echo ""
echo "To bypass hooks when needed:"
echo "  git commit --no-verify"
echo "  git push --no-verify"
echo ""
```

Make it executable:
```bash
chmod +x scripts/setup-hooks.sh
```

**Add to README.md**:
```markdown
## Setup

After cloning this repository, run:
```bash
bash scripts/setup-hooks.sh
```

This installs git hooks that remind you to update documentation when committing code.
```

#### Option 2: Use Husky (Node.js Projects)

[Husky](https://typicode.github.io/husky/) is a popular tool for managing git hooks in Node.js projects.

**Install Husky**:
```bash
npm install --save-dev husky
npx husky install
```

**Add pre-commit hook**:
```bash
npx husky add .husky/pre-commit "bash scripts/pre-commit-docs.sh"
```

**Create script** `scripts/pre-commit-docs.sh`:
```bash
#!/bin/bash
# [Insert pre-commit hook logic here]
```

**Advantage**: Husky hooks are version controlled and automatically installed when team runs `npm install`.

#### Option 3: Git Config Core.hooksPath

Set a shared hooks directory:

```bash
# In your repository, create a hooks directory
mkdir -p .githooks

# Add your hooks to .githooks/
# (e.g., .githooks/pre-commit)

# Configure git to use this directory
git config core.hooksPath .githooks

# Team members run:
git config core.hooksPath .githooks
```

**Advantage**: Hooks are version controlled.
**Disadvantage**: Each team member must run the config command.

---

## Bypassing Hooks

### When to Bypass

Bypass hooks when:
- Making a quick fix where docs aren't affected
- Documentation update is coming in a separate commit
- Hook is blocking legitimate work
- Emergency situation requiring fast commit

### How to Bypass

**Skip pre-commit and commit-msg hooks**:
```bash
git commit --no-verify -m "emergency fix"
# or
git commit -n -m "emergency fix"
```

**Skip pre-push hook**:
```bash
git push --no-verify
# or
git push -n
```

**Disable hooks temporarily**:
```bash
# Rename hooks to disable
mv .git/hooks/pre-commit .git/hooks/pre-commit.disabled

# Re-enable later
mv .git/hooks/pre-commit.disabled .git/hooks/pre-commit
```

### Team Policy on Bypassing

Establish team guidelines:
- **Acceptable**: Bypassing for documentation-only commits, urgent hotfixes
- **Discouraged**: Routinely bypassing because "hooks are annoying"
- **Never**: Bypassing to skip documentation intentionally

**Document your policy** in the team's contribution guidelines.

---

## Team Setup

### Getting Team Buy-In

**Steps to introduce hooks**:

1. **Discuss with team**:
   - Explain benefits and drawbacks
   - Get consensus on reminder vs. enforcement mode
   - Agree on acceptable bypass scenarios

2. **Start with reminders only**:
   - Don't enforce initially
   - Let team adjust to reminders
   - Gather feedback after 2 weeks

3. **Iterate based on feedback**:
   - Adjust hook messages
   - Change when hooks trigger
   - Consider enforcement only if reminders don't work

4. **Make setup easy**:
   - Provide setup script
   - Add to onboarding documentation
   - Help team members install hooks

5. **Lead by example**:
   - Project leads should use hooks
   - Show how documentation updates become routine
   - Celebrate good documentation practices

### Handling Resistance

**If team members resist hooks**:

1. **Understand concerns**:
   - Too disruptive?
   - Slows down workflow?
   - Messages too naggy?

2. **Address concerns**:
   - Make hooks less intrusive
   - Reduce frequency of messages
   - Simplify checklist

3. **Make hooks optional**:
   - Hooks are recommended, not required
   - Rely on code review instead
   - Focus on building documentation culture

4. **Alternative approaches**:
   - Use reminder script manually: `npm run check-docs`
   - Documentation review in code reviews
   - Weekly documentation sync meetings

**Don't force hooks** if they cause more harm than good. They're a tool, not a requirement.

---

## Troubleshooting

### Hook Not Running

**Problem**: Hook script exists but doesn't execute.

**Solutions**:
```bash
# Verify hook is executable
chmod +x .git/hooks/[hook-name]

# Verify hook is in correct location
ls -la .git/hooks/

# Check for typos in filename (must be exact: "pre-commit", not "pre-commit.sh")
mv .git/hooks/pre-commit.sh .git/hooks/pre-commit

# Test hook manually
bash .git/hooks/pre-commit
```

### Hook Failing Unexpectedly

**Problem**: Hook exits with error and blocks commit.

**Solutions**:
```bash
# Check hook for syntax errors
bash -n .git/hooks/pre-commit

# Run hook with debug output
bash -x .git/hooks/pre-commit

# Check for missing commands
which ts-node
which npm

# Bypass temporarily while debugging
git commit --no-verify
```

### Hooks Not Installed After Clone

**Problem**: Teammate cloned repo but hooks aren't working.

**Solution**:
Git hooks are not cloned automatically. Team members must:
```bash
# Run setup script
bash scripts/setup-hooks.sh

# Or manually copy hooks
cp scripts/pre-commit .git/hooks/
chmod +x .git/hooks/pre-commit
```

**Better solution**: Use Husky or document setup in README.

### Different Behavior on Different Machines

**Problem**: Hook works on one machine but not another.

**Possible causes**:
- Different shell (bash vs. zsh)
- Missing dependencies (ts-node not installed)
- Different PATH settings
- Different git version

**Solutions**:
```bash
# Specify bash explicitly in hook shebang
#!/bin/bash

# Check for dependencies before using
if command -v ts-node &> /dev/null; then
    npx ts-node scripts/check-reminders.ts
else
    echo "ts-node not found, skipping documentation check"
fi

# Use portable commands (avoid GNU-specific flags)
```

### Hook Too Slow

**Problem**: Hook takes too long to run, slowing down commits.

**Solutions**:
- Move expensive checks to pre-push instead of pre-commit
- Cache results when possible
- Skip checks for documentation-only commits
- Optimize script performance

---

## Advanced Patterns

### Conditional Hooks

Run hooks only for certain branches:

```bash
#!/bin/bash

BRANCH=$(git rev-parse --abbrev-ref HEAD)

# Only run hook on main/master branch
if [[ "$BRANCH" == "main" || "$BRANCH" == "master" ]]; then
    # Run documentation checks
    echo "Checking documentation on protected branch..."
fi
```

### Integration with CI/CD

Run documentation checks in CI pipeline:

```yaml
# .github/workflows/check-docs.yml
name: Documentation Check

on: [pull_request]

jobs:
  check-docs:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v2
      - name: Check documentation
        run: |
          if [ -f "scripts/check-reminders.ts" ]; then
            npx ts-node scripts/check-reminders.ts
          fi
```

### Hook Management Tools

Consider using hook management tools:
- **Husky**: For Node.js projects
- **Lefthook**: Fast, parallel hooks for any project
- **pre-commit**: Python-based hook framework

---

## Summary

### Recommended Setup

**For most projects**:
1. Start with **manual reminders** using check-reminders.ts script
2. Add **pre-commit hook in reminder mode** (doesn't block)
3. Add **pre-push hook** to run full checklist
4. **Don't enforce** unless documentation drift is a serious problem

### Key Takeaways

- Git hooks are **optional** but can help build documentation habits
- Start with **reminders, not enforcement**
- Make hooks **easy to bypass** for legitimate reasons
- Provide **setup script** for team adoption
- Get **team buy-in** before introducing hooks
- **Iterate** based on team feedback

### Resources

- [Git Hooks Documentation](https://git-scm.com/book/en/v2/Customizing-Git-Git-Hooks)
- [Husky](https://typicode.github.io/husky/)
- [Lefthook](https://github.com/evilmartians/lefthook)
- [Pre-commit Framework](https://pre-commit.com/)

---

**Last Updated**: [Date]
**Version**: 1.0
