using System.CommandLine;
using Bison.CLI.models;
using Bison.CLI;

class Program
{
    static int Main(string[] args)
    {
        string path = "./data/bison_observe_cli_db.csv";
        CsvHandler handler = new CsvHandler();

        var readCommand = new Command("read", "Print recorded observations");
        readCommand.SetAction(parseResult =>
        {
            List<Reading> lines = handler.getFileData(path);
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
                                 ?? throw new ArgumentNullException(nameof(observationArgument),
                                     "Observation is required");
            
            handler.handleObservation(observation, path);
            UserInterface.printLog("Observation recorded.");
        });

        var root = new RootCommand("Bison observation tracker")
        {
            Subcommands = { readCommand, observeCommand }
        };

        return root.Parse(args).Invoke();
    }
}