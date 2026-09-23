using System.Diagnostics;
using System.Security.Cryptography;
using Bogus;
using Xunit;
using Bison.CSVDBService;
using Bison.Tests;
using System.Text.Json;
using System.Net.Http.Json;
using Bison.Core.models;

public class End2EndTest
{
    private static readonly string TestFolder = Path.Combine(Path.GetTempPath(), "bison_e2e_test");
    private const string BaseUrl = "https://localhost:5251";
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };


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
    private const int FuzzLoops = 1000;
    [Fact]
    public async Task FuzzObservations()
    {
        Process server = RunWebService();
        
        int seed = Random.Shared.Next();
        try
        {
            using var client = new HttpClient{BaseAddress = new Uri(BaseUrl)};
        
            var taxonRepository = new TaxonRepository();
            var generator = new RandomDataGenerator(taxonRepository.All.Select(t => t.TaxonId), seed);
            var expectedObservations = new List<Reading>();
            for (int i = 0; i < FuzzLoops; i++)
            {
                var reading = generator.NextReading();
                var response = await client.PostAsJsonAsync("/observation", reading);

                Assert.True(response.IsSuccessStatusCode, $"Expected /observation to succeed but got {response.StatusCode}");
                expectedObservations.Add(reading);
            }
            var actualObservations = await client.GetFromJsonAsync<List<Reading>>("/observations", JsonOptions) ?? new List<Reading>();
            foreach (Reading expected in expectedObservations)
                Assert.Contains(actualObservations, actual => actual.Id == expected.Id && actual.Observation == expected.Observation);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Fuzz test failed at Observations with seed: {seed}");
            Console.WriteLine(e);
            throw;
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
    public async Task FuzzComments()
    {
        Process server = RunWebService();
        
        int seed = Random.Shared.Next();
        Random random = new Random(seed);
        try
        {
            using var client = new HttpClient{BaseAddress = new Uri(BaseUrl)};
        
            var taxonRepository = new TaxonRepository();
            var generator = new RandomDataGenerator(taxonRepository.All.Select(t => t.TaxonId), seed);
            
            var reading = generator.NextReading();
            var response = await client.PostAsJsonAsync("/observation", reading);
            Assert.True(response.IsSuccessStatusCode, $"Expected /observation to succeed but got {response.StatusCode}");
            
            Guid targetId = reading.Id;
            List<string> validComments = new();
            for (int i = 0; i < FuzzLoops; i++)
            {
                // approx 1/10 chance to make a invalid ID and approx 90% to make a valid
                bool validID = random.NextDouble() > 0.1;

                var comment = generator.NextComment(targetId, validID);
                response = await client.PostAsJsonAsync("/comment" , comment);

                if (validID)
                {
                    Assert.True(response.IsSuccessStatusCode, $"Expected /comment to succeed but got {response.StatusCode}");
                    validComments.Add(comment.CommentVal);
                } else
                    Assert.False(response.IsSuccessStatusCode, "Expected /comment to fail for an invalid obersvation id");
            }
            var comments = await client.GetFromJsonAsync<List<Comment>>($"/comments?guid={targetId}", JsonOptions) ?? new List<Comment>();
            var commentValues = comments.Select(c => c.CommentVal).ToList(); 
            foreach (string comment in validComments)
                Assert.Contains(comment, commentValues);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Fuzz test failed at Comments with seed: {seed}");
            Console.WriteLine(e);
            throw;
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
    public async Task FuzzProposals()
    {
        Process server = RunWebService();

        int seed = Random.Shared.Next();
        try
        {
           using var client = new HttpClient{BaseAddress = new Uri(BaseUrl)};

           var taxonRepository = new TaxonRepository();
           var generator = new RandomDataGenerator(taxonRepository.All.Select(t => t.TaxonId), seed);

           const int observationCount = 50;
           var knownObservationIds = new List<Guid>();
           for (int i = 0; i < observationCount; i++)
            {
                var reading = generator.NextReading();
                var seedResponse = await client.PostAsJsonAsync("/observation", reading);
                Assert.True(seedResponse.IsSuccessStatusCode,
                    $"Seeding failed: /observation returned {seedResponse.StatusCode}");
                knownObservationIds.Add(reading.Id);   
            }

            var expectedProposalsByObservations = knownObservationIds.ToDictionary(id => id, _ => new List<String>());
            for ( int i = 0; i < FuzzLoops; i++)
            {
                var proposal = generator.NextProposal(knownObservationIds);
                var response = await client.PostAsJsonAsync("/proposal", proposal);

                bool observationValid = knownObservationIds.Contains(proposal.Id);
                bool taxonValid = taxonRepository.GetByID(proposal.TaxonID) is not null;
                bool expectedValid = observationValid && taxonValid;

                Assert.Equal(expectedValid, response.IsSuccessStatusCode);
                if(expectedValid)
                expectedProposalsByObservations[proposal.Id].Add(proposal.TaxonID);
            }
            foreach(var id in knownObservationIds)
            {
                var proposals = await client.GetFromJsonAsync<List<Proposal>>( $"/proposals?guid={id}", JsonOptions) ?? new List<Proposal>();

                var actual = proposals.Select(p => p.TaxonID).OrderBy(v => v).ToList();
                var expected = expectedProposalsByObservations[id].OrderBy(v => v).ToList();
                Assert.Equal(expected, actual);
            }  
        }
        catch (Exception e)
        {
            Console.WriteLine($"FuzzProposals failed. Seed: {seed}");
            Console.WriteLine(e);
            throw;
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