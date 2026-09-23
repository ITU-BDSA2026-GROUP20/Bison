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

    public static async Task<T?> GetAsync<T>(string endpoint)
    {
        var response = await Instance.GetAsync(endpoint);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<T>();
    }

    // GetAsync("http://xxx/yyy", ("Key1", "Value1"), ("Key2", "Value2"));
    public static async Task<T?> GetAsync<T>(string endpoint, params (string Key, object? Value)[] query)
    {
        var qs = string.Join("&", query.Where(q => q.Value != null).Select(q => $"{Uri.EscapeDataString(q.Key)}={Uri.EscapeDataString(q.Value!.ToString() ?? "")}"));
        var url = qs.Length > 0 ? $"{endpoint}?{qs}" : endpoint;
        var response = await Instance.GetAsync(url);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<T>();
    }

    public static async Task<List<Reading>?> GetPostsByUser(string endpoint, string author)
    {
        var response = await Instance.GetAsync($"{endpoint}?author={author}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<Reading>>();
    }
}