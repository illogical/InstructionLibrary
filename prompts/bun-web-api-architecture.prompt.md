---
agent: 'agent'
description: 'Standard project architecture for web applications and web APIs using Bun.js, Bun.Serve, Vitest, modern vanilla CSS, and TypeScript.'
---
# Bun.js Web Application and API Architecture

Your task is to create or maintain a web application and API project using modern standards with Bun.js. This architecture separates concerns between the web frontend and API backend while leveraging Bun's native capabilities.

## Project Structure

Organize the project with clear separation between web and API:

```
project-root/
├── .env                    # Environment variables (not committed)
├── .env.example            # Example environment configuration
├── .gitignore             # Git ignore file
├── bunfig.toml            # Bun configuration
├── package.json           # Dependencies and scripts
├── tsconfig.json          # TypeScript configuration
├── vitest.config.ts       # Vitest configuration
├── web/                   # Web frontend application
│   ├── public/            # Static assets
│   │   ├── index.html
│   │   └── assets/        # Images, fonts, etc.
│   ├── src/
│   │   ├── main.ts        # Entry point
│   │   ├── styles/        # CSS files
│   │   │   ├── reset.css
│   │   │   ├── variables.css
│   │   │   └── main.css
│   │   ├── components/    # UI components
│   │   ├── utils/         # Utility functions
│   │   └── types/         # TypeScript types
│   └── server.ts          # Bun.Serve for web
├── api/                   # API backend
│   ├── src/
│   │   ├── index.ts       # API entry point
│   │   ├── routes/        # API routes
│   │   ├── handlers/      # Request handlers
│   │   ├── middleware/    # Middleware functions
│   │   ├── services/      # Business logic
│   │   ├── models/        # Data models
│   │   ├── utils/         # Utility functions
│   │   └── types/         # TypeScript types
│   └── server.ts          # Bun.Serve for API
└── tests/                 # Test files
    ├── web/               # Web tests
    └── api/               # API tests
```

## Bun.Serve Configuration

### Web Server (web/server.ts)

Use Bun.Serve to serve the web application with static file handling:

```typescript
import { file } from "bun";
import { join } from "path";

const PORT = parseInt(process.env.WEB_PORT || "3000");
const PUBLIC_DIR = join(import.meta.dir, "public");

Bun.serve({
  port: PORT,
  async fetch(req) {
    const url = new URL(req.url);
    let filePath = url.pathname;
    
    // Default to index.html for root or paths without file extensions
    // Check if path has a file extension (e.g., .js, .css, .png)
    if (filePath === "/" || !/\.[a-zA-Z0-9]+$/.test(filePath)) {
      filePath = "/index.html";
    }
    
    const fullPath = join(PUBLIC_DIR, filePath);
    
    try {
      const fileContent = file(fullPath);
      return new Response(fileContent);
    } catch {
      // 404 handling - return index.html for client-side routing
      const indexFile = file(join(PUBLIC_DIR, "index.html"));
      return new Response(indexFile, { status: 200 });
    }
  },
  error(error) {
    return new Response(`Error: ${error.message}`, { status: 500 });
  },
});

console.log(`Web server running on http://localhost:${PORT}`);
```

### API Server (api/server.ts)

Use Bun.Serve for the API with routing and middleware:

```typescript
const PORT = parseInt(process.env.API_PORT || "3001");

interface Context {
  params?: Record<string, string>;
  body?: any;
}

type Handler = (req: Request, ctx: Context) => Promise<Response> | Response;
type Middleware = (req: Request, ctx: Context, next: () => Promise<Response>) => Promise<Response>;

const routes = new Map<string, Map<string, Handler>>();
const middleware: Middleware[] = [];

// Middleware registration
function use(fn: Middleware) {
  middleware.push(fn);
}

// Route registration
function route(method: string, path: string, handler: Handler) {
  if (!routes.has(path)) {
    routes.set(path, new Map());
  }
  routes.get(path)!.set(method, handler);
}

// CORS middleware
use(async (req, ctx, next) => {
  const response = await next();
  const headers = new Headers(response.headers);
  headers.set("Access-Control-Allow-Origin", process.env.CORS_ORIGIN || "*");
  headers.set("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
  headers.set("Access-Control-Allow-Headers", "Content-Type, Authorization");
  
  return new Response(response.body, {
    status: response.status,
    statusText: response.statusText,
    headers,
  });
});

// Logging middleware
use(async (req, ctx, next) => {
  const start = Date.now();
  const response = await next();
  const duration = Date.now() - start;
  console.log(`${req.method} ${new URL(req.url).pathname} - ${response.status} (${duration}ms)`);
  return response;
});

// Example routes
route("GET", "/api/health", async () => {
  return Response.json({ status: "ok", timestamp: new Date().toISOString() });
});

route("GET", "/api/users", async () => {
  return Response.json({ users: [] });
});

Bun.serve({
  port: PORT,
  async fetch(req) {
    const ctx: Context = {};
    const url = new URL(req.url);
    
    // Handle OPTIONS requests for CORS
    if (req.method === "OPTIONS") {
      return new Response(null, { status: 204 });
    }
    
    // Parse JSON body for POST/PUT/PATCH
    if (["POST", "PUT", "PATCH"].includes(req.method)) {
      try {
        ctx.body = await req.json();
      } catch {
        ctx.body = null;
      }
    }
    
    // Find matching route
    const routeMap = routes.get(url.pathname);
    const handler = routeMap?.get(req.method);
    
    if (!handler) {
      return Response.json({ error: "Not found" }, { status: 404 });
    }
    
    // Execute middleware chain
    let index = 0;
    const next = async (): Promise<Response> => {
      if (index < middleware.length) {
        const mw = middleware[index++];
        return mw(req, ctx, next);
      }
      return handler(req, ctx);
    };
    
    try {
      return await next();
    } catch (error) {
      console.error("Request error:", error);
      return Response.json(
        { error: error instanceof Error ? error.message : "Internal server error" },
        { status: 500 }
      );
    }
  },
  error(error) {
    console.error("Server error:", error);
    return Response.json({ error: "Server error" }, { status: 500 });
  },
});

console.log(`API server running on http://localhost:${PORT}`);
```

## Modern Vanilla CSS

### CSS Architecture Principles

1. **Use CSS Custom Properties (Variables)** for theming and consistency
2. **Leverage Modern CSS Features**: Grid, Flexbox, Container Queries, :has(), :is(), :where()
3. **Mobile-First Responsive Design** with modern media queries
4. **CSS Layers** for better cascade management
5. **Logical Properties** for better internationalization

### Example: CSS Variables (web/src/styles/variables.css)

```css
:root {
  /* Color System */
  --color-primary: hsl(220, 90%, 56%);
  --color-primary-hover: hsl(220, 90%, 48%);
  --color-secondary: hsl(280, 70%, 60%);
  --color-accent: hsl(340, 80%, 58%);
  
  /* Neutral Colors */
  --color-text: hsl(0, 0%, 13%);
  --color-text-muted: hsl(0, 0%, 40%);
  --color-background: hsl(0, 0%, 100%);
  --color-surface: hsl(0, 0%, 98%);
  --color-border: hsl(0, 0%, 88%);
  
  /* Spacing Scale */
  --space-1: 0.25rem;
  --space-2: 0.5rem;
  --space-3: 0.75rem;
  --space-4: 1rem;
  --space-5: 1.25rem;
  --space-6: 1.5rem;
  --space-8: 2rem;
  --space-10: 2.5rem;
  --space-12: 3rem;
  --space-16: 4rem;
  
  /* Typography */
  --font-sans: system-ui, -apple-system, "Segoe UI", Roboto, sans-serif;
  --font-mono: ui-monospace, "Cascadia Code", "Source Code Pro", monospace;
  
  --text-xs: 0.75rem;
  --text-sm: 0.875rem;
  --text-base: 1rem;
  --text-lg: 1.125rem;
  --text-xl: 1.25rem;
  --text-2xl: 1.5rem;
  --text-3xl: 1.875rem;
  --text-4xl: 2.25rem;
  
  /* Layout */
  --border-radius: 0.5rem;
  --border-radius-sm: 0.25rem;
  --border-radius-lg: 1rem;
  
  /* Shadows */
  --shadow-sm: 0 1px 2px 0 rgb(0 0 0 / 0.05);
  --shadow: 0 1px 3px 0 rgb(0 0 0 / 0.1), 0 1px 2px -1px rgb(0 0 0 / 0.1);
  --shadow-md: 0 4px 6px -1px rgb(0 0 0 / 0.1), 0 2px 4px -2px rgb(0 0 0 / 0.1);
  --shadow-lg: 0 10px 15px -3px rgb(0 0 0 / 0.1), 0 4px 6px -4px rgb(0 0 0 / 0.1);
  
  /* Transitions */
  --transition-fast: 150ms cubic-bezier(0.4, 0, 0.2, 1);
  --transition-base: 200ms cubic-bezier(0.4, 0, 0.2, 1);
  --transition-slow: 300ms cubic-bezier(0.4, 0, 0.2, 1);
  
  /* Breakpoints (for JS) */
  --breakpoint-sm: 640px;
  --breakpoint-md: 768px;
  --breakpoint-lg: 1024px;
  --breakpoint-xl: 1280px;
}

/* Dark mode support */
@media (prefers-color-scheme: dark) {
  :root {
    --color-text: hsl(0, 0%, 95%);
    --color-text-muted: hsl(0, 0%, 70%);
    --color-background: hsl(0, 0%, 10%);
    --color-surface: hsl(0, 0%, 13%);
    --color-border: hsl(0, 0%, 25%);
  }
}
```

### Example: Modern CSS Reset (web/src/styles/reset.css)

```css
/* Modern CSS Reset */
*, *::before, *::after {
  box-sizing: border-box;
  margin: 0;
  padding: 0;
}

html {
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-rendering: optimizeLegibility;
}

body {
  min-height: 100vh;
  line-height: 1.5;
  font-family: var(--font-sans);
  color: var(--color-text);
  background-color: var(--color-background);
}

img, picture, video, canvas, svg {
  display: block;
  max-width: 100%;
}

input, button, textarea, select {
  font: inherit;
}

p, h1, h2, h3, h4, h5, h6 {
  overflow-wrap: break-word;
}

button {
  cursor: pointer;
  border: none;
  background: none;
}

a {
  color: inherit;
  text-decoration: none;
}
```

### Example: Component Styles with Modern Features

```css
/* Using modern CSS features */
.card {
  /* Container queries for responsive components */
  container-type: inline-size;
  
  padding: var(--space-4);
  background: var(--color-surface);
  border: 1px solid var(--color-border);
  border-radius: var(--border-radius);
  box-shadow: var(--shadow);
  
  /* Logical properties for better i18n */
  margin-block-end: var(--space-4);
  padding-inline: var(--space-4);
  padding-block: var(--space-3);
}

/* Container query instead of media query */
@container (min-width: 400px) {
  .card {
    display: grid;
    grid-template-columns: auto 1fr;
    gap: var(--space-4);
  }
}

/* Modern selectors */
/* Note: CSS nesting (& syntax) requires Chrome 112+, Firefox 117+, Safari 16.5+ */
/* For broader support, use traditional selectors or a PostCSS plugin */
.button {
  padding: var(--space-3) var(--space-6);
  background: var(--color-primary);
  color: white;
  border-radius: var(--border-radius);
  transition: background var(--transition-base);
}

.button:hover {
  background: var(--color-primary-hover);
}

.button:is(:focus-visible, :active) {
  outline: 2px solid var(--color-primary);
  outline-offset: 2px;
}

/* Grid with auto-fit for responsive layouts */
.grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(min(250px, 100%), 1fr));
  gap: var(--space-4);
}

/* Modern flexbox with gap */
.stack {
  display: flex;
  flex-direction: column;
  gap: var(--space-4);
}

.cluster {
  display: flex;
  flex-wrap: wrap;
  gap: var(--space-2);
  align-items: center;
}
```

## Vitest Configuration and Testing

### Vitest Configuration (vitest.config.ts)

```typescript
import { defineConfig } from "vitest/config";

export default defineConfig({
  test: {
    globals: true,
    environment: "node", // or 'jsdom' for DOM testing
    coverage: {
      provider: "v8",
      reporter: ["text", "json", "html"],
      exclude: [
        "node_modules/",
        "tests/",
        "**/*.config.ts",
        "**/*.d.ts",
      ],
    },
    include: ["tests/**/*.test.ts", "tests/**/*.spec.ts"],
    setupFiles: ["./tests/setup.ts"],
  },
});
```

### Testing Patterns

#### API Testing Example (tests/api/health.test.ts)

```typescript
import { describe, it, expect, beforeAll, afterAll } from "vitest";

describe("API Health Endpoint", () => {
  let baseUrl: string;
  
  beforeAll(() => {
    baseUrl = `http://localhost:${process.env.API_PORT || 3001}`;
  });
  
  it("should return health status", async () => {
    const response = await fetch(`${baseUrl}/api/health`);
    const data = await response.json();
    
    expect(response.status).toBe(200);
    expect(data).toHaveProperty("status", "ok");
    expect(data).toHaveProperty("timestamp");
  });
  
  it("should handle CORS headers", async () => {
    const response = await fetch(`${baseUrl}/api/health`);
    
    expect(response.headers.get("Access-Control-Allow-Origin")).toBeTruthy();
  });
});
```

#### Unit Testing Example (tests/api/services/user.test.ts)

```typescript
import { describe, it, expect, vi } from "vitest";
import { UserService } from "../../../api/src/services/user";

describe("UserService", () => {
  it("should create a new user", async () => {
    const userData = {
      name: "Test User",
      email: "test@example.com",
    };
    
    const user = await UserService.create(userData);
    
    expect(user).toHaveProperty("id");
    expect(user.name).toBe(userData.name);
    expect(user.email).toBe(userData.email);
  });
  
  it("should throw error for invalid email", async () => {
    const userData = {
      name: "Test User",
      email: "invalid-email",
    };
    
    await expect(UserService.create(userData)).rejects.toThrow("Invalid email");
  });
});
```

#### Integration Testing with Mocking

```typescript
import { describe, it, expect, vi, beforeEach } from "vitest";

// Mock external dependencies
vi.mock("../../../api/src/services/database", () => ({
  db: {
    query: vi.fn(),
    insert: vi.fn(),
  },
}));

describe("User API Integration", () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });
  
  it("should fetch users from database", async () => {
    const mockUsers = [
      { id: 1, name: "User 1" },
      { id: 2, name: "User 2" },
    ];
    
    const { db } = await import("../../../api/src/services/database");
    vi.mocked(db.query).mockResolvedValue(mockUsers);
    
    const response = await fetch("http://localhost:3001/api/users");
    const data = await response.json();
    
    expect(db.query).toHaveBeenCalledOnce();
    expect(data.users).toEqual(mockUsers);
  });
});
```

## Environment Variable Handling

### .env File Structure

Create a `.env` file for local development (never commit this):

```env
# Server Configuration
WEB_PORT=3000
API_PORT=3001
NODE_ENV=development

# API Configuration
API_URL=http://localhost:3001
CORS_ORIGIN=http://localhost:3000

# Database
DATABASE_URL=postgres://user:password@localhost:5432/dbname

# Authentication
JWT_SECRET=your-secret-key-here
JWT_EXPIRY=24h

# External Services
SMTP_HOST=smtp.example.com
SMTP_PORT=587
SMTP_USER=
SMTP_PASS=

# Feature Flags
ENABLE_ANALYTICS=false
ENABLE_DEBUG=true
```

### .env.example File

Create a `.env.example` file to commit to version control:

```env
# Server Configuration
WEB_PORT=3000
API_PORT=3001
NODE_ENV=development

# API Configuration
API_URL=http://localhost:3001
CORS_ORIGIN=http://localhost:3000

# Database
DATABASE_URL=

# Authentication
JWT_SECRET=
JWT_EXPIRY=24h

# External Services
SMTP_HOST=
SMTP_PORT=587
SMTP_USER=
SMTP_PASS=

# Feature Flags
ENABLE_ANALYTICS=false
ENABLE_DEBUG=true
```

### Environment Variable Access in Code

Bun has built-in support for .env files. Access environment variables using `process.env`:

```typescript
// api/src/config.ts
export const config = {
  port: parseInt(process.env.API_PORT || "3001"),
  nodeEnv: process.env.NODE_ENV || "development",
  database: {
    url: process.env.DATABASE_URL,
  },
  jwt: {
    secret: process.env.JWT_SECRET,
    expiry: process.env.JWT_EXPIRY || "24h",
  },
  cors: {
    origin: process.env.CORS_ORIGIN || "*",
  },
  features: {
    analytics: process.env.ENABLE_ANALYTICS === "true",
    debug: process.env.ENABLE_DEBUG === "true",
  },
} as const;

// Type-safe environment validation
if (!config.jwt.secret && config.nodeEnv === "production") {
  throw new Error("JWT_SECRET must be set in production");
}
```

### Environment-Specific Configuration

```typescript
// api/src/env.ts
import { z } from "zod";

const envSchema = z.object({
  NODE_ENV: z.enum(["development", "production", "test"]).default("development"),
  API_PORT: z.coerce.number().default(3001),
  WEB_PORT: z.coerce.number().default(3000),
  DATABASE_URL: z.string().url(),
  JWT_SECRET: z.string().min(32),
  CORS_ORIGIN: z.string().url().optional(),
});

export const env = envSchema.parse(process.env);
```

## Package.json Scripts

```json
{
  "name": "bun-web-api-project",
  "version": "1.0.0",
  "scripts": {
    "dev:web": "bun run web/server.ts",
    "dev:api": "bun run api/server.ts",
    "dev": "bun run dev:web & bun run dev:api",
    "build:web": "bun build web/src/main.ts --outdir web/public/dist --minify",
    "build:api": "bun build api/src/index.ts --outdir api/dist --target bun",
    "build": "bun run build:web && bun run build:api",
    "test": "vitest run",
    "test:watch": "vitest",
    "test:coverage": "vitest run --coverage",
    "type-check": "tsc --noEmit",
    "lint": "eslint . --ext .ts,.tsx",
    "format": "prettier --write ."
  },
  "dependencies": {
    "zod": "^3.22.4"
  },
  "devDependencies": {
    "@types/bun": "latest",
    "typescript": "^5.3.3",
    "vitest": "^1.1.0",
    "@vitest/coverage-v8": "^1.1.0"
  }
}
```

## TypeScript Configuration

```json
{
  "compilerOptions": {
    "target": "ESNext",
    "module": "ESNext",
    "lib": ["ESNext"],
    "moduleResolution": "bundler",
    "strict": true,
    "esModuleInterop": true,
    "skipLibCheck": true,
    "forceConsistentCasingInFileNames": true,
    "resolveJsonModule": true,
    "isolatedModules": true,
    "types": ["bun-types", "vitest/globals"],
    "paths": {
      "@web/*": ["./web/src/*"],
      "@api/*": ["./api/src/*"]
    }
  },
  "include": ["web/**/*", "api/**/*", "tests/**/*"],
  "exclude": ["node_modules", "dist"]
}
```

## Best Practices

### 1. Separation of Concerns
- Keep web and API completely separate with their own directories
- Web should only contain frontend code and static assets
- API should only contain backend logic and data handling
- Share TypeScript types through a common types package if needed

### 2. Bun.Serve Usage
- Use Bun.serve for both web and API servers
- Handle errors gracefully with custom error responses
- Implement proper CORS handling in API
- Use middleware pattern for common functionality
- Leverage Bun's native performance optimizations

### 3. Modern CSS
- Use CSS custom properties for all theming values
- Implement dark mode with `prefers-color-scheme`
- Use modern layout techniques (Grid, Flexbox) over frameworks
- Leverage container queries for component-level responsiveness
- Use logical properties for better internationalization support
- Implement CSS layers for better cascade management

### 4. Testing Strategy
- Write unit tests for business logic and utilities
- Write integration tests for API endpoints
- Use Vitest's built-in mocking capabilities
- Aim for high coverage on critical paths
- Use test.each for parameterized tests
- Mock external dependencies and services

### 5. Environment Management
- Never commit .env files to version control
- Always provide .env.example with all required variables
- Validate environment variables at startup
- Use type-safe environment configuration with Zod
- Document required environment variables in README
- Use different .env files for different environments

### 6. Development Workflow
- Run web and API servers concurrently during development
- Use TypeScript for type safety across the entire stack
- Implement hot reloading for fast development iteration
- Use Bun's built-in package manager for fast installs
- Leverage Bun's native TypeScript support (no transpilation needed)

### 7. Production Considerations
- Build and minify frontend assets
- Set appropriate CORS origins (not "*")
- Use production-grade secret keys
- Enable proper logging and monitoring
- Implement rate limiting and security headers
- Use environment-specific configuration
- Consider using a reverse proxy (nginx, caddy) in production

## Security Guidelines

1. **Environment Variables**: Never hardcode secrets; always use environment variables
2. **CORS**: Configure CORS appropriately for your domain in production
3. **Input Validation**: Validate all user inputs on the API side
4. **Authentication**: Implement proper JWT or session-based authentication
5. **HTTPS**: Always use HTTPS in production
6. **Rate Limiting**: Implement rate limiting on API endpoints
7. **Content Security Policy**: Set appropriate CSP headers for the web app
8. **SQL Injection**: Use parameterized queries or ORMs
9. **XSS Protection**: Sanitize user-generated content
10. **Dependencies**: Regularly update dependencies and audit for vulnerabilities

## Common Patterns

### API Response Format

```typescript
// Success response
interface SuccessResponse<T> {
  success: true;
  data: T;
  message?: string;
}

// Error response
interface ErrorResponse {
  success: false;
  error: {
    code: string;
    message: string;
    details?: any;
  };
}

type ApiResponse<T> = SuccessResponse<T> | ErrorResponse;
```

### Error Handling in API

```typescript
class ApiError extends Error {
  constructor(
    public statusCode: number,
    public code: string,
    message: string,
    public details?: any
  ) {
    super(message);
    this.name = "ApiError";
  }
}

// In route handler
try {
  // ... your code
} catch (error) {
  if (error instanceof ApiError) {
    return Response.json({
      success: false,
      error: {
        code: error.code,
        message: error.message,
        details: error.details,
      },
    }, { status: error.statusCode });
  }
  
  return Response.json({
    success: false,
    error: {
      code: "INTERNAL_ERROR",
      message: "An unexpected error occurred",
    },
  }, { status: 500 });
}
```

### Frontend API Client

```typescript
// web/src/utils/api.ts
class ApiClient {
  private baseUrl: string;
  
  constructor(baseUrl: string) {
    this.baseUrl = baseUrl;
  }
  
  async request<T>(
    endpoint: string,
    options?: RequestInit
  ): Promise<T> {
    const response = await fetch(`${this.baseUrl}${endpoint}`, {
      ...options,
      headers: {
        "Content-Type": "application/json",
        ...options?.headers,
      },
    });
    
    if (!response.ok) {
      const error = await response.json();
      throw new Error(error.error?.message || "Request failed");
    }
    
    return response.json();
  }
  
  get<T>(endpoint: string): Promise<T> {
    return this.request<T>(endpoint);
  }
  
  post<T>(endpoint: string, data: any): Promise<T> {
    return this.request<T>(endpoint, {
      method: "POST",
      body: JSON.stringify(data),
    });
  }
  
  put<T>(endpoint: string, data: any): Promise<T> {
    return this.request<T>(endpoint, {
      method: "PUT",
      body: JSON.stringify(data),
    });
  }
  
  delete<T>(endpoint: string): Promise<T> {
    return this.request<T>(endpoint, {
      method: "DELETE",
    });
  }
}

// Note: In Bun, environment variables are accessed via process.env
// For build-time variable replacement, use a bundler like esbuild or Vite
const apiUrl = typeof process !== 'undefined' && process.env.API_URL 
  ? process.env.API_URL 
  : "http://localhost:3001";

export const api = new ApiClient(apiUrl);
```

## Summary

This architecture provides:
- Clear separation between web frontend and API backend
- Bun.Serve for high-performance server handling
- Modern vanilla CSS with custom properties and latest features
- Comprehensive Vitest testing setup
- Secure and type-safe environment variable handling
- TypeScript throughout for type safety
- Fast development with Bun's native capabilities
- Production-ready patterns and best practices
