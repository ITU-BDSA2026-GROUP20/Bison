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

    public static async Task<T?> GetAsync<T>(string endpoint, object obj)
    {
        var response = await Instance.GetAsync(endpoint);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<T>();
    }

    public static async Task<T?> PostAsync<T>(string tempEndpoint, object obj)
    {
        var response = await Instance.PostAsJsonAsync(tempEndpoint, obj);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<T>();
    }


}