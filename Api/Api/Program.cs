using App.Di;
using DotNetApiLogging.Di;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration.AddEnvironmentVariables();

builder.AddAppDependencies();

var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();

builder.Services.AddCors(options => {
    options.AddPolicy("cors", policy => policy
    .WithOrigins(allowedOrigins)
    .SetIsOriginAllowedToAllowWildcardSubdomains()
    .WithMethods("GET", "POST", "PUT", "DELETE", "OPTIONS")
    .WithHeaders("Content-Type", "Authorization", "X-Requested-With")
    .AllowCredentials()
    .SetPreflightMaxAge(TimeSpan.FromMinutes(10)));
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseCors("cors");
app.MapControllers();
app.AddWebLoggingDependencies();
app.Run();
