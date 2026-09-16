namespace Bison.CSVDBService.Tests;

using Bison.Core.models;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;

public class TestAPI : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> fixture;
    private readonly HttpClient client;

    public TestAPI(WebApplicationFactory<Program> fixture)
    {
        this.fixture = fixture;
        client = fixture.CreateClient();
    }

    [Fact]
    public async Task GetObservationTest()
    {
        
        var response = await client.GetAsync("/observations");

        var observations = await response.Content.ReadFromJsonAsync<List<Reading>>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(observations);
    }

    [Fact]
    public async Task PostObservationTest()
    {
        var observation = new Reading("testUser", "Heron at DR Byen", "123456789");

        var response = await client.PostAsJsonAsync("/observation", observation);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    }
    
}