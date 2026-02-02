# Phase Management Guide

This guide explains how to structure phase files, when to create new phases, how to manage phase transitions, and best practices for phase-based development documentation.

## Table of Contents

1. [Understanding Phases](#understanding-phases)
2. [When to Create New Phases](#when-to-create-new-phases)
3. [Phase File Structure](#phase-file-structure)
4. [Managing Phase Transitions](#managing-phase-transitions)
5. [Phase Dependencies](#phase-dependencies)
6. [Phase Completion Criteria](#phase-completion-criteria)
7. [Common Phase Patterns](#common-phase-patterns)
8. [Examples of Well-Structured Phases](#examples-of-well-structured-phases)

---

## Understanding Phases

### What is a Phase?

A **phase** is a logical grouping of related features and tasks that share common goals and dependencies. Each phase represents a milestone in the project's development.

**Key characteristics**:
- Has clear objectives and deliverables
- Contains detailed implementation specifications
- Includes success criteria for completion
- May have dependencies on previous phases
- Typically takes 2-8 weeks to complete (varies by project size)

### Why Use Phases?

**Benefits**:
1. **Breaks large projects into manageable chunks**
2. **Provides clear progress milestones**
3. **Helps prioritize work** (what's essential vs. nice-to-have)
4. **Enables parallel work** (team can work on different phases)
5. **Allows for iterative development** (deliver value incrementally)
6. **Makes planning more accurate** (easier to estimate smaller chunks)

### Phases vs. Specification

**specification.md**:
- High-level architecture and general requirements
- Stays general and stable
- "What" we're building at a high level

**PHASE#.md files**:
- Detailed implementation specifications
- Specific and actionable
- "How" we're building each feature in detail
- Changes as implementation progresses

**Relationship**: specification.md provides the blueprint; PHASE#.md files provide the construction plans.

---

## When to Create New Phases

### Phase 0: Always Exists

**Phase 0** is the foundation phase and should always exist for any project.

**Typical Phase 0 contents**:
- Project structure initialization
- Database setup and core schema
- Authentication/authorization system
- Logging and observability infrastructure
- Core data models
- API foundation
- Testing infrastructure
- Deployment pipeline basics

**Phase 0 goals**: Establish the foundation that all future work builds upon.

### Subsequent Phases: Based on Logical Groupings

Create a new phase when:

1. **Natural Feature Grouping Exists**:
   - Features belong together logically
   - Features share similar complexity
   - Features can be developed relatively independently

2. **Dependency Chain Changes**:
   - Phase N+1 requires Phase N to be complete
   - New architectural layer is needed
   - Integration point is reached

3. **Milestone is Reached**:
   - MVP is complete (Phase 0 + Phase 1)
   - Core features are done (Phase 1)
   - Advanced features begin (Phase 2)
   - Optimization phase starts (Phase 3)

4. **Team or Timeline Shifts**:
   - Different team members work on different phases
   - Budget milestone reached
   - Release point approached

### How Many Phases?

**Typical project phase counts**:

- **Small projects** (1-3 months): 2-3 phases
  - Phase 0: Foundation
  - Phase 1: Core Features
  - Phase 2: Polish & Launch

- **Medium projects** (3-6 months): 3-4 phases
  - Phase 0: Foundation
  - Phase 1: Core Features
  - Phase 2: Advanced Features
  - Phase 3: Optimization & Launch

- **Large projects** (6-12+ months): 4-6+ phases
  - Phase 0: Foundation
  - Phase 1: Core Features Set A
  - Phase 2: Core Features Set B
  - Phase 3: Advanced Features
  - Phase 4: Integrations & Extensions
  - Phase 5: Performance & Scale
  - Phase 6: Polish & Launch

**Don't over-plan**: Create Phase 1 and Phase 2 initially. Create Phase 3+ as you approach Phase 2 completion.

### Signs You Need a New Phase

Create a new phase if:
- Current phase file exceeds 1000 lines (too large)
- Features in current phase have very different complexities
- New features require previous features to be complete
- You're starting a new major category of features (e.g., admin tools after user features)
- Team is ready to work on next milestone

### Signs You Don't Need a New Phase

Don't create a new phase if:
- Current phase isn't complete yet
- New features fit logically in current phase
- Features don't have clear dependencies on current work
- Creating a phase with only 1-2 features

---

## Phase File Structure

### Standard PHASE#.md Structure

Every phase file should follow this structure:

```markdown
# Phase [N]: [Phase Name]

## Phase Overview
[Detailed description, key objectives]

## Success Criteria
- [ ] Criterion 1
- [ ] Criterion 2

## Detailed Implementation Specifications

### Feature 1: [Name]
#### Overview
#### Requirements
#### Database Changes
#### API Endpoints
#### Frontend Components (if applicable)
#### LLM Integration (if applicable)
#### Logging & Observability
#### Testing Requirements
#### Security Considerations

### Feature 2: [Name]
[Same structure]

## Dependencies
## Risks & Mitigation
## Timeline Estimate
## Open Questions
## Notes
```

### Level of Detail

**Phase files should be detailed enough that**:
- A developer can implement the feature without ambiguity
- Code reviewers can verify the implementation matches specs
- AI assistants can understand the full context
- Future maintainers can understand decisions made

**Include**:
- Concrete examples (request/response bodies, SQL schemas)
- Edge cases and error scenarios
- Rationale for decisions
- Security considerations
- Performance requirements
- Testing requirements

**Don't include**:
- Actual implementation code (belongs in codebase)
- Step-by-step implementation instructions (trust developers)
- Excessive detail that belongs in code comments

### Updating Phase Files

**When to update**:
- **Before implementation**: Write initial specs
- **During implementation**: Update if specs change or edge cases discovered
- **After implementation**: Add "as-built" notes if implementation differs

**How to mark changes**:

**Option 1**: Inline notes
```markdown
#### API Endpoint
POST /api/users

**Note**: Originally planned for PUT /api/user but changed to POST /api/users
for REST consistency. (Updated: 2024-01-15)
```

**Option 2**: Implementation notes section at end
```markdown
## Implementation Notes

- Changed user creation from PUT to POST for REST compliance
- Added rate limiting after security review
- Optimized query performance with additional index
```

**Keep phase files living documents** that evolve with the project.

---

## Managing Phase Transitions

### Pre-Transition Checklist

Before moving to Phase N+1, verify Phase N is complete:

**Technical Completion**:
- [ ] All features implemented and tested
- [ ] All tests passing
- [ ] Code reviewed and merged
- [ ] Deployed to staging/production
- [ ] No critical bugs outstanding

**Documentation Completion**:
- [ ] PHASE#.md success criteria all checked off
- [ ] tasks.md phase tasks all checked off
- [ ] README.md updated with new features
- [ ] API documentation current
- [ ] User documentation updated (if applicable)

**Business Completion**:
- [ ] Stakeholders have reviewed deliverables
- [ ] User acceptance testing complete (if applicable)
- [ ] Product owner has signed off
- [ ] Metrics show success (if applicable)

### Transition Process

**Step 1**: Mark Phase Complete
- Update tasks.md with completion date
- Check off all success criteria in PHASE#.md
- Update specification.md phase overview

**Step 2**: Retrospective (optional but recommended)
- What went well?
- What could be improved?
- What did we learn?
- What should we do differently in next phase?

**Step 3**: Create Next Phase File
- Use template from references/templates.md
- Define clear objectives
- Set success criteria
- Document detailed specifications

**Step 4**: Plan Next Phase Tasks
- Add tasks to tasks.md
- Break down into subtasks
- Identify dependencies
- Estimate complexity (optional)

**Step 5**: Communicate
- Announce phase completion (team/stakeholders)
- Share next phase plan
- Align on priorities and timeline

### Handling Incomplete Phases

**What if Phase N isn't complete but we need to move on?**

**Option 1: Extend Phase N**
- Revise timeline
- Remove lower-priority features
- Move some features to later phase

**Option 2: Split Phase N**
- Create Phase N.1 and Phase N.2
- Mark N.1 complete, continue with N.2
- Update phase dependencies

**Option 3: Move Features to Backlog**
- Document why features were descoped
- Add to future phase or backlog
- Don't leave phase perpetually "incomplete"

**Document the decision** in specification.md and affected phase files.

### Overlapping Phases

**Can you work on Phase N+1 before Phase N is complete?**

**Yes, if**:
- Phase N+1 features don't depend on Phase N features
- Team has capacity for parallel work
- Clear separation of concerns exists

**No, if**:
- Phase N+1 requires Phase N deliverables
- Working ahead will cause rework
- Team bandwidth is limited

**Best practice**: Complete phases sequentially when possible. Overlapping adds complexity.

---

## Phase Dependencies

### Types of Dependencies

**1. Technical Dependencies**:
- Phase 1 requires database schema from Phase 0
- Phase 2 requires API endpoints from Phase 1
- Phase 3 requires authentication from Phase 0

**2. Business Dependencies**:
- Phase 2 can't start until Phase 1 is user-tested
- Phase 3 requires Phase 1 metrics data
- Launch phase requires all core features complete

**3. External Dependencies**:
- Phase 2 blocked by third-party API access
- Phase 3 requires vendor integration
- Phase 4 depends on design assets

### Documenting Dependencies

In each PHASE#.md file:

```markdown
## Dependencies

### Technical Dependencies

**Previous Phases**:
- **Phase 0** must be complete because:
  - Requires database schema for users and content
  - Requires authentication system for protected endpoints
  - Requires logging infrastructure for monitoring

**External Services**:
- **Stripe API**: Required for payment processing
  - Need API keys (obtained)
  - Need webhook configuration (pending)
- **SendGrid**: Required for email notifications
  - Need API key (obtained)
  - Need email templates (in progress)

**Libraries/Packages**:
- `stripe` npm package: v12.x or higher
- `@sendgrid/mail` npm package: v7.x or higher

### Blocking Issues

- [ ] Awaiting Stripe webhook endpoint approval
- [ ] Awaiting SendGrid sender verification
- [ ] Need designer input on email templates
```

### Managing Blocked Phases

**If Phase N+1 is blocked**:

1. **Document the blocker** in PHASE[N+1].md
2. **Add unblocking task** to tasks.md
3. **Work on non-blocked features** in current phase
4. **Communicate status** to stakeholders
5. **Escalate if critical** to unblock the issue

**Don't let blockers be invisible** - make them explicit in documentation.

---

## Phase Completion Criteria

### What Makes a Phase "Complete"?

A phase is complete when:
1. **All features implemented** and working
2. **All tests passing** (unit, integration, E2E)
3. **All success criteria checked off** in PHASE#.md
4. **All tasks completed** in tasks.md
5. **Documentation updated** (README, API docs, etc.)
6. **Code reviewed** and merged to main branch
7. **Deployed** to appropriate environment (staging/production)

### Defining Success Criteria

**Good success criteria are**:
- **Specific**: "User registration API endpoint implemented and tested"
- **Measurable**: "80% test coverage on authentication module"
- **Achievable**: Realistic given timeline and resources
- **Relevant**: Directly related to phase goals
- **Time-bound**: Expected within phase timeline (if applicable)

**Example success criteria for Phase 0**:

```markdown
## Success Criteria

- [ ] Project structure initialized with all folders and configuration files
- [ ] PostgreSQL database configured with connection pooling
- [ ] Initial database schema migrated (users, sessions tables)
- [ ] JWT authentication implemented with access/refresh tokens
- [ ] Winston logger configured with info/error levels and file rotation
- [ ] Sentry error tracking integrated and tested
- [ ] Basic API structure with /health endpoint responding
- [ ] Testing framework configured with sample unit and integration tests
- [ ] CI/CD pipeline deploying to staging environment
- [ ] 80%+ test coverage on core authentication module
- [ ] README.md has working installation instructions
- [ ] All Phase 0 tasks in tasks.md checked off
```

**Each criterion should be checkable** - you should be able to definitively say "yes, this is done."

### Partial Completion

**What if 90% of phase is complete but 10% is delayed?**

**Option 1: Mark phase complete, move remainder to next phase**
- Document what's remaining
- Add remaining items to next phase
- Update success criteria to reflect what was completed

**Option 2: Extend phase until complete**
- Revise timeline
- Focus efforts on completion
- Delay next phase start

**Choose based on**:
- Is remaining 10% blocking next phase?
- Is remaining 10% lower priority?
- Is team ready to move on?

**Document the decision** in phase files and specification.md.

---

## Common Phase Patterns

### Pattern 1: Foundation → Core → Advanced

**Phase 0: Foundation**
- Infrastructure and architecture
- Database and data models
- Authentication and authorization
- Logging and monitoring
- Core API structure

**Phase 1: Core Features**
- Essential user-facing features
- Primary user workflows
- MVP functionality

**Phase 2: Advanced Features**
- Nice-to-have features
- Enhanced workflows
- Additional integrations

**Best for**: Most projects, especially MVPs

---

### Pattern 2: Horizontal Layers

**Phase 0: Data Layer**
- Database schema
- Data access layer
- Core models and migrations

**Phase 1: API Layer**
- API endpoints
- Business logic
- Validation and error handling

**Phase 2: UI Layer**
- Frontend components
- User interface
- Client-side logic

**Best for**: Projects with clear architectural layers, larger teams

---

### Pattern 3: Vertical Features

**Phase 0: Foundation**
- Infrastructure setup

**Phase 1: User Management (Full Stack)**
- User registration, login, profile (backend + frontend)

**Phase 2: Content Management (Full Stack)**
- Content creation, editing, viewing (backend + frontend)

**Phase 3: Social Features (Full Stack)**
- Comments, likes, sharing (backend + frontend)

**Best for**: Smaller teams, feature-driven development, demonstrable progress

---

### Pattern 4: MVP → Enhancement

**Phase 0: Foundation**
- Core infrastructure

**Phase 1: MVP**
- Minimum viable product
- Just enough to launch

**Phase 2: User Feedback & Iteration**
- Fixes based on user feedback
- Quick improvements

**Phase 3: Scale & Performance**
- Optimization
- Scaling infrastructure

**Phase 4: Advanced Features**
- Features based on usage data

**Best for**: Startups, new products, uncertain requirements

---

### Pattern 5: Integration-Heavy

**Phase 0: Foundation**
- Core infrastructure

**Phase 1: Core Platform**
- Platform features without integrations

**Phase 2: Integration A**
- First external service integration

**Phase 3: Integration B**
- Second external service integration

**Phase 4: Integration C**
- Third external service integration

**Best for**: Integration platforms, API aggregators, multi-vendor systems

---

## Examples of Well-Structured Phases

### Example 1: E-Commerce Platform

#### Phase 0: Foundation
**Goal**: Establish core infrastructure for e-commerce platform

**Key Features**:
- PostgreSQL database with product, user, order tables
- JWT authentication with role-based access (customer, admin)
- Stripe payment integration (basic)
- Winston logging with Sentry error tracking
- Basic REST API structure
- React frontend foundation with Vite
- Tailwind CSS styling setup

**Success Criteria**:
- [ ] Database schema supports products, users, orders, payments
- [ ] Users can register, login, and access protected routes
- [ ] Stripe test payments complete successfully
- [ ] All errors tracked in Sentry with proper context
- [ ] API health check endpoint responding
- [ ] Frontend renders hello world with Tailwind
- [ ] 80% test coverage on authentication

**Timeline**: 2 weeks

---

#### Phase 1: Core Shopping Experience
**Goal**: Enable users to browse and purchase products

**Key Features**:
- Product listing and search
- Product detail pages
- Shopping cart (session-based)
- Checkout flow
- Order confirmation
- Basic email notifications (order confirmation)

**Success Criteria**:
- [ ] Users can browse paginated product listings
- [ ] Search returns relevant results within 500ms
- [ ] Cart persists across sessions
- [ ] Checkout completes payment successfully
- [ ] Order confirmation emails sent reliably
- [ ] 75% test coverage on shopping flow
- [ ] All user-facing features work on mobile

**Dependencies**:
- Phase 0 database schema
- Phase 0 authentication
- Phase 0 Stripe integration

**Timeline**: 3 weeks

---

#### Phase 2: Admin Panel
**Goal**: Enable administrators to manage products and orders

**Key Features**:
- Admin authentication and role checking
- Product management (CRUD operations)
- Order management (view, update status)
- Basic analytics dashboard
- Customer management

**Success Criteria**:
- [ ] Only admin users can access admin panel
- [ ] Admins can create/edit/delete products
- [ ] Admins can view and update order statuses
- [ ] Dashboard shows key metrics (sales, orders, users)
- [ ] 70% test coverage on admin features

**Dependencies**:
- Phase 0 role-based authentication
- Phase 1 order system

**Timeline**: 2 weeks

---

### Example 2: SaaS Analytics Platform

#### Phase 0: Foundation
**Goal**: Core infrastructure and authentication

**Key Features**:
- PostgreSQL for relational data, Redis for caching
- Multi-tenant architecture (organization model)
- OAuth authentication (Google, GitHub)
- Workspace and project data models
- Real-time event streaming setup (Kafka)
- API rate limiting and quota management
- Logging (Pino) and observability (DataDog)

**Success Criteria**:
- [ ] Multi-tenant data isolation working correctly
- [ ] Users can authenticate via OAuth providers
- [ ] Redis caching reduces database load by 50%
- [ ] Kafka consumers processing events reliably
- [ ] Rate limiting enforced per organization
- [ ] DataDog dashboards showing key metrics
- [ ] 85% test coverage on core models

**Timeline**: 3 weeks

---

#### Phase 1: Data Ingestion
**Goal**: Allow users to send analytics events

**Key Features**:
- Event ingestion API (REST and SDK)
- Event validation and schema enforcement
- Real-time event processing pipeline
- Event storage (time-series database)
- Data retention policies
- SDK for JavaScript and Python

**Success Criteria**:
- [ ] API accepts 10,000 events/second
- [ ] Invalid events rejected with clear errors
- [ ] Events processed within 5 seconds
- [ ] SDKs published to npm and PyPI
- [ ] Retention policies automatically archive old data
- [ ] 80% test coverage on ingestion pipeline

**Dependencies**:
- Phase 0 authentication
- Phase 0 Kafka setup
- Phase 0 multi-tenant architecture

**Timeline**: 4 weeks

---

#### Phase 2: Analytics Dashboard
**Goal**: Visualize analytics data

**Key Features**:
- Real-time dashboard with charts
- Custom query builder
- Saved queries and reports
- Dashboard sharing and permissions
- Export to CSV/PDF

**Success Criteria**:
- [ ] Dashboards load in under 2 seconds
- [ ] Users can create custom queries without SQL
- [ ] Saved queries persist and can be shared
- [ ] Exports complete within 30 seconds for 1M rows
- [ ] 75% test coverage on dashboard logic

**Dependencies**:
- Phase 1 event storage
- Phase 0 workspace model

**Timeline**: 4 weeks

---

## Best Practices Summary

### Do's

**Do**:
- Create clear, specific success criteria for each phase
- Document dependencies between phases
- Keep Phase 0 focused on foundation (don't add features)
- Update phase files as implementation progresses
- Break large phases into smaller ones if they exceed 6-8 weeks
- Document rationale for decisions in phase files
- Include security and testing requirements in every phase
- Mark phases complete when success criteria are met

### Don'ts

**Don't**:
- Create phases with only 1-2 features (too granular)
- Leave phases perpetually "in progress" (mark complete or extend)
- Skip Phase 0 (foundation is critical)
- Create all phases upfront (plan 1-2 ahead, create more as needed)
- Let phase files become outdated (update during implementation)
- Move to next phase when current phase has blocking issues
- Forget to update specification.md when phase completes

### Key Principles

1. **Phases are flexible** - Adapt them to your project, team, and timeline
2. **Detail matters** - Phase files should have enough detail to implement confidently
3. **Living documents** - Update phase files as you learn and implement
4. **Clear completion** - Everyone should agree when a phase is done
5. **Dependencies first** - Always document what's needed from previous phases
6. **Document decisions** - Future you will thank present you

---

**Last Updated**: [Date]
**Version**: 1.0
