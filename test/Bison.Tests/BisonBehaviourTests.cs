using System.Diagnostics;
using System.Runtime.CompilerServices;
using Xunit;
using Bison.CLI.utils;

namespace Bison.Tests;

public class BisonBehaviourTests
{

    private static string RunCli(string args)
        {
            
            var psi = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"\"{typeof(ICliCommand).Assembly.Location}\" {args}",
                RedirectStandardOutput = true,
                UseShellExecute = false
            };

            using var process = Process.Start(psi)!;
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return output;
        }

    [Fact]
    public async Task DiscussionCommandPrintsCorrectErrorOnInvalidCommentId()
    {
        string expected = "Unrecognized id format. Id needs to be a Guid";

        string output = RunCli("discussion invalid-id");

        Assert.Contains(expected, output);
    }

    [Fact]
    public void CommentCommandPrintsCorrectErrorOnInvalidCommentId()
    {
        string expected = "Unrecognized id format. Id needs to be a Guid";

        string output = RunCli("comment \"comment\" invalid-id");

        Console.WriteLine(output);

        Assert.Contains(expected, output);
    }

    [Fact]
    public void FromUnixSecondsString_ConvertsTimestamp()
    {
        long input = 1609459200; // 2021-01-01 at 00:00:00
        DateTime expected = new DateTime(2021,1,1,0,0,0, DateTimeKind.Utc);

        DateTime actual = TimestampConverter.FromUnixSeconds(input.ToString());

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void FromUnixSecondsLong_ConvertsTimestamp()
    {
        long input = 1609459200; // 2021-01-01 at 00:00:00
        DateTime expected = new DateTime(2021,1,1,0,0,0, DateTimeKind.Utc);

        DateTime actual = TimestampConverter.FromUnixSecondsLong(input);

        Assert.Equal(expected, actual);
    }

}