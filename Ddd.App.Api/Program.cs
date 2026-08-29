
using Ddd.App.Api.Services;
using Ddd.App.Core.Services;
using Ddd.App.Di;
using Ddd.App.Web.Entities;
using DotNetApiLogging.Di;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Configuration.AddJsonFile("appsettings.json");
builder.Configuration.AddJsonFile("appsettings.Development.json", optional: true);
builder.AddAppDependencies();
builder.Services.Configure<WebAuthenticationService>(
    builder.Configuration.GetSection(nameof(WebAuthenticationService))
);
builder.Services.Configure<WebAuthentication>(builder.Configuration.GetSection(WebAuthentication.SectionName));
var serviceProvider = builder.Services.BuildServiceProvider();
WebAuthenticationService webAuthenticationService = new WebAuthenticationService(builder.Configuration,
    serviceProvider.GetRequiredService<IOptions<WebAuthentication>>(),
    serviceProvider.GetRequiredService<IAppService>());
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // Keycloak realm discovery document URL (no /auth/ prefix in Keycloak 17+)
        options.Authority = webAuthenticationService.Authority;

        // Must match the 'aud' claim in tokens issued to this client.
        // For Keycloak, the audience is the client_id unless you add a custom audience mapper.
        options.Audience = webAuthenticationService.Authentication.ClientId;

        if (builder.Environment.IsDevelopment())
        {
            options.RequireHttpsMetadata = false; // set false only in local dev
        }
        else
        {
            options.RequireHttpsMetadata = true; // set false only in local dev
        }

        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = webAuthenticationService.Authority,
            ValidateAudience = true,
            ValidAudience = webAuthenticationService.Authentication.ValidAudience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromSeconds(30),
        };

        // Map Keycloak roles into .NET ClaimsPrincipal roles (see section below)
        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = ctx =>
            {
                ctx.Principal = KeycloakClaimsTransformer.Transform(ctx.Principal!);
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter: Bearer {your JWT token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("cors", policy => policy
    .WithOrigins(allowedOrigins)
    .SetIsOriginAllowedToAllowWildcardSubdomains()
    .WithMethods("GET", "POST", "PUT", "DELETE", "OPTIONS")
    .WithHeaders("Content-Type", "Authorization", "X-Requested-With")
    .AllowCredentials()
    .SetPreflightMaxAge(TimeSpan.FromMinutes(10)));
});

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("cors");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.AddWebLoggingDependencies();
app.Run();
