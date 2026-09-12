using System.Diagnostics;
using Xunit;

public class End2EndTest
{
    private static readonly string TestFolder = Path.Combine(Path.GetTempPath(), "bison_e2e_test");
   
    private static string RunCli(string args)
    {
        Directory.CreateDirectory(Path.Combine(TestFolder, "data"));
        var psi = new ProcessStartInfo
        {
             FileName = "dotnet",
            Arguments = $"\"{typeof(ICliCommand).Assembly.Location}\" {args}",
            WorkingDirectory = TestFolder,
            RedirectStandardOutput = true,
            UseShellExecute = false
        };

        using var process = Process.Start(psi)!;
        string output = process.StandardOutput.ReadToEnd();
        process.WaitForExit();
        return output;
    }

    [Fact]
    public void ObserveThenReadAndShowTheObservation()
    {
        RunCli("observe \"Saw a bird near ITU\"");

        string output = RunCli("read");

        Assert.Contains("Saw a bird near ITU", output);
    }

    [Fact]
    public void ObserverePigdeonStoresRecordInDatabase()
    {
        string dbPath = Path.Combine(TestFolder, "data", "bison_observe_cli_db.csv");

        RunCli("observe \"Pigdeon\"");

        Assert.True(File.Exists(dbPath));
        string dbContents = File.ReadAllText(dbPath);
        Assert.Contains("Pigdeon", dbContents);
    }
}