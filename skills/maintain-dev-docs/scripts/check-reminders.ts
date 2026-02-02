#!/usr/bin/env ts-node

/**
 * Documentation Maintenance Reminder Checklist
 *
 * Outputs a comprehensive checklist of critical items to verify when maintaining
 * project documentation. Run this before committing code changes to ensure
 * documentation stays in sync.
 *
 * Usage:
 *   ts-node check-reminders.ts
 *
 * Or add to package.json:
 *   "scripts": {
 *     "check-docs": "ts-node scripts/check-reminders.ts"
 *   }
 *
 * Can be integrated into git hooks or CI/CD pipelines.
 */

interface ReminderCategory {
  category: string;
  items: string[];
}

/**
 * Outputs formatted reminder checklist to console
 */
function outputReminders(): void {
  const reminders: ReminderCategory[] = [
    {
      category: "Architecture Decisions",
      items: [
        "Have you documented the database provider choice in specification.md?",
        "Have you specified LLM providers and models (if applicable)?",
        "Have you configured and documented the logging solution?",
        "Have you set up observability/monitoring and documented it?",
      ]
    },
    {
      category: "Documentation Updates",
      items: [
        "Does specification.md reflect the current architecture?",
        "Are tasks.md checkboxes up to date with actual progress?",
        "Do PHASE#.md files match current implementation?",
        "Is README.md user-facing information current?",
      ]
    },
    {
      category: "Feature Additions",
      items: [
        "Have you updated specification.md with new integrations or tech?",
        "Have you added new tasks to tasks.md?",
        "Have you updated the relevant PHASE#.md file with implementation details?",
        "Have you created feature-specific docs if the feature is complex?",
        "Have you updated README.md if the feature is user-facing?",
      ]
    },
    {
      category: "Phase Transitions",
      items: [
        "Have you marked completed phase tasks as done in tasks.md?",
        "Have you updated tasks.md to reflect phase completion status?",
        "Have you reviewed next phase requirements?",
        "Have you updated specification.md phase overview?",
        "Have you created the next PHASE#.md file if moving to a new phase?",
      ]
    }
  ];

  console.log("\n" + "=".repeat(60));
  console.log(" Development Documentation Maintenance Checklist");
  console.log("=".repeat(60) + "\n");

  reminders.forEach((category, categoryIndex) => {
    console.log(`${categoryIndex + 1}. ${category.category}:`);
    console.log("");

    category.items.forEach((item, itemIndex) => {
      console.log(`   ${String.fromCharCode(97 + itemIndex)}. [ ] ${item}`);
    });

    console.log("");
  });

  console.log("=".repeat(60));
  console.log("\nRun this checklist before committing to ensure documentation");
  console.log("stays synchronized with your codebase.\n");
  console.log("To integrate into git hooks, see references/git-hooks-setup.md");
  console.log("");
}

/**
 * Main execution
 */
function main(): void {
  try {
    outputReminders();
    process.exit(0);
  } catch (error) {
    console.error("Error running reminder checklist:", error);
    process.exit(1);
  }
}

// Run if executed directly
if (require.main === module) {
  main();
}

// Export for potential use as module
export { outputReminders, ReminderCategory };
