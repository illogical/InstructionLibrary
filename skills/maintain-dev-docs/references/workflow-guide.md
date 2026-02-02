# Workflow Guide

This guide provides detailed workflow patterns for maintaining development documentation across different scenarios. Use these workflows to ensure consistent documentation practices throughout your project lifecycle.

## Table of Contents

1. [New Project Workflow](#new-project-workflow)
2. [Existing Project Workflow](#existing-project-workflow)
3. [Feature Addition Workflow](#feature-addition-workflow)
4. [Phase Transition Workflow](#phase-transition-workflow)
5. [Documentation Update Workflow](#documentation-update-workflow)
6. [Documentation Review Workflow](#documentation-review-workflow)
7. [Emergency Documentation Recovery](#emergency-documentation-recovery)

---

## New Project Workflow

Use this workflow when starting a brand new project from scratch.

### Step 1: Initialize Documentation Structure

**Create the `/docs/` folder and basic structure**:

```bash
mkdir -p docs/features
touch docs/brainstorm.md
touch docs/specification.md
touch docs/tasks.md
touch docs/PHASE0.md
touch docs/README.md
```

**Alternative**: Copy templates from this skill's references/templates.md file.

### Step 2: Capture Initial Ideas (brainstorm.md)

**Goal**: Document the initial vision and ideas before structuring them.

**What to include**:
1. Write down the initial ideas and inspiration
2. Define the project purpose (why does it exist?)
3. List 3-5 concrete objectives
4. Identify target users
5. Define success criteria
6. Note any constraints (budget, timeline, technical)
7. List open questions that need answers

**Time investment**: 15-30 minutes

**Tips**:
- Don't worry about structure at this stage
- Capture everything, even half-formed ideas
- Include "nice to have" features
- Note any competing products or inspiration sources
- Be honest about constraints and unknowns

**Example questions to answer**:
- What problem does this solve?
- Who will use this?
- What does success look like?
- What are we NOT building?
- What questions do we need to answer?

### Step 3: Create Technical Specification (specification.md)

**Goal**: Transform brainstorm into structured technical specification.

**Critical Step - Ask About Four Key Decisions**:

Before writing anything else in specification.md, gather these critical architecture decisions:

1. **Database Provider**:
   - Ask: "Which database provider should we use? PostgreSQL, MySQL, MongoDB, SQLite?"
   - Follow up: "Why this choice? What requirements drove this decision?"

2. **LLM Providers** (if applicable):
   - Ask: "Will this project use LLMs for any features?"
   - If yes: "Which provider? OpenAI, Anthropic, local models?"
   - Follow up: "Which specific models? What will they be used for?"

3. **Logging Solution**:
   - Ask: "Which logging solution should we set up? Winston, Pino, built-in, etc.?"
   - Follow up: "What log levels do we need? Any specific format requirements?"

4. **Observability & Monitoring**:
   - Ask: "How should we handle observability? Sentry, Prometheus, CloudWatch?"
   - Follow up: "What metrics should we track? Any alerting requirements?"

**What to include**:
1. **Architecture & Tech Stack** (REQUIRED - with the four items above):
   - Database (provider, rationale, schema strategy, hosting)
   - LLM providers (if applicable)
   - Backend framework and version
   - Frontend framework (if applicable)
   - Deployment platform
   - Logging & observability solutions
   - Authentication strategy (if applicable)

2. **API & Data Models**:
   - Key API endpoints (high-level)
   - Core data structures
   - External integrations

3. **General Requirements**:
   - High-level functional requirements
   - Non-functional requirements (performance, security, scalability, reliability)

4. **Phase Overview**:
   - Brief description of each implementation phase
   - Link to detailed PHASE#.md files

**Time investment**: 1-2 hours

**Tips**:
- Keep this document general, not detailed
- Focus on "what" not "how"
- Document rationale for decisions, not just facts
- Link to external documentation for frameworks/tools
- This becomes your single source of truth for architecture

### Step 4: Build Task List (tasks.md)

**Goal**: Create hierarchical task breakdown organized by phases.

**What to include**:
1. Organize tasks by phases (Phase 0, 1, 2, etc.)
2. Create task hierarchy with clear parent-child relationships
3. Use checkboxes for tracking
4. Estimate Phase 0 tasks first (foundation work)
5. Add placeholders for future phase tasks

**Structure**:
```markdown
### Phase 0: Foundation
- [ ] Setup & Infrastructure
  - [ ] Initialize project structure
  - [ ] Configure database
  - [ ] Setup logging
- [ ] Core Models
  - [ ] Define schemas
  - [ ] Create migrations
```

**Time investment**: 30-45 minutes

**Tips**:
- Start with Phase 0 (foundation) tasks only
- Break tasks down to achievable chunks (4-8 hours max)
- Use hierarchical structure (feature → subtasks)
- Don't over-plan future phases yet
- Tasks become more detailed as you approach them
- Update this file continuously as you work

### Step 5: Detail Foundation Phase (PHASE0.md)

**Goal**: Create detailed implementation specs for Phase 0.

**What to include**:
1. **Phase overview** and objectives
2. **Success criteria** (checkboxes for completion)
3. **Detailed specifications** for each foundation feature:
   - Requirements (functional and non-functional)
   - Database changes (schemas, migrations)
   - API endpoints (with request/response examples)
   - LLM integration details (if applicable)
   - Logging and observability setup
   - Testing requirements
   - Security considerations
4. **Dependencies** and risks
5. **Timeline estimate** (optional)

**Time investment**: 2-3 hours

**Tips**:
- This is where details live (not in specification.md)
- Include code examples for schemas and API endpoints
- Document "why" for architectural decisions
- Be specific about testing requirements
- Note any blockers or dependencies
- Update as you implement (living document)

### Step 6: Create User-Facing Documentation (README.md)

**Goal**: Write documentation that users will see first.

**What to include**:
1. Project description (1-2 compelling sentences)
2. Feature list
3. Prerequisites
4. Installation instructions (step-by-step)
5. Configuration (environment variables, config files)
6. Running the project (dev mode, production build, tests)
7. Basic usage examples
8. Link to detailed documentation (specification.md)

**Time investment**: 30-60 minutes

**Tips**:
- Write for someone who knows nothing about your project
- Test installation instructions on a clean machine
- Include common troubleshooting issues
- Keep it concise but complete
- Update as you add user-facing features
- This is the first impression, make it count

### Step 7: Begin Implementation

**Goal**: Start building with documentation in place.

**Workflow**:
1. Review PHASE0.md before starting each feature
2. Mark tasks as in-progress in tasks.md
3. Implement the feature
4. Update phase file if implementation differs from plan
5. Check off task in tasks.md when complete
6. Update README.md if user-facing
7. Commit code and docs together

**Tips**:
- Refer to specification.md for architecture decisions
- Update documentation as you go, not after
- If you discover new tasks, add them to tasks.md immediately
- If specs change, update PHASE0.md
- Keep feedback loops short (update docs frequently)

---

## Existing Project Workflow

Use this workflow when adding documentation to a project that already has some code or partial documentation.

### Step 1: Assess Current State

**Goal**: Understand what exists and what's missing.

**Actions**:
1. Check if `/docs/` folder exists
2. List existing documentation files
3. Review existing documentation quality and completeness
4. Identify gaps in documentation

**Questions to answer**:
- Does a specification exist? Is it current?
- Is there a task list or roadmap?
- Are phase files or detailed specs documented?
- Is README.md user-friendly and current?
- Are architecture decisions documented?

**Create assessment checklist**:
```markdown
- [ ] specification.md exists and is current
- [ ] Database provider documented
- [ ] LLM providers documented (if applicable)
- [ ] Logging solution documented
- [ ] Observability setup documented
- [ ] tasks.md exists with current progress
- [ ] Phase files exist for implemented features
- [ ] README.md has correct setup instructions
- [ ] API documentation exists
```

### Step 2: Create Missing Core Files

**Goal**: Establish the standard documentation structure.

**Priority order**:
1. **specification.md** (highest priority)
   - Extract architecture info from codebase
   - Ask about missing critical decisions (DB, LLM, logging, observability)
   - Document current state, not ideal state

2. **tasks.md**
   - List features already implemented (marked as complete)
   - List in-progress features
   - List planned features (if known)

3. **PHASE files**
   - Create files for each implemented/in-progress phase
   - Document what was built, even if specs weren't written beforehand
   - Include "as-built" documentation

4. **README.md**
   - Ensure it reflects current functionality
   - Update installation instructions
   - Verify all steps work on a clean machine

5. **brainstorm.md** (optional)
   - May not exist for mature projects
   - Create if still evolving project direction

### Step 3: Fill Critical Gaps

**Goal**: Ensure the four critical architecture decisions are documented.

**Database Provider**:
- Check codebase for database configuration
- Document: Provider, version, ORM/query builder, hosting
- Document rationale if known, or note "Historical decision"

**LLM Providers** (if applicable):
- Check code for LLM API calls
- Document: Provider, models, use cases, cost considerations
- Document: Rate limiting, fallback strategies

**Logging**:
- Check code for logging implementation
- Document: Framework, log levels, where logs are sent
- Document: Log retention, log analysis tools

**Observability**:
- Check for monitoring setup
- Document: Error tracking, metrics, tracing, alerts
- Document: Dashboards, on-call procedures

**If any are missing from codebase, add them to tasks.md as technical debt.**

### Step 4: Align Documentation with Code

**Goal**: Ensure documentation matches current implementation.

**Actions**:
1. **Review specification.md** against codebase:
   - Verify tech stack matches what's actually used
   - Update API endpoint list
   - Verify data models match database schema

2. **Update tasks.md**:
   - Mark implemented features as complete
   - Add date completed if tracking
   - Ensure in-progress items are actually in progress

3. **Review phase files**:
   - Document features that were implemented but not spec'd
   - Update specifications to match "as-built" reality
   - Note any deviations from original plans

4. **Test README.md**:
   - Follow installation instructions exactly
   - Fix any steps that don't work
   - Update for current state of project

### Step 5: Establish Going-Forward Process

**Goal**: Prevent documentation drift in the future.

**Actions**:
1. **Set up reminder script**:
   - Add check-reminders.ts to project
   - Add npm script: `"check-docs": "ts-node scripts/check-reminders.ts"`

2. **Consider git hooks** (optional):
   - See references/git-hooks-setup.md
   - Pre-commit hook to remind about docs

3. **Make docs part of "done" definition**:
   - Feature isn't complete until documentation is updated
   - Include docs in code review checklist

4. **Schedule documentation reviews**:
   - Weekly: Review tasks.md, update checkboxes
   - Bi-weekly: Review specification.md for accuracy
   - Monthly: Review all documentation for completeness

**Tips**:
- Don't try to document everything at once
- Start with critical files (specification.md, tasks.md)
- Document current state first, improve later
- Set up processes to prevent future drift
- Make documentation a team habit, not a one-time effort

---

## Feature Addition Workflow

Use this workflow when adding a new feature to an existing project.

### Before Implementation

**Step 1: Determine Impact** (5 minutes)

Ask these questions:
- Does this feature introduce new technology/services?
- Does this feature change existing APIs?
- Is this feature user-facing?
- Is this feature complex enough for its own documentation file?

**Step 2: Update specification.md** (if needed) (10-15 minutes)

Update specification.md if:
- Adding new external service or API integration
- Introducing new technology or framework
- Changing deployment architecture
- Adding new infrastructure component

**What to update**:
- Add to appropriate section (Architecture & Tech Stack, External Integrations)
- Document rationale for new technology
- Note any cost implications
- Update phase overview if this changes phase goals

**Step 3: Add Tasks to tasks.md** (10 minutes)

**Actions**:
1. Identify which phase this feature belongs to
2. Add parent task for the feature
3. Break down into subtasks:
   - Design/planning
   - Backend implementation
   - Frontend implementation (if applicable)
   - Testing
   - Documentation
4. Use checkboxes for tracking

**Example**:
```markdown
### Phase 1: Core Features
- [ ] User Profile Feature
  - [ ] Design profile data schema
  - [ ] Create profile API endpoints
  - [ ] Build profile UI components
  - [ ] Write unit tests
  - [ ] Write integration tests
  - [ ] Update README with profile usage
```

**Step 4: Update PHASE#.md** (30-60 minutes)

**Add detailed specifications**:
1. Feature overview and requirements
2. Database changes (schemas, migrations, indexes)
3. API endpoints (with full request/response examples)
4. Frontend components (if applicable)
5. LLM integration (if applicable)
6. Logging requirements (what to log at each level)
7. Metrics to track
8. Testing requirements (unit, integration, E2E)
9. Security considerations (validation, auth, rate limiting)

**Tips**:
- Be specific with examples
- Include error cases
- Document edge cases
- Note any assumptions

**Step 5: Create Feature Documentation** (optional) (30-60 minutes)

Create `docs/features/feature-name.md` if:
- Feature is substantial and complex
- Feature needs extensive documentation
- Feature involves multiple systems
- You want dedicated space for feature discussion

Use the feature documentation template from references/templates.md.

### During Implementation

**Step 1: Mark Task In Progress**

Update tasks.md:
```markdown
- [~] User Profile Feature  # Use tilde to show in-progress
  - [x] Design profile data schema  # Check off completed subtasks
  - [~] Create profile API endpoints  # Current work
  - [ ] Build profile UI components
```

**Step 2: Update Docs as You Go**

As you implement:
- If implementation differs from PHASE#.md specs, update the specs
- If you discover edge cases, add them to phase file
- If you add new tasks, update tasks.md
- If you change APIs, update phase file with new examples

**Don't wait until feature is complete to update documentation.**

**Step 3: Add Logging and Metrics**

As you write code:
- Add appropriate log statements (info, warn, error)
- Track metrics defined in phase file
- Add tracing for performance monitoring
- Verify logging matches what's documented

### After Implementation

**Step 1: Complete Tasks** (2 minutes)

Update tasks.md:
```markdown
- [x] User Profile Feature
  - [x] Design profile data schema
  - [x] Create profile API endpoints
  - [x] Build profile UI components
  - [x] Write unit tests
  - [x] Write integration tests
  - [x] Update README with profile usage
```

**Step 2: Update README.md** (if user-facing) (10-15 minutes)

If feature is user-facing:
- Add to features list
- Update usage examples
- Add configuration if needed
- Update installation if dependencies added
- Add troubleshooting for common issues

**Step 3: Verify Documentation Sync**

Run through checklist:
- [ ] specification.md updated if new tech added
- [ ] tasks.md checkboxes updated
- [ ] PHASE#.md reflects actual implementation
- [ ] README.md updated if user-facing
- [ ] Feature docs created if needed

**Step 4: Run Reminder Script** (1 minute)

```bash
ts-node docs/check-reminders.ts
```

Review checklist output and verify all items addressed.

**Step 5: Commit Docs with Code**

```bash
git add .
git commit -m "Add user profile feature

Implements user profile management with CRUD operations.
Includes profile photo upload and bio editing.

Documentation updated:
- Added profile endpoints to PHASE1.md
- Updated tasks.md with completion status
- Updated README.md with profile usage examples"
```

Commit docs and code together to keep them synchronized.

---

## Phase Transition Workflow

Use this workflow when completing one phase and moving to the next.

### Step 1: Review Current Phase Completion

**Goal**: Ensure current phase is truly complete.

**Actions**:
1. Open tasks.md and review current phase section
2. Verify all tasks are checked off
3. Open PHASE#.md for current phase
4. Review success criteria - all items should be checked

**If tasks remain incomplete**:
- Decide: Move to next phase, or complete remaining tasks?
- If moving forward: Document why tasks were skipped
- If completing: Finish tasks before transitioning

### Step 2: Mark Phase Complete in tasks.md

**Update tasks.md**:

**Before**:
```markdown
### Phase 0: Foundation
- [x] Setup & Infrastructure
- [x] Database Setup
- [x] Core Models
```

**After**:
```markdown
### Phase 0: Foundation ✓ (Completed: 2024-01-15)
- [x] Setup & Infrastructure
- [x] Database Setup
- [x] Core Models
```

**Move completed tasks** (optional):
```markdown
## Completed Tasks

### Phase 0: Foundation (Completed: 2024-01-15)
- [x] Setup & Infrastructure
- [x] Database Setup
- [x] Core Models
```

### Step 3: Update specification.md Phase Overview

**Update the phase overview section**:

**Before**:
```markdown
## Phase Overview

### Phase 0: Foundation
Initial project setup, database configuration, core models.
- Key deliverables: Project structure, database, authentication

### Phase 1: Core Features
Main user-facing features.
- Key deliverables: TBD
```

**After**:
```markdown
## Phase Overview

### Phase 0: Foundation ✓ (Completed: 2024-01-15)
Initial project setup, database configuration, core models.
- Key deliverables: Project structure, PostgreSQL database, JWT authentication
- **Status**: Complete

### Phase 1: Core Features (In Progress: Started 2024-01-16)
Main user-facing features including user profiles, content creation, and search.
- Key deliverables: User profiles, content CRUD, search functionality
- **Status**: In Progress
```

### Step 4: Create Next Phase File

**Create PHASE[N+1].md** using template from references/templates.md:

```bash
cp docs/references/templates.md temp.md
# Extract PHASE#.md template section
```

**Or copy from template manually.**

**Fill in**:
1. Phase overview and goals
2. Success criteria for this phase
3. Detailed specs for features in this phase
4. Dependencies on previous phases
5. Risks and mitigation strategies
6. Timeline estimate (optional)

**Time investment**: 1-2 hours for comprehensive phase planning

### Step 5: Plan Next Phase Tasks

**Add Phase [N+1] tasks to tasks.md**:

```markdown
### Phase 1: Core Features
- [ ] User Profiles
  - [ ] Profile data model
  - [ ] Profile API endpoints
  - [ ] Profile UI components
  - [ ] Profile tests
- [ ] Content Creation
  - [ ] Content model
  - [ ] Creation API
  - [ ] Editor component
  - [ ] Content tests
- [ ] Search Functionality
  - [ ] Search indexing
  - [ ] Search API
  - [ ] Search UI
  - [ ] Search tests
```

### Step 6: Review Dependencies

**Verify**:
- All Phase [N] deliverables are in place for Phase [N+1] to proceed
- No blocking issues exist
- External dependencies are resolved
- Team is ready to begin next phase

**If dependencies are missing**:
- Document them in PHASE[N+1].md
- Add tasks to resolve them
- May need to delay phase transition

### Step 7: Communicate Transition

**If working in a team**:
- Announce phase completion
- Share PHASE[N+1].md for review
- Discuss priorities and timeline
- Assign tasks if applicable

**If solo project**:
- Review your own PHASE[N+1].md
- Ensure you understand the scope
- Break down any tasks that are too large

---

## Documentation Update Workflow

Use this workflow for maintaining documentation as the project evolves.

### Daily Updates

**Before starting work**:
1. Read relevant PHASE#.md section for today's work
2. Mark task as in-progress in tasks.md

**During work**:
1. Update phase files if implementation differs from specs
2. Add newly discovered tasks to tasks.md immediately
3. Note any decisions made in phase files

**After completing work**:
1. Check off completed tasks in tasks.md
2. Verify phase file reflects what was built
3. Update README.md if user-facing changes

**Before committing**:
1. Review what documentation needs updating
2. Update relevant files
3. Commit docs with code changes

**Time investment**: 5-10 minutes per day

### Weekly Reviews

**Every week, review**:

1. **tasks.md** (10 minutes):
   - Verify all checkboxes are accurate
   - Add any forgotten tasks
   - Update task descriptions if needed
   - Review next week's priorities

2. **PHASE#.md** (15 minutes):
   - Verify current phase file matches implementation
   - Update any specs that changed
   - Review upcoming features

3. **README.md** (5 minutes):
   - Verify installation instructions work
   - Check that feature list is current
   - Test configuration steps

**Time investment**: 30 minutes per week

### Monthly Reviews

**Every month, comprehensive review**:

1. **specification.md** (30 minutes):
   - Verify architecture section is current
   - Update tech stack if dependencies changed
   - Verify API section lists all endpoints
   - Update requirements if they evolved
   - Check phase overview matches reality

2. **All PHASE#.md files** (45 minutes):
   - Review each phase file for accuracy
   - Update success criteria
   - Add any post-implementation notes
   - Document lessons learned

3. **README.md** (15 minutes):
   - Full test of installation instructions
   - Verify all usage examples work
   - Update troubleshooting section
   - Check links aren't broken

4. **tasks.md** (15 minutes):
   - Clean up completed tasks section
   - Review backlog items
   - Reprioritize if needed
   - Plan next phase if approaching transition

5. **Feature docs** (15 minutes):
   - Review feature-specific documentation
   - Update if features evolved
   - Add troubleshooting sections

**Time investment**: 2 hours per month

### Ad-Hoc Updates

**When to update immediately**:

- **Architecture change**: Update specification.md immediately
- **New external service**: Update specification.md same day
- **API endpoint change**: Update PHASE#.md before committing
- **Breaking change**: Update README.md before release
- **New task discovered**: Add to tasks.md immediately
- **Bug discovered**: Note in relevant phase file

**Don't defer these updates** - they're critical for maintaining documentation accuracy.

---

## Documentation Review Workflow

Use this workflow when reviewing documentation for accuracy and completeness.

### Pre-Commit Review

**Before every commit, verify**:

1. **Code changes match docs**:
   - If you added an API endpoint, is it in PHASE#.md?
   - If you changed behavior, is PHASE#.md updated?
   - If user-facing, is README.md updated?

2. **Tasks are updated**:
   - In-progress tasks marked correctly
   - Completed tasks checked off
   - New tasks added if discovered

3. **Run reminder script**:
   ```bash
   ts-node docs/check-reminders.ts
   ```

**Time investment**: 2-3 minutes

### Code Review Checklist

**When reviewing PRs, check**:

Documentation updates:
- [ ] specification.md updated if new tech/services added
- [ ] tasks.md checkboxes updated
- [ ] PHASE#.md updated with implementation details
- [ ] README.md updated if user-facing changes
- [ ] Feature docs created/updated if needed

Critical decisions documented:
- [ ] Database changes documented
- [ ] API changes documented
- [ ] LLM integration documented (if applicable)
- [ ] Logging added and documented
- [ ] Metrics tracked and documented

**Reject PR if documentation is missing or incorrect.**

### Phase Completion Review

**When completing a phase**:

1. **Review PHASE#.md**:
   - All success criteria checked off
   - All features documented
   - Implementation matches specs (or specs updated)
   - Lessons learned noted

2. **Review tasks.md**:
   - All phase tasks checked off
   - No orphaned tasks
   - Next phase tasks planned

3. **Review specification.md**:
   - Phase overview updated
   - New technology documented
   - Requirements reflect current state

4. **Review README.md**:
   - Feature list current
   - Installation works
   - Usage examples accurate

**Time investment**: 30-45 minutes

---

## Emergency Documentation Recovery

Use this workflow when documentation has fallen seriously behind code.

### Step 1: Assess the Gap

**Determine**:
- How far behind is documentation?
- What's missing vs. what's outdated?
- What's the business impact?
- How much time can we invest in recovery?

### Step 2: Prioritize Recovery

**Priority 1 - Critical** (do first):
- specification.md: Architecture & Tech Stack section
- Database provider, LLM providers, logging, observability
- README.md: Installation and getting started

**Priority 2 - Important** (do second):
- tasks.md: Current state and next priorities
- Current PHASE#.md: What we're building now
- README.md: Feature list and usage

**Priority 3 - Nice to have** (do when time permits):
- Historical PHASE#.md files: As-built documentation
- Feature documentation
- Completed tasks history

### Step 3: Document Current State

**Focus on "as-is" not "as-planned"**:

1. Read the codebase
2. Extract actual architecture decisions
3. Document what exists, not what was intended
4. Note gaps between docs and reality

**Don't try to recreate history** - document the present.

### Step 4: Establish Prevention

**Implement processes**:
1. Add pre-commit reminders
2. Make docs part of PR reviews
3. Schedule weekly documentation updates
4. Add documentation to definition of "done"

**Root cause analysis**:
- Why did docs fall behind?
- What process failed?
- How do we prevent it recurring?

### Step 5: Gradual Improvement

**Don't try to fix everything at once**:

Week 1: Critical items (specification, README, current phase)
Week 2: Important items (tasks, previous phases)
Week 3: Nice to have items (feature docs, history)

**Continuous improvement**:
- Update docs with every PR going forward
- Dedicate 30 minutes weekly to documentation debt
- Review monthly for accuracy

**Time investment**: 4-8 hours for recovery + ongoing maintenance

---

## General Tips

### Keep Feedback Loops Short

- Update docs in small increments, not large batches
- Update immediately after making decisions
- Commit docs with related code changes
- Review docs weekly, not monthly

### Make Documentation a Habit

- Treat docs as part of the feature, not separate work
- Include documentation time in estimates
- Review documentation in code reviews
- Celebrate good documentation

### Use Templates Consistently

- Always use templates from references/templates.md
- Adapt templates to project needs
- Don't skip sections, mark as "N/A" if not applicable
- Maintain consistent format across all docs

### Tool Integration

- Add reminder script to project
- Consider git hooks for automation
- Use linting for markdown quality
- Automate what you can, but human review is essential

---

**Last Updated**: [Date]
**Version**: 1.0
