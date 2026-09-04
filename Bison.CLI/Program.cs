using System.CommandLine;
using Bison.CLI.models;
using Bison.CLI;

class Program
{
    static int Main(string[] args)
    {
        inputParser parser = new inputParser();

        var readCommand = new Command("read", "Print recorded observations");
        readCommand.SetAction(parseResult =>
        {
            List<Reading> lines = parser.getFileData("./Bison.CLI/data/bison_observe_cli_db.csv");
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
            handleObservation(parseResult.GetValue(observationArgument)!);
            UserInterface.printLog("Observation recorded.");
        });

        var root = new RootCommand("Bison observation tracker")
        {
            Subcommands = { readCommand, observeCommand }
        };

        return root.Parse(args).Invoke();
    }

    
    
    static void handleObservation(string observation)
    {
        string author = Environment.UserName;
        string timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
        string filePath = "./Bison.CLI/data/bison_observe_cli_db.csv";

        try
        {
            using (StreamWriter sw = File.AppendText(filePath))
            {
                sw.WriteLine($"{author},\"{observation}\",{timestamp}");
            }
        }
        catch (Exception e)
        {
            UserInterface.printExeptionError(e);
        }

    }
}