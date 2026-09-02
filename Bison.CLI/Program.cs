using System.CommandLine;

class Program
{
    static int Main(string[] args)
    {
        inputParser parser = new inputParser();

        var readCommand = new Command("read", "Print recorded observations");
        readCommand.SetAction(parseResult =>
        {
            List<string[]> lines = parser.getFileData("./data/bison_observe_cli_db.csv");
            printOutput(lines);
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
            Console.WriteLine("Observation recorded");
        });

        var root = new RootCommand("Bison observation tracker")
        {
            Subcommands = { readCommand, observeCommand }
        };

        return root.Parse(args).Invoke();
    }

    static void printOutput(List<string[]> list)
    {
        for (int i = 1; i < list.Count; i++)
        {
            string[] row = list[i];

            string author = row[0];
            string observation = row[1];
            DateTime timestamp = DateTime.UnixEpoch.AddSeconds(long.Parse(row[2]));
            
            Console.WriteLine(author + " @ " + timestamp + " " + observation);
        }
    }
    
    
    static void handleObservation(string observation)
    {
        string author = Environment.UserName;
        string timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
        string filePath = "./data/bison_observe_cli_db.csv";

        try
        {
            using (StreamWriter sw = File.AppendText(filePath))
            {
                sw.WriteLine($"{author},\"{observation}\",{timestamp}");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

    }
}