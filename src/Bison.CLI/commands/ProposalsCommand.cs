using SimpleDB;
using Bison.Core.models;
using System.CommandLine;

namespace Bison.CLI.commands;

public class ProposalsCommand() : ICliCommand
{
    public Command Build()
    {
        Command proposalCommand = new Command("proposals", "Find all proposals for a given observation based on its ID");
        Argument<string> observationIdArgument = new Argument<string>("observationId") { Description = "The ID of the observation to propose a taxon for"};
        proposalCommand.Arguments.Add(observationIdArgument);
        proposalCommand.SetAction(parseResult =>
        {
            string observationId = parseResult.GetValue(observationIdArgument)
                ?? throw new ArgumentNullException(nameof(observationIdArgument), "Observation ID is required");

            if (!Guid.TryParse(observationId, out Guid observationGuid))
            {
                Console.WriteLine("Unrecognized id format. Id needs to be a Guid");
                return;
            }
            
            List<Proposal> proposals = Client.GetAsync<List<Proposal>>("/proposals", observationGuid).GetAwaiter().GetResult()
                ?? throw new InvalidDataException("Data has been returned null, data not stored in database");

            UserInterface.printOutput(proposals);
        });

        return proposalCommand;
    }
}