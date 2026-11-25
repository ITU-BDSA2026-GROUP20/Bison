using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using Bison.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Bison.PlaywrightTests;

/// Starts the real Bison.Razor app in its own process, on a free port, with its own fresh SQLite database file.
public sealed class BisonServer : IAsyncDisposable
{
        private readonly Process process;

    public string BaseUrl { get; }
    public string DbPath { get; }

    private BisonServer(Process process, string baseUrl, string dbPath)
    {
        this.process = process;
        BaseUrl = baseUrl;
        DbPath = dbPath;
    }
    public static async Task<BisonServer> StartAsync()
    {
        string projectDir = Path.Combine(FindRepoRoot(), "src", "Bison.Razor");
        string baseUrl = $"http://127.0.0.1:{GetFreePort()}";
        string dbPath = Path.Combine(Path.GetTempPath(), $"bison-e2e-{Guid.NewGuid():N}.db");

        var startInfo = new ProcessStartInfo("dotnet", $"run --project \"{projectDir}\" --no-launch-profile")
        {
            WorkingDirectory = projectDir,
            UseShellExecute = false,
        };
        startInfo.Environment["ASPNETCORE_URLS"] = baseUrl;
        startInfo.Environment["ASPNETCORE_ENVIRONMENT"] = "Development";
        startInfo.Environment["BISONDBPATH"] = dbPath;

        var process = Process.Start(startInfo)
            ?? throw new InvalidOperationException("Could not start Bison.Razor");

        var server = new BisonServer(process, baseUrl, dbPath);
        await server.WaitUntilReadyAsync(TimeSpan.FromMinutes(2));
        return server;
    }
    private async Task WaitUntilReadyAsync(TimeSpan timeout)
    {
        using var http = new HttpClient(new HttpClientHandler { UseProxy = false });
        var deadline = DateTime.UtcNow + timeout;

        while (DateTime.UtcNow < deadline)
        {
            if (process.HasExited)
                throw new InvalidOperationException($"Bison.Razor exited early with code {process.ExitCode}");
            try
            {
                var response = await http.GetAsync(BaseUrl);
                if (response.IsSuccessStatusCode) return;
            }
            catch (HttpRequestException)
            {
            }
            await Task.Delay(500);
        }
        throw new TimeoutException($"Bison.Razor did not answer on {BaseUrl} within {timeout}");
    }
    public Database OpenDatabase()
    {
        var options = new DbContextOptionsBuilder<Database>()
            .UseSqlite($"Data Source={DbPath}")
            .Options;
        return new Database(options);
    }
    public async ValueTask DisposeAsync()
    {
        if (!process.HasExited)
        {
            process.Kill(entireProcessTree: true);
            await process.WaitForExitAsync();
        }
        process.Dispose();
    }
    private static int GetFreePort()
    {
        var listener = new TcpListener(IPAddress.Loopback, 0);
        listener.Start();
        int port = ((IPEndPoint)listener.LocalEndpoint).Port;
        listener.Stop();
        return port;
    }
    private static string FindRepoRoot()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir is not null && !File.Exists(Path.Combine(dir.FullName, "Bison.sln")))
            dir = dir.Parent;
        return dir?.FullName ?? throw new DirectoryNotFoundException("Could not find Bison.sln");
    }
}