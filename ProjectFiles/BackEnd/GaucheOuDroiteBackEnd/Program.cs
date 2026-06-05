using Microsoft.EntityFrameworkCore;

using GaucheOuDroiteBackEnd.Data;
using GaucheOuDroiteBackEnd.Services;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DataBaseContext") ?? throw new InvalidOperationException("Connection string 'DataBaseContext' not found.");

builder.Services.AddDbContext<DataBaseContext>(options => options.UseSqlite(connectionString));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<AuthenticationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();