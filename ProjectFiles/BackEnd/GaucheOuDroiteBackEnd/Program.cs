// For data base
using Microsoft.EntityFrameworkCore;

// For controllers
using GaucheOuDroiteBackEnd.Data;
using GaucheOuDroiteBackEnd.Security;
using GaucheOuDroiteBackEnd.Services;

// For authentication token (JWT)
using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

using GaucheOuDroiteBackEnd.Models;

// For request rate limitation
using System.Threading.RateLimiting;

using Shared.DTOs;
using Shared.Tools;


const string SCRIPT_NAME = "Program.cs";


var builder = WebApplication.CreateBuilder(args);

// --- Project includes

// Adding services to the container (project).

// Project controllers
builder.Services.AddControllers();

// Project data base
string? connectionString = builder.Configuration.GetConnectionString("DataBaseContext") ?? throw new InvalidOperationException($"ERROR: [{SCRIPT_NAME}] Connection string 'DataBaseContext' not found.");

builder.Services.AddDbContext<DataBaseContext>(options => options.UseSqlite(connectionString));

// Project services
builder.Services.AddScoped<AuthenticationService>();

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<UserProgressionService>();

builder.Services.AddScoped<LevelService>();
builder.Services.AddScoped<LevelResponseTimeStepService>();

// Project security
builder.Services.AddScoped<PasswordHasher>();
builder.Services.AddScoped<JwtTokenService>();

// Fills the JwtTokenSettings with the data inside the 'appsettings.json'.JwtSettings
builder.Services.Configure<JwtTokenSettings>(
    builder.Configuration.GetSection("JwtSettings")
);

JwtTokenSettings jwtTokenSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtTokenSettings>()
    ?? throw new InvalidOperationException($"ERROR: [{SCRIPT_NAME}] JwtSettings not found inside 'appsetting.json' or 'secret.json' files.");

if (string.IsNullOrEmpty(jwtTokenSettings.Key))
    throw new NullReferenceException($"ERROR: [{SCRIPT_NAME}] The JwtSettings.Key inside 'appsetting.json' or 'secret.json' files is null or empty. Please verify that you added the secret JwtSettings Key. Check out the 'appsettings.json' file for more information.");


builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        // Keep the claim names exactly as they appear in the token (no surprise remapping).
        options.MapInboundClaims = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtTokenSettings.Issuer,
            ValidAudience = jwtTokenSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtTokenSettings.Key)),
            ClockSkew = TimeSpan.Zero,
            NameClaimType = ClaimTypes.NameIdentifier,
        };
    });

builder.Services.AddAuthentication();


builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
    {
        return RateLimitPartition.GetFixedWindowLimiter(

            // We use the IP address to detect who is sending the request
            //
            // Note:
            // Creating partitions on client IP addresses makes the app vulnerable to Denial of Service Attacks which employ IP Source Address Spoofing.
            // For more information, see BCP 38 RFC 2827 Network Ingress Filtering: Defeating Denial of Service Attacks which employ IP Source Address Spoofing.
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",

            
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 5,
                Window = TimeSpan.FromSeconds(10),

                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 0
            }
        );
    });

    // Creating a custom rejection handling logic
    options.OnRejected = async (context, cancellationToken) =>
    {
        Console.WriteLine($"WARNING: [{SCRIPT_NAME}] Rate limit exceeded for IP: {context.HttpContext.Connection.RemoteIpAddress}");

        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        context.HttpContext.Response.Headers.RetryAfter = "10";

        ApiResponseDTO apiResponseDTO = new()
        {
            HasSucceeded = false,
            ErrorMessage = "Rate limit exceeded. Please try again a little later."
        };

        await context.HttpContext.Response.WriteAsync(ObjectToStringFormatter.ObjectToString(apiResponseDTO), cancellationToken);
    };
});


// Development tools
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen();

// ---

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwagger();
    app.UseSwaggerUI();
}

// For request rate limitation
// Note: If the rate limiter is using authentication token to work be sure to put 'app.UseRateLimiter();' after 'builder.Services.AddAuthentication();' and 'WebApplication app = builder.Build();'.
app.UseRateLimiter();


app.UseHttpsRedirection();

app.UseAuthorization();


app.MapControllers();

app.Run();