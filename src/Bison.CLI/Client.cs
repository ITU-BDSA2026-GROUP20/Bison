using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Bison.Core.models;

namespace Bison.CLI;

public static class Client
{
    private const string BaseUrl = "http://localhost:5251/";

    public static HttpClient Instance { get; } = BuildClient();

    private static HttpClient BuildClient()
    {
        var client = new HttpClient { BaseAddress = new Uri(BaseUrl) };
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
        return client;
    }

    public static async Task<T?> GetAsync<T>(string endpoint)
    {
        var response = await Instance.GetAsync(endpoint);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<T>();
    }

    // Needs to be rewritten in the future to use params object[] and then wrap them into the query 
    // automatically using the type of the object to determine the name of the query parameter. 
    // For now, this is a quick and dirty solution to get the job done.
    public static async Task<T?> GetAsync<T>(string endpoint, Guid id)
    {
        var response = await Instance.GetAsync($"{endpoint}?guid={id}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<T>();
    }

    public static async Task<T?> PostAsync<T>(string endpoint, object obj)
    {
        var response = await Instance.PostAsJsonAsync(endpoint, obj);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<T>();
    }

    public static async Task<bool> PostAsync(string endpoint, object obj)
    {
        var response = await Instance.PostAsJsonAsync(endpoint, obj);
        return response.StatusCode == HttpStatusCode.OK;
    }
}