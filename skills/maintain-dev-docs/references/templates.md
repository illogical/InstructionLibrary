# Documentation Templates

This file contains all templates for the maintain-dev-docs system. Use these templates when creating new documentation files in a project's `/docs/` folder.

## Table of Contents

1. [brainstorm.md Template](#1-brainstormmd-template)
2. [specification.md Template](#2-specificationmd-template)
3. [tasks.md Template](#3-tasksmd-template)
4. [PHASE#.md Template](#4-phasemd-template)
5. [README.md Template](#5-readmemd-template)
6. [Feature Documentation Template](#6-feature-documentation-template)

---

## 1. brainstorm.md Template

Use this template when starting a new project to capture initial ideas and evolving requirements.

```markdown
# Project Brainstorm: [Project Name]

## Initial Ideas

[Free-form ideas and inspiration. What sparked this project? What problem does it solve? What makes it interesting?]

## Purpose

[Why does this project exist? What need does it fulfill? What gap does it fill in the market or for users?]

## Objectives

[What do we want to achieve with this project? List 3-5 concrete objectives.]

- Objective 1: [Description]
- Objective 2: [Description]
- Objective 3: [Description]
- Objective 4: [Description]
- Objective 5: [Description]

## Target Users

[Who will use this project? Describe the target audience, user personas, or use cases.]

### Primary Users
[Description of primary user group]

### Secondary Users
[Description of secondary user groups, if applicable]

## Success Criteria

[How will we know we succeeded? What metrics or outcomes define success?]

- Success criterion 1: [Description]
- Success criterion 2: [Description]
- Success criterion 3: [Description]

## Constraints & Considerations

[Known limitations, technical constraints, budget considerations, timeline constraints, team size, etc.]

### Technical Constraints
- [Constraint 1]
- [Constraint 2]

### Business Constraints
- [Constraint 1]
- [Constraint 2]

### Other Considerations
- [Consideration 1]
- [Consideration 2]

## Open Questions

[Things we need to figure out. Questions to answer before moving forward.]

1. [Question 1]
2. [Question 2]
3. [Question 3]

## Related Projects / Inspiration

[Similar projects, competitors, or sources of inspiration]

- [Project 1]: [Why it's relevant]
- [Project 2]: [Why it's relevant]

## Next Steps

[What needs to happen to move from brainstorm to specification?]

1. [Step 1]
2. [Step 2]
3. [Step 3]

---

**Last Updated**: [Date]
**Status**: [Draft/In Progress/Finalized]
```

---

## 2. specification.md Template

Use this template to document architecture, tech stack, API design, and general requirements. This is the core technical specification document.

```markdown
# Technical Specification: [Project Name]

## Overview

[Brief project description. What does this project do? What problem does it solve? 2-3 sentences maximum.]

## Architecture & Tech Stack

### Database

- **Provider**: [PostgreSQL / MySQL / MongoDB / SQLite / etc.]
- **Rationale**: [Why this choice? What requirements drove this decision?]
- **Schema Strategy**: [Migrations approach, ORM or raw SQL, schema versioning]
- **Hosting**: [Where will the database be hosted? Local, cloud provider, managed service?]

### LLM Providers

[Remove this section if project doesn't use LLMs]

- **Primary Provider**: [OpenAI / Anthropic / Local (Ollama) / Hugging Face / etc.]
- **Model(s)**: [Specific models: gpt-4, claude-3-opus, llama-2, etc.]
- **Use Cases**: [What LLM features are used for? Text generation, embeddings, classification, etc.]
- **Fallback Strategy**: [If primary fails, what's the backup? Rate limiting strategy?]
- **Cost Considerations**: [Token usage estimates, budget constraints]

### Backend Framework

- **Framework**: [Express.js / FastAPI / Django / ASP.NET Core / Spring Boot / etc.]
- **Language/Runtime**: [Node.js / Python / C# / Java / Go / etc.]
- **Version**: [Specific version numbers]
- **Key Libraries**: [Important dependencies]

### Frontend Framework

[Remove if backend-only project]

- **Framework**: [React / Vue / Angular / Svelte / etc.]
- **Build Tool**: [Vite / Webpack / Parcel / esbuild / etc.]
- **Language**: [TypeScript / JavaScript]
- **Styling**: [Tailwind / CSS Modules / styled-components / etc.]
- **State Management**: [Redux / Zustand / Context API / Pinia / etc.]

### Deployment

- **Platform**: [AWS / Azure / GCP / Vercel / Netlify / Docker / Railway / etc.]
- **Environment Strategy**: [Development, Staging, Production setup]
- **CI/CD**: [GitHub Actions / GitLab CI / Jenkins / CircleCI / etc.]
- **Infrastructure as Code**: [Terraform / CloudFormation / Pulumi / None]

### Logging & Observability

- **Logging Solution**: [Winston / Pino / Serilog / Python logging / etc.]
- **Log Level Strategy**: [How logs are categorized: debug, info, warn, error]
- **Metrics**: [Prometheus / CloudWatch / Application Insights / etc.]
- **Tracing**: [OpenTelemetry / Jaeger / Zipkin / etc.]
- **Error Tracking**: [Sentry / Rollbar / Bugsnag / etc.]
- **Performance Monitoring**: [DataDog / New Relic / Grafana / etc.]

### Authentication & Authorization

[If applicable]

- **Strategy**: [JWT / Session-based / OAuth / etc.]
- **Provider**: [Auth0 / Firebase Auth / Clerk / Custom / etc.]
- **User Management**: [How users are stored and managed]

### Additional Infrastructure

[Other important infrastructure components]

- **Caching**: [Redis / Memcached / In-memory / etc.]
- **Message Queue**: [RabbitMQ / AWS SQS / Redis / etc.]
- **File Storage**: [S3 / Azure Blob / Local filesystem / etc.]
- **CDN**: [CloudFlare / AWS CloudFront / etc.]

## API & Data Models

### API Endpoints

[List key endpoints and their purposes. Detailed specs go in PHASE#.md files.]

#### Authentication
- `POST /api/auth/register` - User registration
- `POST /api/auth/login` - User login
- `POST /api/auth/logout` - User logout

#### Core Resources
- `GET /api/[resource]` - List resources
- `POST /api/[resource]` - Create resource
- `GET /api/[resource]/:id` - Get specific resource
- `PUT /api/[resource]/:id` - Update resource
- `DELETE /api/[resource]/:id` - Delete resource

[Add additional endpoint categories as needed]

### Data Schemas

[High-level description of core data structures. Detailed schemas go in PHASE#.md files.]

#### User
- id: UUID
- email: string (unique)
- password_hash: string
- created_at: timestamp
- updated_at: timestamp

#### [Additional Models]
[Brief description of other core data models]

### External Integrations

[Third-party APIs, services, webhooks]

- **[Integration Name]**: [Purpose, endpoints used, authentication method]
- **[Integration Name]**: [Purpose, endpoints used, authentication method]

## General Requirements

### Functional Requirements

[High-level functional requirements. Detailed requirements go in PHASE#.md files.]

1. **[Requirement Category]**
   - [Requirement 1]
   - [Requirement 2]
   - [Requirement 3]

2. **[Requirement Category]**
   - [Requirement 1]
   - [Requirement 2]

### Non-Functional Requirements

#### Performance
- **Response Time**: [Target response times for API endpoints]
- **Throughput**: [Expected requests per second]
- **Concurrent Users**: [How many simultaneous users should the system support?]

#### Security
- **Authentication**: [How users are authenticated]
- **Authorization**: [How permissions are managed]
- **Data Protection**: [Encryption at rest/in transit, PII handling]
- **Rate Limiting**: [API rate limits, DDoS protection]
- **Input Validation**: [How user input is validated and sanitized]

#### Scalability
- **Expected Load**: [Current and projected user/data volumes]
- **Scaling Strategy**: [Horizontal/vertical scaling approach]
- **Bottlenecks**: [Known or anticipated performance bottlenecks]

#### Reliability
- **Uptime Requirements**: [Target uptime percentage]
- **Error Handling**: [How errors are handled and communicated]
- **Data Backup**: [Backup strategy and frequency]
- **Disaster Recovery**: [Recovery time objective (RTO) and recovery point objective (RPO)]

#### Maintainability
- **Code Standards**: [Coding conventions, linting, formatting]
- **Documentation**: [API docs, code comments, architecture diagrams]
- **Testing**: [Testing strategy overview - details in PHASE#.md files]

## Phase Overview

[Brief description of implementation phases. Detailed specs in PHASE#.md files.]

### Phase 0: Foundation
[1-2 sentence description of foundation phase goals]
- Key deliverables: [List 3-5 main items]

### Phase 1: Core Features
[1-2 sentence description of core features phase]
- Key deliverables: [List 3-5 main items]

### Phase 2: Advanced Features
[1-2 sentence description of advanced features phase]
- Key deliverables: [List 3-5 main items]

[Add additional phases as needed]

## Assumptions & Dependencies

### Assumptions
- [Assumption 1]
- [Assumption 2]
- [Assumption 3]

### Dependencies
- [External dependency 1]
- [External dependency 2]
- [External dependency 3]

## Risks & Mitigations

| Risk | Impact | Probability | Mitigation |
|------|--------|-------------|------------|
| [Risk description] | [High/Medium/Low] | [High/Medium/Low] | [Mitigation strategy] |
| [Risk description] | [High/Medium/Low] | [High/Medium/Low] | [Mitigation strategy] |

## Future Considerations

[Features or improvements for future phases beyond initial launch]

- [Consideration 1]
- [Consideration 2]
- [Consideration 3]

---

**Last Updated**: [Date]
**Version**: [1.0]
**Status**: [Draft / In Review / Approved / Living Document]
```

---

## 3. tasks.md Template

Use this template to track all project tasks in a hierarchical structure organized by phases.

```markdown
# Project Tasks: [Project Name]

## Overview

This document tracks all tasks for the project, organized by implementation phases. Check off tasks as they're completed to maintain accurate progress tracking.

**Legend**:
- [ ] Not started
- [x] Completed
- [~] In progress (mark with tilde temporarily, then check when done)

---

## Task Hierarchy

### Phase 0: Foundation

**Goal**: [1 sentence describing Phase 0 goals]

- [ ] **Setup & Infrastructure**
  - [ ] Initialize project structure
  - [ ] Setup version control (Git)
  - [ ] Configure development environment
  - [ ] Setup CI/CD pipeline
  - [ ] Configure linting and formatting
  - [ ] Setup environment variables and secrets management

- [ ] **Database Setup**
  - [ ] Choose and install database provider
  - [ ] Design initial database schema
  - [ ] Setup database migrations
  - [ ] Configure ORM or database access layer
  - [ ] Setup database connection pooling
  - [ ] Create seed data for development

- [ ] **Logging & Observability**
  - [ ] Setup logging framework
  - [ ] Configure log levels and formatting
  - [ ] Setup error tracking (Sentry, Rollbar, etc.)
  - [ ] Configure metrics collection
  - [ ] Setup monitoring dashboards
  - [ ] Configure alerts for critical errors

- [ ] **Core Models & Data Layer**
  - [ ] Define core data schemas
  - [ ] Create database migrations
  - [ ] Implement data access layer
  - [ ] Write model validation
  - [ ] Add database indexes

- [ ] **Authentication & Authorization** [if applicable]
  - [ ] Design auth strategy (JWT, sessions, etc.)
  - [ ] Implement user registration
  - [ ] Implement user login/logout
  - [ ] Implement password hashing
  - [ ] Setup token generation and validation
  - [ ] Implement authorization middleware
  - [ ] Add password reset functionality

- [ ] **Testing Infrastructure**
  - [ ] Setup testing framework
  - [ ] Configure test database
  - [ ] Setup test coverage reporting
  - [ ] Write example tests (unit, integration)
  - [ ] Configure E2E testing (if applicable)

- [ ] **Documentation**
  - [ ] Complete specification.md
  - [ ] Write initial README.md
  - [ ] Document API endpoints (basic)
  - [ ] Setup API documentation tool (if applicable)

---

### Phase 1: Core Features

**Goal**: [1 sentence describing Phase 1 goals]

- [ ] **Feature Group 1: [Feature Name]**
  - [ ] Design feature specifications
  - [ ] Backend implementation
    - [ ] Database models/migrations
    - [ ] API endpoints
    - [ ] Business logic
    - [ ] Input validation
    - [ ] Error handling
  - [ ] Frontend implementation [if applicable]
    - [ ] UI components
    - [ ] State management
    - [ ] API integration
    - [ ] Form validation
  - [ ] Testing
    - [ ] Unit tests
    - [ ] Integration tests
    - [ ] E2E tests [if applicable]
  - [ ] Documentation
    - [ ] Update README
    - [ ] API documentation
    - [ ] Feature documentation

- [ ] **Feature Group 2: [Feature Name]**
  - [ ] Design feature specifications
  - [ ] Backend implementation
    - [ ] Database models/migrations
    - [ ] API endpoints
    - [ ] Business logic
    - [ ] Input validation
    - [ ] Error handling
  - [ ] Frontend implementation [if applicable]
    - [ ] UI components
    - [ ] State management
    - [ ] API integration
    - [ ] Form validation
  - [ ] Testing
    - [ ] Unit tests
    - [ ] Integration tests
    - [ ] E2E tests [if applicable]
  - [ ] Documentation
    - [ ] Update README
    - [ ] API documentation
    - [ ] Feature documentation

- [ ] **Feature Group 3: [Feature Name]**
  [Follow same structure as above]

---

### Phase 2: Advanced Features

**Goal**: [1 sentence describing Phase 2 goals]

- [ ] **Advanced Feature 1: [Feature Name]**
  - [ ] Research and design
  - [ ] Implementation
  - [ ] Testing
  - [ ] Documentation

- [ ] **Advanced Feature 2: [Feature Name]**
  - [ ] Research and design
  - [ ] Implementation
  - [ ] Testing
  - [ ] Documentation

- [ ] **Performance Optimization**
  - [ ] Profile application performance
  - [ ] Optimize database queries
  - [ ] Implement caching strategy
  - [ ] Optimize frontend bundle size
  - [ ] Add lazy loading where appropriate

- [ ] **Security Hardening**
  - [ ] Security audit
  - [ ] Implement rate limiting
  - [ ] Add input sanitization
  - [ ] Configure CORS properly
  - [ ] Add security headers
  - [ ] Penetration testing

---

### Phase 3: Polish & Launch Preparation

**Goal**: [Prepare for production launch]

- [ ] **Production Readiness**
  - [ ] Setup production environment
  - [ ] Configure production database
  - [ ] Setup production logging and monitoring
  - [ ] Configure production secrets
  - [ ] Setup backup strategy
  - [ ] Create deployment runbook

- [ ] **Testing & QA**
  - [ ] Full regression testing
  - [ ] Performance testing
  - [ ] Security testing
  - [ ] User acceptance testing
  - [ ] Cross-browser testing [if applicable]
  - [ ] Mobile responsiveness testing [if applicable]

- [ ] **Documentation**
  - [ ] Finalize README
  - [ ] Create user guide/documentation
  - [ ] Document deployment process
  - [ ] Create troubleshooting guide
  - [ ] Update API documentation

- [ ] **Launch**
  - [ ] Deploy to production
  - [ ] Monitor for errors
  - [ ] Verify all features working
  - [ ] Announce launch

---

## Completed Tasks

[Move completed tasks here with completion dates, or simply keep them checked above]

### Completed - [Date Range]
- [x] Task 1 (Completed: YYYY-MM-DD)
- [x] Task 2 (Completed: YYYY-MM-DD)

---

## Backlog / Future Tasks

[Tasks that aren't assigned to a phase yet, or ideas for future enhancements]

- [ ] Future enhancement 1
- [ ] Future enhancement 2
- [ ] Nice-to-have feature 3

---

**Last Updated**: [Date]
```

---

## 4. PHASE#.md Template

Use this template for each implementation phase (PHASE0.md, PHASE1.md, etc.). This contains detailed implementation specifications.

```markdown
# Phase [N]: [Phase Name]

## Phase Overview

[Detailed description of this phase's goals and scope. What are we building in this phase? Why is it important? How does it fit into the overall project?]

**Phase Duration**: [Estimated timeframe, if applicable]

**Key Objectives**:
1. [Objective 1]
2. [Objective 2]
3. [Objective 3]

## Success Criteria

What defines completion of this phase?

- [ ] [Criterion 1: e.g., All core API endpoints implemented and tested]
- [ ] [Criterion 2: e.g., Database schema migrated and seeded]
- [ ] [Criterion 3: e.g., Authentication system fully functional]
- [ ] [Criterion 4: e.g., 80%+ test coverage]
- [ ] [Criterion 5: e.g., Documentation complete]

---

## Detailed Implementation Specifications

### Feature 1: [Feature Name]

#### Overview
[Brief description of what this feature does and why it's needed]

#### Requirements

##### Functional Requirements
- [Requirement 1: What the feature must do]
- [Requirement 2: User interactions or workflows]
- [Requirement 3: Expected behavior]

##### Non-Functional Requirements
- **Performance**: [Response time, throughput requirements]
- **Security**: [Auth requirements, data protection]
- **Scalability**: [Expected load]
- **Usability**: [UX considerations]

#### Database Changes

##### New Tables/Collections
```sql
-- Example: User table
CREATE TABLE users (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    email VARCHAR(255) UNIQUE NOT NULL,
    password_hash VARCHAR(255) NOT NULL,
    created_at TIMESTAMP DEFAULT NOW(),
    updated_at TIMESTAMP DEFAULT NOW()
);

CREATE INDEX idx_users_email ON users(email);
```

##### Migrations
- Migration 001: [Description]
- Migration 002: [Description]

##### Data Relationships
[Describe relationships between tables/models]

#### API Endpoints

##### Endpoint 1: [Endpoint Name]
```
POST /api/[resource]
```

**Description**: [What this endpoint does]

**Authentication**: [Required/Optional, method]

**Request Headers**:
```
Content-Type: application/json
Authorization: Bearer [token]
```

**Request Body**:
```json
{
  "field1": "string",
  "field2": 123,
  "field3": {
    "nested": "object"
  }
}
```

**Validation Rules**:
- `field1`: Required, string, max 255 characters
- `field2`: Required, integer, min 0, max 999
- `field3.nested`: Optional, string

**Response (Success - 200/201)**:
```json
{
  "success": true,
  "data": {
    "id": "uuid",
    "field1": "string",
    "field2": 123,
    "created_at": "2024-01-01T00:00:00Z"
  }
}
```

**Response (Error - 400/401/500)**:
```json
{
  "success": false,
  "error": {
    "code": "ERROR_CODE",
    "message": "Human-readable error message",
    "details": {}
  }
}
```

**Error Codes**:
- `VALIDATION_ERROR`: Invalid input data
- `UNAUTHORIZED`: Authentication required
- `RESOURCE_NOT_FOUND`: Resource doesn't exist
- `SERVER_ERROR`: Internal server error

##### Endpoint 2: [Endpoint Name]
[Follow same structure as Endpoint 1]

#### Frontend Components

[If applicable]

##### Component 1: [Component Name]

**Purpose**: [What this component does]

**Props**:
```typescript
interface ComponentProps {
  prop1: string;
  prop2?: number;
  onEvent: (data: EventData) => void;
}
```

**State**:
- [State variable 1]: [Purpose]
- [State variable 2]: [Purpose]

**Behavior**:
- [Behavior description 1]
- [Behavior description 2]

**Styling**: [Tailwind classes / CSS Modules / styled-components approach]

##### Component 2: [Component Name]
[Follow same structure]

#### LLM Integration

[If this feature uses LLMs]

##### Use Case: [Specific LLM use case]

**Provider**: [OpenAI / Anthropic / etc.]
**Model**: [gpt-4, claude-3-opus, etc.]

**Prompt Template**:
```
[System message or prompt template]

User input: {user_input}
Context: {context}

Instructions: [Specific instructions for the LLM]
```

**Input Parameters**:
- `user_input`: [Description]
- `context`: [Description]
- `temperature`: [Value and rationale]
- `max_tokens`: [Value and rationale]

**Response Processing**:
[How the LLM response is parsed and used]

**Error Handling**:
- Rate limit exceeded: [Fallback strategy]
- API timeout: [Retry logic]
- Invalid response: [Validation and recovery]

**Cost Considerations**:
- Estimated tokens per request: [Number]
- Expected requests per day: [Number]
- Monthly cost estimate: [Amount]

#### Logging & Observability

##### What to Log

**Info Level**:
- [Event 1: e.g., User registration successful]
- [Event 2: e.g., API request completed]

**Warning Level**:
- [Event 1: e.g., Rate limit approaching]
- [Event 2: e.g., Slow database query]

**Error Level**:
- [Event 1: e.g., Database connection failed]
- [Event 2: e.g., External API error]

##### Metrics to Track

- [Metric 1: e.g., Registration success rate]
- [Metric 2: e.g., API response time (p50, p95, p99)]
- [Metric 3: e.g., Error rate by endpoint]

##### Traces

[What operations should be traced for performance monitoring]

- [Operation 1: e.g., Full user registration flow]
- [Operation 2: e.g., Database query execution]

##### Alerts

[What conditions should trigger alerts]

- [Alert 1: Error rate > 5% for 5 minutes]
- [Alert 2: API response time p95 > 2 seconds]

#### Testing Requirements

##### Unit Tests

**Files to Test**:
- [File 1]: [What to test]
  - Test case 1: [Description]
  - Test case 2: [Description]
- [File 2]: [What to test]
  - Test case 1: [Description]
  - Test case 2: [Description]

**Target Coverage**: [Percentage, e.g., 80%]

##### Integration Tests

**Scenarios**:
1. [Scenario 1: e.g., User registration end-to-end]
   - Setup: [What needs to be prepared]
   - Actions: [Steps to test]
   - Assertions: [What to verify]

2. [Scenario 2]
   [Follow same structure]

##### E2E Tests

[If applicable]

**User Flows**:
1. [Flow 1: e.g., New user registration and first login]
2. [Flow 2: e.g., Complete user journey through key features]

#### Security Considerations

##### Input Validation
- [What inputs need validation and how]
- [SQL injection prevention]
- [XSS prevention]

##### Authentication & Authorization
- [Who can access this feature]
- [What permissions are required]
- [How permissions are checked]

##### Data Protection
- [What data is sensitive]
- [How sensitive data is encrypted/protected]
- [PII handling]

##### Rate Limiting
- [Rate limits per user/IP]
- [Rate limiting strategy]

---

### Feature 2: [Feature Name]

[Follow the same structure as Feature 1 above. Repeat for each major feature in this phase.]

---

## Dependencies

### Technical Dependencies

**External Services**:
- [Service 1]: [Why it's needed, what happens if unavailable]
- [Service 2]: [Why it's needed, what happens if unavailable]

**Libraries/Packages**:
- [Package 1]: [Purpose, version requirements]
- [Package 2]: [Purpose, version requirements]

### Phase Dependencies

**Previous Phases**:
- Phase [N-1] must be complete because: [Reason]
- Specifically requires: [Specific deliverables from previous phases]

**Blocking Issues**:
- [Issue 1: e.g., Awaiting API access from third-party]
- [Issue 2: e.g., Database migration approval needed]

---

## Risks & Mitigation

| Risk | Impact | Probability | Mitigation Strategy |
|------|--------|-------------|---------------------|
| [Risk 1: e.g., Third-party API instability] | High | Medium | [Implement retry logic, caching, and fallback options] |
| [Risk 2: e.g., Database performance issues] | Medium | Low | [Add proper indexes, implement query optimization, load testing] |
| [Risk 3] | [High/Medium/Low] | [High/Medium/Low] | [Strategy] |

---

## Timeline Estimate

[Optional - rough time estimates]

**Total Phase Duration**: [Estimated timeframe]

**Feature Breakdown**:
- Feature 1: [Estimate]
- Feature 2: [Estimate]
- Testing: [Estimate]
- Documentation: [Estimate]
- Buffer: [Estimate]

**Critical Path**:
[What must happen in sequence? What can happen in parallel?]

---

## Open Questions

[Questions that need answers before or during this phase]

1. [Question 1]
2. [Question 2]
3. [Question 3]

---

## Notes

[Any additional context, decisions made, lessons learned, or important information]

---

**Last Updated**: [Date]
**Status**: [Planning / In Progress / Complete]
```

---

## 5. README.md Template

Use this template for the user-facing README file in the project root.

```markdown
# [Project Name]

[Brief, compelling description of what this project does. 1-2 sentences that immediately communicate value.]

## Features

- Feature 1 description
- Feature 2 description
- Feature 3 description
- Feature 4 description
- Feature 5 description

## Getting Started

### Prerequisites

Before you begin, ensure you have the following installed:

- [Requirement 1] - [Version, e.g., Node.js 18.x or higher]
- [Requirement 2] - [Version, e.g., PostgreSQL 14 or higher]
- [Requirement 3] - [Version, e.g., npm 9.x or higher]

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/username/project-name.git
   cd project-name
   ```

2. Install dependencies:
   ```bash
   npm install
   # or
   yarn install
   # or
   pnpm install
   ```

3. Setup environment variables:
   ```bash
   cp .env.example .env
   ```

   Edit `.env` and fill in the required values:
   ```
   DATABASE_URL=postgresql://user:password@localhost:5432/dbname
   API_KEY=your_api_key_here
   JWT_SECRET=your_jwt_secret_here
   ```

4. Setup the database:
   ```bash
   npm run db:migrate
   npm run db:seed
   ```

### Configuration

[Explain any configuration files or environment variables]

#### Environment Variables

| Variable | Description | Required | Default |
|----------|-------------|----------|---------|
| `DATABASE_URL` | PostgreSQL connection string | Yes | - |
| `API_KEY` | [Service] API key | Yes | - |
| `PORT` | Server port | No | 3000 |
| `LOG_LEVEL` | Logging level (debug/info/warn/error) | No | info |

#### Configuration Files

- `config.json` - [Description of what this configures]
- `.env` - Environment-specific variables

### Running the Project

#### Development Mode

```bash
npm run dev
```

The application will be available at `http://localhost:3000`

#### Production Build

```bash
npm run build
npm start
```

#### Running Tests

```bash
# Run all tests
npm test

# Run tests in watch mode
npm run test:watch

# Run tests with coverage
npm run test:coverage

# Run E2E tests
npm run test:e2e
```

#### Linting and Formatting

```bash
# Lint code
npm run lint

# Format code
npm run format

# Type check (TypeScript)
npm run type-check
```

## Usage

### Basic Usage

[Show the most common use case with code examples]

```typescript
// Example code showing basic usage
import { Something } from 'project-name';

const instance = new Something();
instance.doSomething();
```

### Advanced Usage

[Show more complex scenarios]

```typescript
// Example code showing advanced usage
```

### API Documentation

[If this is an API]

API documentation is available at: `http://localhost:3000/api/docs` (when running in development mode)

Or see [API.md](docs/API.md) for full API reference.

## Project Structure

```
project-name/
├── src/
│   ├── components/     # React components (if applicable)
│   ├── routes/         # API routes or page routes
│   ├── models/         # Data models
│   ├── services/       # Business logic services
│   ├── utils/          # Utility functions
│   └── config/         # Configuration
├── tests/              # Test files
├── docs/               # Development documentation
│   ├── specification.md
│   ├── tasks.md
│   └── PHASE0.md
├── public/             # Static assets (if applicable)
└── scripts/            # Build/deployment scripts
```

## Documentation

For more detailed technical documentation:

- [Technical Specification](docs/specification.md) - Architecture, tech stack, requirements
- [Implementation Tasks](docs/tasks.md) - Current project tasks and progress
- [Phase 0 Details](docs/PHASE0.md) - Foundation phase implementation specs

## Contributing

[If accepting contributions]

We welcome contributions! Please follow these steps:

1. Fork the repository
2. Create a feature branch: `git checkout -b feature/amazing-feature`
3. Make your changes
4. Run tests: `npm test`
5. Commit your changes: `git commit -m 'Add amazing feature'`
6. Push to the branch: `git push origin feature/amazing-feature`
7. Open a Pull Request

### Coding Standards

- Follow the existing code style
- Write tests for new features
- Update documentation as needed
- Ensure all tests pass before submitting PR

## Deployment

[Instructions for deploying to production]

### Deploy to [Platform]

```bash
[Deployment commands]
```

## Troubleshooting

### Common Issues

#### Issue 1: [Common problem]
**Solution**: [How to fix it]

#### Issue 2: [Another common problem]
**Solution**: [How to fix it]

### Getting Help

- Check the [Issues](https://github.com/username/project-name/issues) page
- Join our [Discord/Slack community]
- Email: support@example.com

## Tech Stack

- **Backend**: [Framework and version]
- **Frontend**: [Framework and version]
- **Database**: [Database and version]
- **Deployment**: [Platform]
- **CI/CD**: [Tool]

## License

[License information, e.g., MIT, Apache 2.0, etc.]

This project is licensed under the [LICENSE NAME] - see the [LICENSE](LICENSE) file for details.

## Acknowledgments

- [Credit to inspirations, libraries, or contributors]
- [Thank you notes]

## Contact

**Project Maintainer**: [Name]
- Email: [email]
- GitHub: [@username](https://github.com/username)
- Website: [website]

---

**Built with** ❤️ **using** [Main technologies]
```

---

## 6. Feature Documentation Template

Use this template when creating detailed documentation for complex individual features in `docs/features/[feature-name].md`.

```markdown
# Feature: [Feature Name]

## Overview

[Detailed description of what this feature does, why it exists, and what problem it solves.]

**Status**: [Planning / In Progress / Complete / Deprecated]
**Phase**: [Which phase this belongs to]
**Priority**: [High / Medium / Low]

## User Stories

### Primary User Story
As a [type of user],
I want to [perform some action],
So that [achieve some goal].

### Additional User Stories
1. As a [user type], I want to [action] so that [goal]
2. As a [user type], I want to [action] so that [goal]

## Requirements

### Functional Requirements

1. **[Requirement Category]**
   - [Specific requirement 1]
   - [Specific requirement 2]
   - [Specific requirement 3]

2. **[Requirement Category]**
   - [Specific requirement 1]
   - [Specific requirement 2]

### Non-Functional Requirements

- **Performance**: [Response time, throughput requirements]
- **Security**: [Security considerations]
- **Usability**: [UX requirements]
- **Accessibility**: [Accessibility requirements]

### Acceptance Criteria

- [ ] [Criterion 1: Specific, measurable condition]
- [ ] [Criterion 2: Specific, measurable condition]
- [ ] [Criterion 3: Specific, measurable condition]
- [ ] [Criterion 4: Specific, measurable condition]

## Architecture

### High-Level Design

[Describe the high-level architecture of this feature. Include diagrams if helpful.]

```
[ASCII diagram or description of component interactions]
User → Frontend Component → API Endpoint → Service Layer → Database
```

### Components

#### Backend Components
- **[Component 1]**: [Purpose and responsibilities]
- **[Component 2]**: [Purpose and responsibilities]

#### Frontend Components
[If applicable]
- **[Component 1]**: [Purpose and responsibilities]
- **[Component 2]**: [Purpose and responsibilities]

### Data Flow

1. [Step 1 of data flow]
2. [Step 2 of data flow]
3. [Step 3 of data flow]

## Implementation Details

### Database Schema

```sql
-- Tables or collections needed for this feature
CREATE TABLE feature_data (
    id UUID PRIMARY KEY,
    field1 VARCHAR(255),
    field2 INTEGER,
    created_at TIMESTAMP DEFAULT NOW()
);
```

### API Endpoints

#### [Endpoint Name]
```
[METHOD] /api/path
```

**Request**:
```json
{
  "field": "value"
}
```

**Response**:
```json
{
  "success": true,
  "data": {}
}
```

[Repeat for each endpoint]

### Key Algorithms

[If this feature involves complex algorithms or logic]

**Algorithm: [Name]**

**Purpose**: [What this algorithm does]

**Pseudocode**:
```
function algorithmName(input):
    step 1
    step 2
    return output
```

**Time Complexity**: [O(n), etc.]
**Space Complexity**: [O(1), etc.]

### External Dependencies

- [Dependency 1]: [Why it's needed]
- [Dependency 2]: [Why it's needed]

## User Experience

### User Flow

1. [Step 1 of user interaction]
2. [Step 2 of user interaction]
3. [Step 3 of user interaction]

### UI/UX Considerations

- [Consideration 1]
- [Consideration 2]
- [Consideration 3]

### Error Handling

**User-Facing Errors**:
- [Error scenario 1]: [How it's displayed to user]
- [Error scenario 2]: [How it's displayed to user]

## Testing

### Test Scenarios

#### Scenario 1: [Happy Path]
**Given**: [Initial state]
**When**: [Action taken]
**Then**: [Expected outcome]

#### Scenario 2: [Edge Case]
**Given**: [Initial state]
**When**: [Action taken]
**Then**: [Expected outcome]

#### Scenario 3: [Error Case]
**Given**: [Initial state]
**When**: [Action taken]
**Then**: [Expected outcome]

### Test Coverage

- [ ] Unit tests for [component]
- [ ] Integration tests for [flow]
- [ ] E2E tests for [user journey]
- [ ] Performance tests
- [ ] Security tests

## Security Considerations

### Authentication & Authorization
[How this feature handles auth]

### Input Validation
[What inputs are validated and how]

### Data Protection
[How sensitive data is protected]

### Known Vulnerabilities
[Any security concerns and mitigations]

## Performance Considerations

### Expected Load
[Anticipated usage patterns]

### Optimization Strategies
- [Strategy 1]
- [Strategy 2]

### Caching
[What is cached and for how long]

### Monitoring
[What metrics are tracked]

## Risks & Mitigation

| Risk | Impact | Probability | Mitigation |
|------|--------|-------------|------------|
| [Risk 1] | [High/Medium/Low] | [High/Medium/Low] | [Strategy] |
| [Risk 2] | [High/Medium/Low] | [High/Medium/Low] | [Strategy] |

## Open Questions

1. [Question 1]
2. [Question 2]
3. [Question 3]

## Future Enhancements

[Features or improvements to consider for future iterations]

- [Enhancement 1]
- [Enhancement 2]
- [Enhancement 3]

## Related Documentation

- [Link to related feature documentation]
- [Link to API documentation]
- [Link to architecture diagrams]

## Change Log

| Date | Change | Author |
|------|--------|--------|
| YYYY-MM-DD | [Description of change] | [Name] |
| YYYY-MM-DD | [Description of change] | [Name] |

---

**Last Updated**: [Date]
**Author**: [Name]
**Reviewers**: [Names]
```

---

## Usage Notes

### When to Use Each Template

- **brainstorm.md**: Start here for new projects. Capture raw ideas before structuring them.
- **specification.md**: Use after brainstorm solidifies. This is your main technical reference.
- **tasks.md**: Create early and update constantly. This is your progress tracker.
- **PHASE#.md**: Create one per implementation phase. These contain detailed implementation specs.
- **README.md**: Create early for user-facing documentation. Update as features are added.
- **features/feature-name.md**: Use for complex features that need dedicated documentation.

### Customization

These templates are starting points. Customize them based on:
- Project size (smaller projects need less documentation)
- Team size (larger teams need more structure)
- Project type (web app, API, CLI tool, library, etc.)
- Organization standards (match your company's documentation practices)

### Keeping Templates Updated

As your project evolves, you may find you need:
- Additional sections in templates
- Different organization
- More or less detail

Don't be afraid to adapt these templates to your needs. The goal is useful documentation, not template compliance.

---

**Template Version**: 1.0
**Last Updated**: [Date]
