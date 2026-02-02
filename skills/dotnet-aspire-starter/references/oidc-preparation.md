# OIDC Authentication Preparation

OIDC authentication is prepared but commented out in scaffolded projects. This guide shows how to enable it.

## Architecture

The scaffolded auth setup uses:

| Layer | Auth Type | Purpose |
|-------|-----------|---------|
| **Web (Frontend)** | Cookie + OIDC | User login via identity provider |
| **API (Backend)** | JWT Bearer | Validate tokens from frontend |
| **Token Flow** | Authorization Code + PKCE | Secure token exchange |

## Prerequisites

Choose an identity provider:

| Provider | Use Case | Setup Guide |
|----------|----------|-------------|
| **Azure AD / Entra ID** | Enterprise, Microsoft 365 integration | [Azure AD Setup](#azure-ad-setup) |
| **Auth0** | Multi-tenant SaaS | [Auth0 Setup](#auth0-setup) |
| **Keycloak** | Self-hosted, open source | [Keycloak Setup](#keycloak-setup) |
| **Duende IdentityServer** | Full control, ASP.NET Core | [IdentityServer Setup](#identityserver-setup) |

## Step 1: Register Applications

### Azure AD Setup

1. Go to [Azure Portal](https://portal.azure.com) → Azure Active Directory → App Registrations
2. Click **New registration**

**For Web (Frontend):**
- Name: `YourProject-Web`
- Redirect URI: `https://localhost:7001/signin-oidc` (adjust port)
- Click **Register**
- Note the **Application (client) ID**
- Navigate to **Certificates & secrets** → **New client secret**
- Note the **Client secret value**

**For API (Backend):**
- Name: `YourProject-Api`
- Click **Register**
- Navigate to **Expose an API** → **Add a scope**
- Scope name: `access_as_user`
- Who can consent: Admins and users
- Note the **Application ID URI** (e.g., `api://abc-123`)

**Connect Web to API:**
- Go to Web app registration → **API permissions**
- **Add a permission** → **My APIs** → Select your API
- Check `access_as_user` → **Add permissions**

### Auth0 Setup

1. Go to [Auth0 Dashboard](https://manage.auth0.com)
2. Applications → **Create Application**

**For Web:**
- Name: `YourProject-Web`
- Type: **Regular Web Application**
- Settings:
  - Allowed Callback URLs: `https://localhost:7001/signin-oidc`
  - Allowed Logout URLs: `https://localhost:7001/signout-callback-oidc`
- Note **Domain**, **Client ID**, and **Client Secret**

**For API:**
- APIs → **Create API**
- Name: `YourProject-Api`
- Identifier: `https://yourproject-api`
- Note the **Identifier** (this is the audience)

### Keycloak Setup

1. Open Keycloak Admin Console
2. Create a new realm or use existing
3. Clients → **Create client**

**For Web:**
- Client ID: `yourproject-web`
- Client authentication: ON
- Standard flow: enabled
- Valid redirect URIs: `https://localhost:7001/*`
- Note the **Client secret** from Credentials tab

**For API:**
- Typically no separate client needed
- Use the realm's token endpoint for validation

## Step 2: Configure Web (Frontend)

Update `appsettings.json`:

```json
{
  "Auth": {
    "Authority": "https://login.microsoftonline.com/{tenant-id}/v2.0",  // Azure AD
    "ClientId": "your-web-client-id",
    "ClientSecret": "your-web-client-secret",
    "Scopes": [ "openid", "profile", "email", "api://your-api-id/access_as_user" ]
  }
}
```

**For Auth0:**
```json
{
  "Auth": {
    "Authority": "https://your-tenant.auth0.com",
    "ClientId": "your-auth0-client-id",
    "ClientSecret": "your-auth0-client-secret",
    "Scopes": [ "openid", "profile", "email" ]
  }
}
```

Uncomment authentication in `Program.cs`:

```csharp
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
})
.AddOpenIdConnect(options =>
{
    options.Authority = builder.Configuration["Auth:Authority"];
    options.ClientId = builder.Configuration["Auth:ClientId"];
    options.ClientSecret = builder.Configuration["Auth:ClientSecret"];
    options.ResponseType = "code";
    options.SaveTokens = true;
    options.GetClaimsFromUserInfoEndpoint = true;
    options.Scope.Clear();
    foreach (var scope in builder.Configuration.GetSection("Auth:Scopes").Get<string[]>() ?? Array.Empty<string>())
    {
        options.Scope.Add(scope);
    }
});

// ... later in the file
app.UseAuthentication();
app.UseAuthorization();
```

## Step 3: Configure API (Backend)

Update `appsettings.json`:

```json
{
  "Auth": {
    "Authority": "https://login.microsoftonline.com/{tenant-id}/v2.0",  // Azure AD
    "Audience": "api://your-api-id"
  }
}
```

**For Auth0:**
```json
{
  "Auth": {
    "Authority": "https://your-tenant.auth0.com",
    "Audience": "https://yourproject-api"
  }
}
```

Uncomment authentication in `Program.cs`:

```csharp
using Microsoft.AspNetCore.Authentication.JwtBearer;

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.Authority = builder.Configuration["Auth:Authority"];
    options.Audience = builder.Configuration["Auth:Audience"];
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});

builder.Services.AddAuthorization();

// ... later in the file
app.UseAuthentication();
app.UseAuthorization();
```

## Step 4: Propagate Tokens from Web to API

Configure the HTTP client in Web to send access tokens:

```csharp
using Microsoft.AspNetCore.Authentication;

builder.Services.AddHttpClient("api", client =>
{
    client.BaseAddress = new Uri("https+http://api");
})
.AddHttpMessageHandler<AccessTokenHandler>();

builder.Services.AddTransient<AccessTokenHandler>();

// AccessTokenHandler.cs
public class AccessTokenHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AccessTokenHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var accessToken = await _httpContextAccessor.HttpContext!
            .GetTokenAsync("access_token");

        if (!string.IsNullOrEmpty(accessToken))
        {
            request.Headers.Authorization = 
                new AuthenticationHeaderValue("Bearer", accessToken);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
```

Also register IHttpContextAccessor:

```csharp
builder.Services.AddHttpContextAccessor();
```

## Step 5: Protect Endpoints

### API Controllers

```csharp
[Authorize]  // Require authentication for entire controller
[ApiController]
[Route("api/[controller]")]
public class TodosController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodos()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        // ... return user's todos
    }

    [AllowAnonymous]  // Allow unauthenticated access
    [HttpGet("public")]
    public async Task<ActionResult<IEnumerable<TodoItem>>> GetPublicTodos()
    {
        // ... return public todos
    }
}
```

### Web Controllers

```csharp
public class HomeController : Controller
{
    [Authorize]
    public IActionResult Dashboard()
    {
        var userName = User.Identity?.Name;
        return View();
    }

    [AllowAnonymous]
    public IActionResult Index()
    {
        return View();
    }
}
```

## Step 6: Add Login/Logout UI

Update `_Layout.cshtml`:

```html
<header>
    <nav class="navbar navbar-expand-lg navbar-light bg-light">
        <div class="container">
            <a class="navbar-brand" href="/">YourProject</a>
            <div class="ml-auto">
                @if (User.Identity?.IsAuthenticated == true)
                {
                    <span class="navbar-text">Hello, @User.Identity.Name!</span>
                    <a class="btn btn-outline-secondary" asp-controller="Account" asp-action="Logout">Logout</a>
                }
                else
                {
                    <a class="btn btn-primary" asp-controller="Account" asp-action="Login">Login</a>
                }
            </div>
        </div>
    </nav>
</header>
```

Create `AccountController.cs`:

```csharp
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;

public class AccountController : Controller
{
    [HttpGet]
    public IActionResult Login(string returnUrl = "/")
    {
        return Challenge(new AuthenticationProperties
        {
            RedirectUri = returnUrl
        }, OpenIdConnectDefaults.AuthenticationScheme);
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
        return Redirect("/");
    }
}
```

## Testing Authentication

1. Run the Aspire host: `dotnet run --project YourProject.AppHost`
2. Navigate to the Web application
3. Click **Login** → redirects to identity provider
4. Login with test account
5. Redirected back to Web application, authenticated
6. Web makes authenticated API calls with token

## Claims and Roles

### Access User Claims

```csharp
[HttpGet("profile")]
public IActionResult GetProfile()
{
    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    var email = User.FindFirst(ClaimTypes.Email)?.Value;
    var name = User.FindFirst(ClaimTypes.Name)?.Value;

    return Ok(new { userId, email, name });
}
```

### Role-Based Authorization

**In Azure AD:** Add app roles to manifest, assign users

**In API:**
```csharp
[Authorize(Roles = "Admin")]
[HttpDelete("{id}")]
public async Task<IActionResult> DeleteTodo(int id)
{
    // ... only admins can delete
}
```

### Policy-Based Authorization

```csharp
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanManageTodos", policy =>
        policy.RequireClaim("permissions", "manage:todos"));
});

[Authorize(Policy = "CanManageTodos")]
[HttpPost]
public async Task<IActionResult> CreateTodo(TodoItem todo)
{
    // ...
}
```

## Production Considerations

### Secure Secrets

Use Azure Key Vault, AWS Secrets Manager, or environment variables:

```bash
dotnet user-secrets set "Auth:ClientSecret" "your-secret"
```

### HTTPS Only

Ensure all environments use HTTPS:

```csharp
app.UseHsts();
app.UseHttpsRedirection();
```

### Token Refresh

Tokens expire (typically 1 hour). Implement refresh:

```csharp
.AddOpenIdConnect(options =>
{
    options.SaveTokens = true;
    options.Scope.Add("offline_access");  // Request refresh token
    
    options.Events = new OpenIdConnectEvents
    {
        OnTokenValidated = async context =>
        {
            // Store refresh token
            var refreshToken = context.TokenEndpointResponse!.RefreshToken;
            // ... save to cache/database
        }
    };
});
```

## Troubleshooting

### Redirect Loop

- Check `RedirectUri` matches exactly in identity provider
- Ensure cookies are enabled
- Verify SameSite and Secure cookie policies

### Token Validation Failed

- Confirm `Authority` and `Audience` match identity provider
- Check token hasn't expired
- Verify issuer and audience claims in JWT

### CORS Errors

If Web and API are on different origins, configure CORS in API:

```csharp
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("https://localhost:7001")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

app.UseCors();
```

## Reference

- [ASP.NET Core Authentication](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/)
- [OIDC Spec](https://openid.net/connect/)
- [JWT.io Debugger](https://jwt.io/)
