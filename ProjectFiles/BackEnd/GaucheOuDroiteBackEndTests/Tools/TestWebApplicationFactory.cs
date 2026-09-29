using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;

using GaucheOuDroiteBackEnd.Data;


namespace GaucheOuDroiteBackEndTests.Tools
{
    public sealed class TestWebApplicationFactory : WebApplicationFactory<Program>
    {
        const string TEST_JWT_KEY = "TestKey_ThisIsOnlyUsedForAutomatedTests_123456789";
        const string TEST_JWT_ISSUER = "GaucheOuDroiteTests";
        const string TEST_JWT_AUDIENCE = "GaucheOuDroiteTests";
        const int TEST_JWT_EXPIRY_MINUTES = 60;


        protected override void ConfigureWebHost(IWebHostBuilder p_builder)
        {
            p_builder.ConfigureAppConfiguration((context, configurationBuilder) =>
            {
                Dictionary<string, string?> configurationValues = new()
                {
                    // DataBase
                    ["ConnectionStrings:DataBaseContext"] = "Data Source=:memory:",

                    // JWT
                    ["JwtSettings:Key"] = TEST_JWT_KEY,
                    ["JwtSettings:Issuer"] = TEST_JWT_ISSUER,
                    ["JwtSettings:Audience"] = TEST_JWT_AUDIENCE,
                    ["JwtSettings:ExpiryMinutes"] = TEST_JWT_EXPIRY_MINUTES.ToString()
                };

                configurationBuilder.AddInMemoryCollection(configurationValues);

            });

            p_builder.ConfigureServices(services =>
            {
                // Removing the DataBaseContext registered by Program.cs.
                services.RemoveAll<DbContextOptions<DataBaseContext>>();


                // Creating one SQLite in-memory sqliteConnection for the complete lifetime of the test application.
                services.AddSingleton<SqliteConnection>(_ =>
                {
                    SqliteConnection sqliteConnection = new(
                        "Data Source=:memory:"
                    );

                    sqliteConnection.Open();

                    return sqliteConnection;
                });


                // Making DataBaseContext use the shared SQLite connection.
                services.AddDbContext<DataBaseContext>((serviceProvider, options) =>
                {
                    SqliteConnection connection =
                        serviceProvider.GetRequiredService<SqliteConnection>();

                    options.UseSqlite(connection);
                });

                // Forcing the application to use the test configurations
                // PostConfigure() is called after the application has been configured.
                // We do this to avoid the test application to use the development JWT key.
                services.PostConfigure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters.ValidIssuer = TEST_JWT_ISSUER;
                    options.TokenValidationParameters.ValidAudience = TEST_JWT_AUDIENCE;
                    options.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TEST_JWT_KEY));
                });

            });

        }
    }
}