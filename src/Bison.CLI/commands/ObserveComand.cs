using SimpleDB;
using Bison.Core.models;
using System.CommandLine;

namespace Bison.CLI.commands;

public class ObserveCommand() : ICliCommand
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

            Reading result = Client.PostAsync<Reading>("/observation", reading).GetAwaiter().GetResult()
                ?? throw new InvalidDataException("Data has been returned null, data not stored in database");
            UserInterface.printLog("Observation recorded.");
        });

        return observeCommand;
    }    
}