using SimpleDB;
using Bison.Core.models;
using System.CommandLine;

namespace Bison.CLI.commands;

public class ReadCommand() : ICliCommand
{
    public Command Build()
    {
        Command readCommand = new Command("read", "Read all readings from the database");
        readCommand.SetAction(async parseResult =>
        {
            List<Reading> lines = Client.GetAsync<List<Reading>>("/observations").GetAwaiter().GetResult()
                ?? throw new ArgumentNullException("Returned null");
            
            
            UserInterface.printOutput(lines);
        });
        
        return readCommand;
    }    
}