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

    [Fact]
    public async Task PostProposalTest()
    {
        var observation = new Reading("testUser", "Heron at DR Byen", "123456789");
        var response = await client.PostAsJsonAsync("/observation", observation);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var proposal = new Proposal("MSTSNM:Arter:3e4e67e4-f785-ea11-aa77-501ac539d1ea", observation.Id, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString());
        var proposalResponse = await client.PostAsJsonAsync("/proposal", proposal);
        Assert.Equal(HttpStatusCode.OK, proposalResponse.StatusCode);
    }

    [Fact]
    public async Task PostProposalWrongTaxonTest()
    {
        var observation = new Reading("testUser", "Heron at DR Byen", "123456789");
        var response = await client.PostAsJsonAsync("/observation", observation);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var proposal = new Proposal("1234", observation.Id, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString());
        var proposalResponse = await client.PostAsJsonAsync("/proposal", proposal);
        Assert.Equal(HttpStatusCode.BadRequest, proposalResponse.StatusCode);
    }

    [Fact]
    public async Task GetProposalsTest()
    {
        var response = await client.GetAsync("/proposals?guid=3e4e67e4-f785-ea11-aa77-501ac539d1ea");
        var proposals = await response.Content.ReadFromJsonAsync<List<Proposal>>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(proposals);
    }
}