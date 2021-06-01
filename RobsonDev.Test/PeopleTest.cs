using Microsoft.AspNetCore.Mvc.Testing;
using Polly;
using RobsonDev.Api;
using RobsonDev.Common;
using RobsonDev.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace RobsonDev.Test
{
    public class PeopleTest : IClassFixture<WebApplicationFactory<Startup>>, IDisposable
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public PeopleTest(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task AuthenticateAndReturnTokenThenRequestListPeoples()
        {
            // Arrange
            var client = _factory.CreateClient();

            var retryPolicy = Policy
                .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .RetryAsync(3, onRetry: (message, retryCount) =>
                {
                    string msg = $"Retentativa: {retryCount}";
                });

            var user = new { Username = "robson", Password = "H+123456" };

            var userJson = JsonSerializer.Serialize(user);
            var content = new StringContent(userJson, Encoding.UTF8, "application/json");

            // Act
            

            var response = await retryPolicy.ExecuteAsync(async () => await client.PostAsync("/api/authenticate/login", content).ConfigureAwait(false));

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            var userViewModel = await response.Content.ReadFromJsonAsync<UserViewModel>();            

            Assert.IsType<UserViewModel>(userViewModel);

            var _token = userViewModel.Token;

            client.Dispose();



            // Act
            var clientPeople = _factory.CreateClient();
            clientPeople.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",$"{_token}");
            var peopleResponse = await retryPolicy.ExecuteAsync(async () => await clientPeople.GetAsync("/api/peoples").ConfigureAwait(false));

            // Assert
            peopleResponse.EnsureSuccessStatusCode(); // Status Code 200-299
            var listPeoples = await peopleResponse.Content.ReadFromJsonAsync<List<People>>();

            Assert.IsType<List<People>>(listPeoples);
            clientPeople.Dispose();
        }

        public void Dispose()
        {
            _factory.Dispose();
        }
    }
}
