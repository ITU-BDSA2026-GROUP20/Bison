using SimpleDB;
using Bison.Core.models;
using System.CommandLine;

namespace Bison.CLI.commands;

public class DiscussionCommand() : ICliCommand
{
    public Command Build()
    {
        Argument<string> commentIdArgument = new Argument<string>("commentId") { Description = "The ID of the commment to view the discussion of" };
        Command discussionCommand = new Command("discussion", "Print all comments for a specific observation ID");
        discussionCommand.Arguments.Add(commentIdArgument);
        discussionCommand.SetAction(ParseResult => {
            string commentId = ParseResult.GetValue(commentIdArgument)
                ?? throw new ArgumentNullException(nameof(commentIdArgument), "Comment ID is required");


            if (!Guid.TryParse(commentId, out Guid commentGuid))
            {
                Console.WriteLine("Unrecognized id format. Id needs to be a Guid");
                return;
            }

            //List<Comment> comments = database.Read().ToList();
            List<Comment> comments = Client.GetAsync<List<Comment>>("/comments").GetAwaiter().GetResult()
                ?? throw new ArgumentNullException("No values in database");

            UserInterface.printOutput(comments.Where(c => c.Id == commentGuid).ToList());

        });
        
        return discussionCommand;
    }
}