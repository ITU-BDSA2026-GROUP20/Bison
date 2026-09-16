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

    private static Process RunWebService()
    {
        string servicePath = Path.Combine(AppContext.BaseDirectory, "Bison.CSVDBService.dll");
        string serverFolder = Path.Combine(TestFolder, "server");
        Directory.CreateDirectory(serverFolder);

        var psi = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = $"\"{servicePath}\" --urls http://localhost:5251",
            WorkingDirectory = serverFolder,
            RedirectStandardOutput = true,
            UseShellExecute = false
        };

        var process = Process.Start(psi)!;
        if (process.WaitForExit(2000))
        Thread.Sleep(3000);
        return process;
    }

    [Fact]
    public void ObserveThenReadAndShowTheObservation()
    {
        Process server = RunWebService();
        try
        {
            RunCli("observe \"Saw a bird near ITU\"");

            string output = RunCli("read");

            Assert.Contains("Saw a bird near ITU", output);

        }
        finally
        {
            if (!server.HasExited)
            {
                server.Kill();
                server.WaitForExit();
            }
            DeleteTestFolder(TestFolder);
        }

    }

    [Fact]
    public void ObserveStoresRecordInDatabase()
    {
        Process server = RunWebService();
        try
        {
            string dbPath = Path.Combine(TestFolder, "data", "bison_reading_cli_db.csv");

            RunCli("observe \"Pidgeon\"");

            Assert.True(File.Exists(dbPath));
            string dbContents = File.ReadAllText(dbPath);
            Assert.Contains("Pidgeon", dbContents);

        }

        finally
        {
            if (!server.HasExited)
            {
                server.Kill();
                server.WaitForExit();
            }

            DeleteTestFolder(TestFolder);
        }


    }

    [Fact]
    public void AllCommandsTested()
    {
        Process server = RunWebService();
        try
        {
            string observeOutput = RunCli("observe \"Sean saw a fox\"");
            Assert.Contains("Observation recorded.", observeOutput);


            string readOutput = RunCli("read");
            Assert.Contains("Sean saw a fox", readOutput);
            string observationId = readOutput.Trim().Split(' ').Last();


            string commentOutput = RunCli($"comment \"Nice find!\" {observationId}");
            Assert.Contains("Comment recorded.", commentOutput);


            string discussionOutput = RunCli($"discussion {observationId}");
            Assert.Contains("Nice find!", discussionOutput);

        }

        finally
        {
            if (!server.HasExited)
            {
                server.Kill();
                server.WaitForExit();
            }
            DeleteTestFolder(TestFolder);
        }

    }

    private static void DeleteTestFolder(string testFolder)
    {
        if (Directory.Exists(testFolder))
            Directory.Delete(testFolder, recursive: true);
    }
}