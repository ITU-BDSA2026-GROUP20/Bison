using System.Diagnostics;
using System.Runtime.CompilerServices;
using Xunit;

namespace Bison.Tests;

public class BisonBehaviourTests
{
    [Fact]
    public async Task PrintsCorrectErrorOnInvalidCommentId()
    {
        string expected = "Unrecognized id format. Id needs to be a Guid";

        var psi = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = $"run --project \"{GetCliProjectPath()}\" -- discussion invalid-id",
            RedirectStandardOutput = true,
            UseShellExecute = false,
        };

        using var process = Process.Start(psi)!;
        string output = await process.StandardOutput.ReadToEndAsync();
        await process.WaitForExitAsync();

        Assert.Contains(expected, output);
    }

    private static string GetCliProjectPath([CallerFilePath] string here = "")
        => Path.GetFullPath(Path.Combine(Path.GetDirectoryName(here)!, "..", "..", "src", "Bison.CLI", "Bison.CLI.csproj"));
}