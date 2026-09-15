using SimpleDB;
using Bison.CLI.models;
using System.CommandLine;

namespace Bison.CLI.commands;

public class ReadCommand(IDatabaseRepository<Reading> database) : ICliCommand
{
    public Command Build()
    {
        Command readCommand = new Command("read", "Read all readings from the database");
        readCommand.SetAction(async parseResult =>
        {
            List<Reading> lines = await Client.GetAsync<List<Reading>>("/observations")
                ?? throw new ArgumentNullException("Returned null");
            
            
            UserInterface.printOutput(lines);
        });
        
        return readCommand;
    }    
}