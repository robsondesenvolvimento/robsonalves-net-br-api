using Microsoft.AspNetCore.Mvc.Testing;
using System.Text.Json;
using Xunit;
using RobsonDev.Api;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using System;

namespace RobsonDev.Test
{
    public class AuthenticateTest : IClassFixture<WebApplicationFactory<Startup>>, IDisposable
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public AuthenticateTest(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("api/authenticate/login")]
        public async Task AuthenticateAndReturnToken(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            var user = new { Username = "robson", Password = "H+123456" };

            var userJson = JsonSerializer.Serialize(user);
            var content = new StringContent(userJson, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync(url, content).ConfigureAwait(false);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());

            client.Dispose();
        }

        public void Dispose()
        {
            _factory.Dispose();
        }
    }
}
