using System.Diagnostics;
using System.Runtime.CompilerServices;
using Xunit;
using Bison.CLI.utils;

namespace Bison.Tests;

public class BisonBehaviourTests
{
    [Fact]
    public async Task DiscussionCommandPrintsCorrectErrorOnInvalidCommentId()
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

    [Fact]
    public async Task CommentCommandPrintsCorrectErrorOnInvalidCommentId()
    {
        string expected = "Unrecognized id format. Id needs to be a Guid";

        var psi = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = $"run --project \"{GetCliProjectPath()}\" -- comment \"example message\" invalid-id",
            RedirectStandardOutput = true,
            UseShellExecute = false,
        };

        using var process = Process.Start(psi)!;
        string output = await process.StandardOutput.ReadToEndAsync();
        await process.WaitForExitAsync();

        Assert.Contains(expected, output);
    }

    [Fact]
    public void FromUnixSeconds_ConvertsTimestamp()
    {
        long input = 1609459200; // 2021-01-01 at 00:00:00
        DateTime expected = new DateTime(2021,1,1,0,0,0, DateTimeKind.Utc);

        DateTime actual = TimestampConverter.FromUnixSeconds(input.ToString());

        Assert.Equal(expected, actual);
    }

    private static string GetCliProjectPath([CallerFilePath] string here = "")
        => Path.GetFullPath(Path.Combine(Path.GetDirectoryName(here)!, "..", "..", "src", "Bison.CLI", "Bison.CLI.csproj"));
}