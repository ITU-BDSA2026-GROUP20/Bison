using System.CommandLine;
using Bison.CLI.models;
using Bison.CLI;
using SimpleDB;

class Program
{
    static int Main(string[] args)
    {
        string path = "./data/bison_observe_cli_db.csv";
        IDatabaseRepository<Reading> csvDatabase = new IDatabaseRepositoryImpl<Reading>(path);

        var readCommand = new Command("read", "Print recorded observations");
        readCommand.SetAction(parseResult =>
        {
            List<Reading> lines = csvDatabase.Read().ToList();
            UserInterface.printOutput(lines);
        });

        var observationArgument = new Argument<string>("observation")
        {
            Description = "The observation to record"
        };

        var observeCommand = new Command("observe", "Record a new observation");
        observeCommand.Arguments.Add(observationArgument);
        observeCommand.SetAction(parseResult =>
        {
            string observation = parseResult.GetValue(observationArgument)
                ?? throw new ArgumentNullException(nameof(observationArgument), "Observation is required");
            
            var reading = new Reading
            (
                Environment.UserName, 
                observation, 
                DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()
            );

            csvDatabase.Store(reading);
            UserInterface.printLog("Observation recorded.");
        });

        var root = new RootCommand("Bison observation tracker")
        {
            Subcommands = { readCommand, observeCommand }
        };

        return root.Parse(args).Invoke();
    }
}