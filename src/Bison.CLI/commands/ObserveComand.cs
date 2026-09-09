using SimpleDB;
using Bison.CLI.models;
using System.CommandLine;

namespace Bison.CLI.commands;

public class ObserveCommand(IDatabaseRepository<Reading> database) : ICliCommand
{
    public Command Build()
    {
        Command observeCommand = new Command("observe", "Record a new observation");
        Argument<string> observationArgument = new Argument<string>("observation") { Description = "The observation to record" };

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

            database.Store(reading);
            UserInterface.printLog("Observation recorded.");
        });

        return observeCommand;
    }    
}