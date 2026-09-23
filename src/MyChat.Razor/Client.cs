using System.Net;
using System.Net.Http.Headers;
using Bison.Core.models;

namespace MyChat.Razor;

public static class Client
{
    private const string BaseUrl = "http://localhost:5000/";

    public static HttpClient Instance { get; } = BuildClient();

    private static HttpClient BuildClient()
    {
        var client = new HttpClient { BaseAddress = new Uri(BaseUrl) };
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
        return client;
    }

    public static async Task<T?> GetAsync<T>(string endpoint, params (string Key, object? Value)[] query)
    {
        var response = await Instance.GetAsync($"{endpoint}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<T>();
    }

    
    // Needs to be rewritten in the future to use params object[] and then wrap them into the query 
    // automatically using the type of the object to determine the name of the query parameter. 
    // For now, this is a quick and dirty solution to get the job done.

    //Methods
    public static async Task<T?> GetAsync<T>(string endpoint)
    {
        var response = await Instance.GetAsync(endpoint);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<T>();
    }

    public static async Task<List<Reading>?> GetPostsByUser(string endpoint, string author)
    {
        var response = await Instance.GetAsync($"{endpoint}?author={author}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<Reading>>();
    }

    public static async Task<T?> GetAsync<T>(string endpoint, Guid id)
    {
        var response = await Instance.GetAsync($"{endpoint}?guid={id}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<T>();
    }
}