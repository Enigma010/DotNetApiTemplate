using Ddd.App.Core;
using Ddd.App.Core.Services;
using Ddd.App.Di;
using Ddd.App.Web.Components;
using Ddd.App.Web.Components.Account;
using Ddd.App.Web.Entities;
using Ddd.App.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Options;
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

builder.AddAppDependencies();
builder.Services.Configure<WebAuthenticationService>(
    builder.Configuration.GetSection(nameof(WebAuthenticationService))
);
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.Configure<WebAuthentication>(builder.Configuration.GetSection(WebAuthentication.SectionName));

var serviceProvider = builder.Services.BuildServiceProvider();
WebAuthenticationService webAuthenticationService = new WebAuthenticationService(builder.Configuration,
    serviceProvider.GetRequiredService<IOptions<WebAuthentication>>(),
    serviceProvider.GetRequiredService<IAppService>());

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
        options.Cookie.Name = webAuthenticationService.CookieName;
    })
    .AddOpenIdConnect(options =>
    {
        // Your authentik application/provider URL
        options.Authority = webAuthenticationService.Authority;

        // From authentik provider settings
        options.ClientId = webAuthenticationService.Authentication.ClientId;
        options.ClientSecret = webAuthenticationService.ClientSecret;

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
                return Task.CompletedTask;
            },
            OnAuthenticationFailed = context =>
            {
                return Task.CompletedTask;
            },
            OnRedirectToIdentityProviderForSignOut = context =>
            {
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

app.MapPost("account/logout", async context =>
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

app.MapPost("account/login", async (HttpContext context) =>
{
    // Challenges the OIDC middleware to redirect the user to the Identity Provider
    await context.ChallengeAsync(OpenIdConnectDefaults.AuthenticationScheme, new AuthenticationProperties
    {
        RedirectUri = "https://localhost:7035/"
    });
});

app.MapGet("/api/token", async (HttpContext context) =>
{
    var token = await context.GetTokenAsync("access_token");

    return Results.Ok(new { token });
}).RequireAuthorization();
app.Run();
