using SimpleDB;
using Bison.Core.models;
using System.CommandLine;

namespace Bison.CLI.commands;

public class ProposalCommand() : ICliCommand
{
    public Command Build()
    {
        Command proposalCommand = new Command("proposal", "Record a taxon proposal to an observation based on its ID");
        Argument<string> taxonIdArgument = new Argument<string>("taxonId") { Description = "The taxon ID to propose"};
        Argument<string> observationIdArgument = new Argument<string>("observationId") { Description = "The ID of the observation to propose a taxon for"};
        proposalCommand.Arguments.Add(taxonIdArgument);
        proposalCommand.Arguments.Add(observationIdArgument);
        proposalCommand.SetAction(parseResult =>
        {
            string taxonId = parseResult.GetValue(taxonIdArgument)
                ?? throw new ArgumentNullException(nameof(taxonIdArgument), "Taxon ID is required");

            string observationId = parseResult.GetValue(observationIdArgument)
                ?? throw new ArgumentNullException(nameof(observationIdArgument), "Observation ID is required");

            if (!Guid.TryParse(observationId, out Guid observationGuid))
            {
                Console.WriteLine("Unrecognized id format. Id needs to be a Guid");
                return;
            }
            
            Proposal proposal = new Proposal(taxonId, observationGuid, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString());
            Proposal result = Client.PostAsync<Proposal>("/proposal", proposal).GetAwaiter().GetResult()
                ?? throw new InvalidDataException("Data has been returned null, data not stored in database");

            UserInterface.printLog("Proposal recorded.");
        });

        return proposalCommand;
    }
}