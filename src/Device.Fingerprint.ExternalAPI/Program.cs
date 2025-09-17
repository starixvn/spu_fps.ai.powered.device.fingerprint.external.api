using Device.FingerPrint.Gateway;
using Fanex.Logging;
using Fanex.Logging.Extensions;
using Fanex.Logging.Extensions.AspNetCore;
using Fanex.Logging.Extensions.Database;
using Fanex.Logging.Sentry;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.AddSingleton<ILogger, Logger>();
builder.Services.AddSingleton<IAppSettings, AppSettings>(); 

builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = true,
        ValidAudience = builder.Configuration.GetValue<string>("JWT:Audience"),
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("JWT:Password") ?? ""))
    };
});

builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(builder => builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
});

builder.Services.AddResponseCaching();

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter(policyName: "fixed", options =>
    {
        options.PermitLimit = 4;
        options.Window = TimeSpan.FromSeconds(12);
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        options.QueueLimit = 2;
    });
});

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddHealthChecks();

builder.Services.AddControllers();

builder.Services.AddSwaggerGen(swagger =>
{
    swagger.SwaggerDoc("v1", new OpenApiInfo { Title = "Device.Fingerprint.WebAPI", Version = "v1" });
    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme. Example: \"bearer {token}\"",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
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

var app = builder.Build();

var appSettings = app.Services.GetRequiredService<IAppSettings>();

LogManager
    .SetDefaultLogCategory("Device.Fingerprint.WebAPI")
    .Use(new List<ILogEngine>
    {
            new SentryLogging(new SentryEngineOptions
            {
                Dsn = !string.IsNullOrEmpty(appSettings.SentryDsn) ? appSettings.SentryDsn : null,
                Environment = appSettings.Environment ?? "development",
                Release = builder.Configuration["Sentry:Release"]
            }),
    });

ExceptionHandler
    .UseConfig(new LoggingConfiguration())
    .UseGetDbExceptionInfo(DatabaseInfoGetters.GetDbExceptionInfo);

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Device.Fingerprint.ExternalAPI v1");
});

app.UseForwardedHeaders();

app.UseRouting();

app.UseRateLimiter();

app.UseCors();

app.UseResponseCaching();

app.Use(async (context, next) =>
{
    context.Response.GetTypedHeaders().CacheControl =
        new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
        {
            Public = true,
            MaxAge = TimeSpan.FromMinutes(60)
        };
    context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Vary] =
        new string[] { "Accept-Encoding", "version" };

    await next();
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseHealthChecks("/healthz", new HealthCheckOptions
{
    Predicate = _ => true
});

await LogService.InfoAsync("ExternalAPI Start!");

await app.RunAsync();
