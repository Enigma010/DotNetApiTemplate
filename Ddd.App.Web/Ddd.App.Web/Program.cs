using Ddd.App.Authentication;
using Ddd.App.Core;
using Ddd.App.Di;
using Ddd.App.Web.Components;
using Ddd.App.Web.Components.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents()
    .AddAuthenticationStateSerialization();

builder.Services.AddCascadingAuthenticationState();

builder.Configuration.AddJsonFile("appsettings.json");
builder.Configuration.AddJsonFile("appsettings.Development.json", optional: true);
builder.Configuration.AddEnvironmentVariables();
string envConfigPath = builder.Configuration.GetValue<string?>("EnvConfigPath") ?? string.Empty;
if (!string.IsNullOrEmpty(envConfigPath))
{
    EnvConfig envConfig = new EnvConfig(envConfigPath);
    envConfig.SetEnvironmentVariables();
}
builder.AddAppDependencies();
builder.Services.Configure<Authentication>(
    builder.Configuration.GetSection(nameof(Authentication))
);
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
Authentication authentication = builder.Configuration.GetSection(nameof(Authentication)).Get<Authentication>()
    ?? throw new InvalidOperationException("Authentication configuration section is missing.");

builder.Services
    .AddAuthentication(options =>
    {
        // Use cookies for local session state
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;

        // Redirect unauthenticated users to authentik
        options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
    })
    .AddCookie(options =>
    {
        options.Cookie.Name = authentication.CookieName;
    })
    .AddOpenIdConnect(options =>
    {
        // Your authentik application/provider URL
        options.Authority = authentication.Authority; //"https://auth.example.com/application/o/myapp/";

        // From authentik provider settings
        options.ClientId = authentication.ClientId;
        options.ClientSecret = authentication.ClientSecret(builder.Configuration);

        options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

        // Standard Authorization Code flow
        options.ResponseType = OpenIdConnectResponseType.Code;

        // Save tokens in auth session
        options.SaveTokens = true;

        // HTTPS required in production
        options.RequireHttpsMetadata = true;

        // Pull additional claims from userinfo endpoint
        options.GetClaimsFromUserInfoEndpoint = true;

        options.SignedOutCallbackPath = "/signout-callback-oidc";

        options.MapInboundClaims = false;
        options.TokenValidationParameters.NameClaimType = JwtRegisteredClaimNames.Name;
        options.TokenValidationParameters.RoleClaimType = "roles";

        // Requested scopes
        options.Scope.Clear();
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("email");
        
        // Optional: map name/role claims
        options.TokenValidationParameters.NameClaimType = "preferred_username";
        options.TokenValidationParameters.RoleClaimType = "groups";

        // Optional: debug events
        options.Events = new OpenIdConnectEvents
        {
            OnTokenValidated = context =>
            {
                Console.WriteLine("User authenticated");

                return Task.CompletedTask;
            },
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine(context.Exception);

                return Task.CompletedTask;
            },
            OnRedirectToIdentityProviderForSignOut = context =>
            {
                Console.WriteLine($"Logout URL: {context.ProtocolMessage.IssuerAddress}");
                Console.WriteLine($"IdTokenHint: {context.ProtocolMessage.IdTokenHint}");
                context.ProtocolMessage.Prompt = "login";
                return Task.CompletedTask;
            }
        };

        options.RequireHttpsMetadata = false;
    });
builder.Services.AddAuthorization();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Ddd.App.Web.Client._Imports).Assembly);

app.UseRouting();
app.UseAntiforgery();
// Add additional endpoints required by the Identity /Account Razor components.
app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/logout", async context =>
{
    await context.SignOutAsync(
        CookieAuthenticationDefaults.AuthenticationScheme);

    await context.SignOutAsync(
        OpenIdConnectDefaults.AuthenticationScheme,
        new AuthenticationProperties
        {
            RedirectUri = "/"
        });
});

app.MapGet("/api/token", async (HttpContext context) =>
{
    var token = await context.GetTokenAsync("access_token");

    return Results.Ok(new { token });
}).RequireAuthorization();

app.Run();
