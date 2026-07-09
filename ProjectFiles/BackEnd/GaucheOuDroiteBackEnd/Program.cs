using Microsoft.EntityFrameworkCore;

// For Token
using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

using GaucheOuDroiteBackEnd.Data;
using GaucheOuDroiteBackEnd.Models;
using GaucheOuDroiteBackEnd.Security;
using GaucheOuDroiteBackEnd.Services;


var builder = WebApplication.CreateBuilder(args);

// --- Project includes

// Adding services to the container (project).

// Project controllers
builder.Services.AddControllers();

// Project data base
string? connectionString = builder.Configuration.GetConnectionString("DataBaseContext") ?? throw new InvalidOperationException("ERROR: [Program.cs] Connection string 'DataBaseContext' not found.");

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
    ?? throw new InvalidOperationException($"ERROR: [Program.cs] JwtSettings not found inside 'appsetting.json'.");


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

// Development tools
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen();

// ---

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();