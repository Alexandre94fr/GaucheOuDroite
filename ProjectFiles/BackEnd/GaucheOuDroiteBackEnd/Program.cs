using Microsoft.EntityFrameworkCore;

using GaucheOuDroiteBackEnd.Data;
using GaucheOuDroiteBackEnd.Security;
using GaucheOuDroiteBackEnd.Services;


var builder = WebApplication.CreateBuilder(args);

// --- Project includes

// Adding services to the container (project).

// Project controllers
builder.Services.AddControllers();

// Project data base
string? connectionString = builder.Configuration.GetConnectionString("DataBaseContext") ?? throw new InvalidOperationException("Connection string 'DataBaseContext' not found.");

builder.Services.AddDbContext<DataBaseContext>(options => options.UseSqlite(connectionString));

// Project services
builder.Services.AddScoped<AuthenticationService>();

builder.Services.AddScoped<UserService>();
//builder.Services.AddScoped<LevelService>(); // TODO
builder.Services.AddScoped<UserProgressionService>();

// Project security
builder.Services.AddScoped<PasswordHasher>();

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

app.UseAuthorization();

app.MapControllers();

app.Run();