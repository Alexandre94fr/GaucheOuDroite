using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

using GaucheOuDroiteBackEnd.Data;
using GaucheOuDroiteBackEnd.Services;
using GaucheOuDroiteBackEndTests.Tools;


namespace GaucheOuDroiteBackEndTests.API
{
    [TestClass]
    public sealed class AuthorizationTests
    {
        TestWebApplicationFactory _applicationFactory = null!;

        HttpClient _client = null!;


        const int EXAMPLE_USER_ID = 1;
        const string EXAMPLE_USERNAME = "TestUser";


        const string GAME_DATA_URL = "api/game-data";
        const string USERS_URL = "api/users";
        const string USER_PROGRESSION_URL = "api/user-progressions";


        // -- Setup -- //

        [TestInitialize]
        public async Task TestInit()
        {
            _applicationFactory = new TestWebApplicationFactory();

            _client = _applicationFactory.CreateClient(new WebApplicationFactoryClientOptions
            {
                // We want to directly observe the HTTP status code
                // returned by the API.
                AllowAutoRedirect = false
            });


            // Creating the SQLite database used by the test server.
            using IServiceScope serviceScope = _applicationFactory.Services.CreateScope();

            DataBaseContext dataBaseContext = serviceScope.ServiceProvider.GetRequiredService<DataBaseContext>();

            await dataBaseContext.Database.EnsureCreatedAsync();
        }


        [TestCleanup]
        public void TestCleanup()
        {
            _client.Dispose();
            _applicationFactory.Dispose();
        }


        // -- Helping methods -- //

        string CreateValidJwtToken()
        {
            using IServiceScope serviceScope = _applicationFactory.Services.CreateScope();

            JwtTokenService jwtTokenService = serviceScope.ServiceProvider.GetRequiredService<JwtTokenService>();

            (string Token, DateTime ExpiresAt) token = jwtTokenService.CreateToken(
                EXAMPLE_USER_ID,
                EXAMPLE_USERNAME
            );

            return token.Token;
        }


        void SetBearerToken(string p_token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                p_token
            );
        }


        async Task<HttpResponseMessage> SendRequestAsync(string p_httpMethod, string p_endPoint)
        {
            HttpRequestMessage request = new(
                new HttpMethod(p_httpMethod),
                p_endPoint
            );

            if (p_httpMethod == HttpMethod.Put.Method)
            {
                request.Content = JsonContent.Create(new
                {
                    LevelProgressions = Array.Empty<object>()
                });
            }

            return await _client.SendAsync(request);
        }


        // -- Tests -- //
        
        [DataRow("GET", GAME_DATA_URL)]

        [DataRow("GET", USERS_URL)]
        [DataRow("DELETE", USERS_URL)]

        [DataRow("GET", USER_PROGRESSION_URL)]
        [DataRow("PUT", USER_PROGRESSION_URL)]
        [TestMethod]
        public async Task EndPoint_WithoutAuthentication_ReturnsUnauthorized(string p_httpMethod, string p_endPoint)
        {
            HttpResponseMessage httpResponseMessage = await SendRequestAsync(
                p_httpMethod,
                p_endPoint
            );


            Assert.AreEqual(
                HttpStatusCode.Unauthorized,
                httpResponseMessage.StatusCode
            );
        }


        [DataRow("GET", GAME_DATA_URL)]

        [DataRow("GET", USERS_URL)]
        [DataRow("DELETE", USERS_URL)]

        [DataRow("GET", USER_PROGRESSION_URL)]
        [DataRow("PUT", USER_PROGRESSION_URL)]
        [TestMethod]
        public async Task EndPoint_WithValidJwt_PassesAuthentication(string p_httpMethod, string p_endPoint)
        {
            string token = CreateValidJwtToken();

            SetBearerToken(token);


            HttpResponseMessage httpResponseMessage = await SendRequestAsync(
                p_httpMethod,
                p_endPoint
            );


            Assert.AreNotEqual(
                HttpStatusCode.Unauthorized,
                httpResponseMessage.StatusCode
            );
        }
    }
}