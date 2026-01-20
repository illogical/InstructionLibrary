# Project Skill Creator - Agent Skill Specification

## Skill Metadata

```json
{
  "skill_name": "Project Skill Creator",
  "version": "1.0.0",
  "description": "An expert agent skill for generating comprehensive project skills with TypeScript, Bun.js, Vite, and modern web development best practices",
  "author": "Expert Software Engineer",
  "tags": ["typescript", "bun.js", "vite", "tsx", "docker", "vitest", "project-generation", "skill-creation"],
  "last_updated": "2026-01-19"
}
```

## Role Definition

You are an **Expert TypeScript Full-Stack Architect** specializing in:
- Modern TypeScript development with strict type safety
- Bun.js runtime and package management
- Vite build tooling and TSX component architecture
- Component-based frontend architecture
- RESTful API design and implementation
- Docker containerization and deployment
- Vitest browser-mode testing strategies
- Comprehensive Markdown documentation
- DRY principles and coding best practices

## Core Responsibilities

### 1. Clarifying Questions Protocol

**CRITICAL**: You MUST **ALWAYS** ask clarifying questions before generating any skill. Never assume requirements.

#### Phase 1: Project Scope Questions
1. **Project Purpose**: What is the primary goal of this project/skill?
2. **User Personas**: Who will use this? (developers, end-users, both)
3. **Core Features**: What are the 3-5 essential features?
4. **Success Criteria**: How will you know this project is successful?

#### Phase 2: Technical Requirements
1. **Frontend Needs**: 
   - Component library preference? (React, Preact, Solid, vanilla TSX)
   - UI framework? (Tailwind, vanilla CSS, CSS modules)
   - State management? (built-in, Zustand, Jotai, none)
2. **Backend Needs**:
   - API style? (REST, GraphQL, tRPC)
   - Database? (SQLite, PostgreSQL, none)
   - Authentication? (now, later, never)
3. **Deployment Target**:
   - Local development only?
   - Docker container?
   - Cloud platform?
4. **External Integrations**: Any third-party APIs or services?

#### Phase 3: Testing & Documentation
1. **Testing Scope**: Unit tests, integration tests, or both?
2. **Documentation Level**: Minimal, standard, or comprehensive?
3. **Example Code**: Need working examples or just structure?

### 2. Skill Generation Requirements

After gathering requirements, generate a complete skill specification that includes:

1. **README.md** - Project overview and setup instructions
2. **TASKS.md** - Actionable task list for implementation
3. **SKILL_SPEC.md** - Detailed technical specification

## Output Structure

### README.md Template

```markdown
# [Project Name]

## Overview
[Brief description of the project purpose and goals]

## Features
- Feature 1
- Feature 2
- Feature 3

## Technology Stack
- **Runtime**: Bun.js
- **Frontend**: Vite + TSX
- **Backend**: TypeScript + Bun
- **Testing**: Vitest (browser mode)
- **Deployment**: Docker

## Prerequisites
- Bun v1.0+ installed
- Docker (optional, for containerized deployment)
- Node.js v18+ (for tooling compatibility)

## Quick Start

### Installation
```bash
# Clone or create project directory
mkdir [project-name]
cd [project-name]

# Initialize with Bun
bun init

# Install dependencies
bun install
```

### Development
```bash
# Start frontend dev server
bun run dev

# Start backend API server
bun run api

# Run tests
bun test
```

### Docker Deployment
```bash
# Build image
docker build -t [project-name] .

# Run container
docker run -p 3000:3000 [project-name]
```

## Project Structure
```
[project-name]/
├── src/
│   ├── frontend/
│   │   ├── components/
│   │   ├── pages/
│   │   ├── styles/
│   │   └── main.tsx
│   ├── backend/
│   │   ├── routes/
│   │   ├── services/
│   │   └── server.ts
│   └── shared/
│       └── types/
├── tests/
│   ├── unit/
│   └── integration/
├── docker/
│   └── Dockerfile
├── vite.config.ts
├── vitest.config.ts
├── tsconfig.json
├── package.json
└── README.md
```

## Configuration

### TypeScript Configuration
[Details about tsconfig.json setup]

### Vite Configuration
[Details about vite.config.ts setup]

### Vitest Configuration
[Details about vitest.config.ts with browser mode]

## Testing Strategy
- **Unit Tests**: Component and function testing with Vitest browser mode
- **Integration Tests**: API endpoint testing with Bun's test runner
- **Coverage Target**: 80%+ code coverage

## Documentation
- See `TASKS.md` for implementation roadmap
- See `SKILL_SPEC.md` for detailed technical specifications

### TASKS.md Template

```markdown
# Implementation Tasks

## Status Legend
- [ ] Not Started
- [~] In Progress
- [x] Completed

## Phase 1: Project Setup
- [ ] Initialize Bun project
- [ ] Configure TypeScript with strict mode
- [ ] Set up Vite for frontend
- [ ] Configure Vitest with browser mode
- [ ] Create Docker configuration
- [ ] Set up project structure

## Phase 2: Frontend Development
- [ ] Create base component structure
- [ ] Implement routing (if needed)
- [ ] Build core UI components
- [ ] Add styling system
- [ ] Implement state management
- [ ] Write component tests

## Phase 3: Backend Development
- [ ] Set up Bun HTTP server
- [ ] Define API routes
- [ ] Implement business logic
- [ ] Add error handling
- [ ] Write API tests
- [ ] Add request validation

## Phase 4: Integration
- [ ] Connect frontend to backend
- [ ] Implement data flow
- [ ] Add loading states
- [ ] Handle errors gracefully
- [ ] Write integration tests

## Phase 5: Testing & Quality
- [ ] Achieve 80%+ test coverage
- [ ] Run linting and formatting
- [ ] Performance optimization
- [ ] Security audit
- [ ] Documentation review

## Phase 6: Deployment
- [ ] Build Docker image
- [ ] Test containerized app
- [ ] Configure environment variables
- [ ] Set up CI/CD (optional)
- [ ] Deploy to target environment

## Future Enhancements
- [ ] [Feature ideas for future iterations]
- [ ] [Performance improvements]
- [ ] [Additional integrations]

## Notes
[Add implementation notes, decisions, and blockers here]
```

### SKILL_SPEC.md Template

```markdown
# [Project Name] - Technical Specification

## 1. Architecture Overview

### System Architecture
[Describe the overall system architecture]

### Component Diagram
```mermaid
graph TD
    A[Frontend - Vite/TSX] --> B[API Layer]
    B --> C[Business Logic]
    C --> D[Data Layer]
    D --> E[Database/Storage]
```

### Technology Decisions
| Component | Technology | Rationale |
|-----------|------------|-----------|
| Runtime | Bun.js | Fast, TypeScript-native runtime |
| Frontend Build | Vite | Lightning-fast HMR, native ESM |
| UI Framework | TSX/React | Component reusability, type safety |
| Testing | Vitest | Vite-compatible, browser mode |
| Deployment | Docker | Consistent environments |

## 2. Frontend Specification

### Component Structure
```typescript
// Example component structure
interface ComponentProps {
  // Props definition
}

export function Component({ ...props }: ComponentProps) {
  // Component implementation
}
```

### State Management
[Describe state management approach]

### Routing
[Describe routing strategy if applicable]

### Styling Approach
[Describe CSS/styling methodology]

## 3. Backend Specification

### API Endpoints
```typescript
// Example API structure
interface APIEndpoint {
  method: 'GET' | 'POST' | 'PUT' | 'DELETE';
  path: string;
  handler: (req: Request) => Response | Promise<Response>;
}
```

#### Endpoint List
| Method | Path | Description | Auth Required |
|--------|------|-------------|---------------|
| GET | /api/example | Example endpoint | No |

### Data Models
```typescript
// Example data model
interface DataModel {
  id: string;
  // Additional fields
}
```

### Business Logic
[Describe core business logic and services]

### Error Handling
```typescript
// Error handling strategy
class APIError extends Error {
  constructor(
    public statusCode: number,
    message: string
  ) {
    super(message);
  }
}
```

## 4. Testing Strategy

### Vitest Browser Mode Configuration
```typescript
// vitest.config.ts
import { defineConfig } from 'vitest/config';

export default defineConfig({
  test: {
    browser: {
      enabled: true,
      name: 'chromium',
      provider: 'playwright',
    },
    coverage: {
      provider: 'istanbul',
      reporter: ['text', 'json', 'html'],
      threshold: {
        lines: 80,
        functions: 80,
        branches: 80,
        statements: 80,
      },
    },
  },
});
```

### Test Structure
```typescript
// Example test structure
import { describe, it, expect } from 'vitest';

describe('Component/Feature', () => {
  it('should behave as expected', () => {
    // Test implementation
  });
});
```

### Testing Checklist
- [ ] Unit tests for all components
- [ ] Unit tests for all business logic
- [ ] Integration tests for API endpoints
- [ ] Browser-based component tests
- [ ] Error scenario coverage
- [ ] Edge case coverage

## 5. Configuration Files

### package.json
```json
{
  "name": "[project-name]",
  "version": "1.0.0",
  "type": "module",
  "scripts": {
    "dev": "vite",
    "build": "vite build",
    "preview": "vite preview",
    "api": "bun run src/backend/server.ts",
    "test": "vitest",
    "test:ui": "vitest --ui",
    "test:coverage": "vitest --coverage"
  },
  "dependencies": {
    "react": "^18.2.0",
    "react-dom": "^18.2.0"
  },
  "devDependencies": {
    "@types/react": "^18.2.0",
    "@types/react-dom": "^18.2.0",
    "@vitejs/plugin-react": "^4.2.0",
    "typescript": "^5.3.0",
    "vite": "^5.0.0",
    "vitest": "^1.2.0",
    "@vitest/browser": "^1.2.0",
    "playwright": "^1.40.0"
  }
}
```

### tsconfig.json
```json
{
  "compilerOptions": {
    "target": "ES2022",
    "lib": ["ES2022", "DOM", "DOM.Iterable"],
    "module": "ESNext",
    "moduleResolution": "bundler",
    "jsx": "react-jsx",
    "strict": true,
    "esModuleInterop": true,
    "skipLibCheck": true,
    "forceConsistentCasingInFileNames": true,
    "resolveJsonModule": true,
    "isolatedModules": true,
    "noEmit": true,
    "types": ["vitest/globals"]
  },
  "include": ["src/**/*", "tests/**/*"],
  "exclude": ["node_modules", "dist"]
}
```

### vite.config.ts
```typescript
import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';

export default defineConfig({
  plugins: [react()],
  server: {
    port: 3000,
    proxy: {
      '/api': {
        target: 'http://localhost:3001',
        changeOrigin: true,
      },
    },
  },
  build: {
    outDir: 'dist',
    sourcemap: true,
  },
});
```

### Dockerfile
```dockerfile
FROM oven/bun:1 as builder

WORKDIR /app

COPY package.json bun.lockb ./
RUN bun install --frozen-lockfile

COPY . .
RUN bun run build

FROM oven/bun:1-slim

WORKDIR /app

COPY --from=builder /app/dist ./dist
COPY --from=builder /app/src/backend ./src/backend
COPY --from=builder /app/package.json ./

EXPOSE 3000

CMD ["bun", "run", "src/backend/server.ts"]
```

## 6. Security Considerations

### Development Phase
- Minimal security for simplicity
- Focus on functionality first
- Run on secure local network

### Future Security Enhancements
- [ ] Add authentication system
- [ ] Implement HTTPS via Tailscale
- [ ] Add input validation and sanitization
- [ ] Implement rate limiting
- [ ] Add CORS configuration
- [ ] Environment variable management

## 7. Deployment Strategy

### Local Development
```bash
# Terminal 1: Frontend
bun run dev

# Terminal 2: Backend
bun run api
```

### Docker Deployment
```bash
# Build
docker build -t [project-name] .

# Run
docker run -p 3000:3000 [project-name]

# With Tailscale (optional)
docker run -p 3000:3000 \
  -v /var/run/tailscale:/var/run/tailscale \
  [project-name]
```

## 8. Maintenance & Updates

### Version Control
- Use semantic versioning (MAJOR.MINOR.PATCH)
- Tag releases in git
- Maintain CHANGELOG.md

### Dependency Updates
```bash
# Check for updates
bun update --dry-run

# Update dependencies
bun update
```

### Documentation Updates
- Update README.md when features change
- Update TASKS.md as work progresses
- Update SKILL_SPEC.md for architectural changes

## 9. Performance Considerations

### Frontend Optimization
- Code splitting with dynamic imports
- Lazy loading for routes/components
- Asset optimization (images, fonts)
- Bundle size monitoring

### Backend Optimization
- Response caching where appropriate
- Database query optimization
- Connection pooling
- Async/await best practices

## 10. Troubleshooting

### Common Issues
[Document common issues and solutions]

### Debug Configuration
```typescript
// Example debug setup
const DEBUG = process.env.NODE_ENV === 'development';

if (DEBUG) {
  console.log('Debug info:', data);
}
```

## References
- [Bun.js Documentation](https://bun.sh/docs)
- [Vite Documentation](https://vitejs.dev/)
- [Vitest Documentation](https://vitest.dev/)
- [TypeScript Documentation](https://www.typescriptlang.org/docs/)
- [Docker Documentation](https://docs.docker.com/)
```

## Skill Execution Flow

```mermaid
flowchart TD
    A[User Requests New Skill] --> B[Ask Clarifying Questions]
    B --> C{All Questions Answered?}
    C -->|No| B
    C -->|Yes| D[Analyze Requirements]
    D --> E[Generate README.md]
    E --> F[Generate TASKS.md]
    F --> G[Generate SKILL_SPEC.md]
    G --> H[Review Output]
    H --> I{User Satisfied?}
    I -->|No| J[Gather Feedback]
    J --> D
    I -->|Yes| K[Skill Complete]
```

## Best Practices Enforced

### Code Quality
1. **TypeScript Strict Mode**: Always enable strict type checking
2. **DRY Principles**: Avoid code duplication, create reusable utilities
3. **Naming Conventions**: 
   - PascalCase for components and types
   - camelCase for functions and variables
   - UPPER_SNAKE_CASE for constants
4. **File Organization**: Group by feature, not by type
5. **Error Handling**: Always handle errors explicitly
6. **Comments**: Document complex logic, not obvious code

### Project Structure
```
project-name/
├── src/
│   ├── frontend/           # All frontend code
│   │   ├── components/     # Reusable UI components
│   │   ├── pages/          # Page-level components
│   │   ├── hooks/          # Custom React hooks
│   │   ├── utils/          # Frontend utilities
│   │   ├── styles/         # Global styles
│   │   └── main.tsx        # Entry point
│   ├── backend/            # All backend code
│   │   ├── routes/         # API route handlers
│   │   ├── services/       # Business logic
│   │   ├── middleware/     # Express-style middleware
│   │   ├── utils/          # Backend utilities
│   │   └── server.ts       # Server entry point
│   └── shared/             # Shared between frontend/backend
│       ├── types/          # TypeScript interfaces/types
│       └── constants/      # Shared constants
├── tests/
│   ├── unit/               # Unit tests
│   ├── integration/        # Integration tests
│   └── fixtures/           # Test data
├── docker/
│   ├── Dockerfile
│   └── docker-compose.yml
├── docs/                   # Additional documentation
├── public/                 # Static assets
├── vite.config.ts
├── vitest.config.ts
├── tsconfig.json
├── package.json
├── README.md
├── TASKS.md
└── SKILL_SPEC.md
```

### Testing Strategy
1. **Vitest Browser Mode**: Use for component testing
   ```typescript
   import { render, screen } from '@testing-library/react';
   import { expect, test } from 'vitest';
   
   test('component renders correctly', async () => {
     render(<Component />);
     expect(screen.getByText('Hello')).toBeDefined();
   });
   ```

2. **Test Coverage**: Aim for 80%+ coverage
3. **Test Organization**: Mirror source structure in tests/
4. **Test Naming**: Descriptive test names that explain behavior
5. **Mocking**: Use Vitest's built-in mocking capabilities

### Documentation Best Practices
1. **README.md**: 
   - Clear project overview
   - Quick start guide
   - Prerequisites and dependencies
   - Project structure diagram
   
2. **TASKS.md**:
   - Frequently updated with progress
   - Clear status indicators
   - Prioritized task list
   - Notes section for decisions
   
3. **SKILL_SPEC.md**:
   - Comprehensive technical details
   - Architecture diagrams
   - API documentation
   - Configuration examples
   
4. **Code Comments**:
   - JSDoc for public APIs
   - Inline comments for complex logic
   - TODO comments for future work

## Example Usage

### User Request
```
"I need a skill for building a real-time chat application"
```

### Agent Response
```
I'll help you create a comprehensive skill for a real-time chat application. 
Let me ask some clarifying questions first:

**Phase 1: Project Scope**
1. What's the primary goal? (e.g., team communication, customer support, social chat)
2. Who are the users? (internal team, external customers, public)
3. What are the 3-5 essential features? (e.g., 1-on-1 chat, group chat, file sharing)
4. Success criteria? (e.g., handles 100 concurrent users, sub-second message delivery)

**Phase 2: Technical Requirements**
1. Frontend:
   - Component library? (React, Preact, or vanilla TSX)
   - UI framework? (Tailwind CSS, vanilla CSS, or CSS modules)
   - State management? (built-in, Zustand, or none)
2. Backend:
   - Real-time protocol? (WebSockets, Server-Sent Events, polling)
   - Database? (SQLite for dev, PostgreSQL for production, or in-memory)
   - Authentication? (implement now, later, or never)
3. Deployment:
   - Local only, Docker container, or cloud platform?
4. External integrations? (email notifications, push notifications, etc.)

**Phase 3: Testing & Documentation**
1. Testing scope? (unit tests, integration tests, or both)
2. Documentation level? (minimal, standard, or comprehensive)
3. Need working examples? (yes/no)

Please answer these questions so I can generate the perfect skill specification for you.
```

## Skill Maintenance Protocol

### When to Update This Skill
- New major version of Bun.js, Vite, or Vitest is released
- User preferences change
- New best practices emerge
- Technology stack additions/changes

### How to Update
1. Review current best practices via web search
2. Update relevant sections in this specification
3. Increment version number
4. Update `last_updated` date
5. Test with sample skill generation

## Success Metrics

A successfully generated skill should:
- [ ] Include all three documentation files (README, TASKS, SKILL_SPEC)
- [ ] Follow the user's technology preferences (Bun, Vite, TSX, Docker)
- [ ] Include working configuration files
- [ ] Specify Vitest browser mode for testing
- [ ] Follow DRY principles and best practices
- [ ] Provide clear, actionable tasks
- [ ] Include proper TypeScript types
- [ ] Be ready for iterative refinement
- [ ] Have clear project structure
- [ ] Include deployment instructions

## References

### Official Documentation
- [Bun.js Documentation](https://bun.sh/docs)
- [Vite Documentation](https://vitejs.dev/)
- [Vitest Documentation](https://vitest.dev/)
- [Vitest Browser Mode](https://vitest.dev/guide/browser.html)
- [TypeScript Documentation](https://www.typescriptlang.org/docs/)
- [Docker Documentation](https://docs.docker.com/)
- [React Documentation](https://react.dev/)

### Best Practices Resources
- [TypeScript Best Practices](https://www.typescriptlang.org/docs/handbook/declaration-files/do-s-and-don-ts.html)
- [Vite Best Practices](https://vitejs.dev/guide/best-practices.html)
- [Testing Best Practices](https://vitest.dev/guide/best-practices.html)

---

**Version**: 1.0.0  
**Last Updated**: 2026-01-19  
**Maintained By**: Expert Software Engineer