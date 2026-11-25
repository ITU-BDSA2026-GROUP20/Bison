using System.Net;
using System.Net.Sockets;

namespace Bison.PlaywrightTests;

/// Starts the real Bison.Razor app in its own process, on a free port, with its own fresh SQLite database file.
public sealed class BisonServer
{
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