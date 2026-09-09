using SimpleDB;
using Bison.CLI.models;
using System.CommandLine;

namespace Bison.CLI.commands;

public class DiscussionCommand(IDatabaseRepository<Comment> database) : ICliCommand
{
    public Command Build()
    {
        Argument<string> commentIdArgument = new Argument<string>("commentId") { Description = "The ID of the commment to view the discussion of" };
        Command discussionCommand = new Command("discussion", "Print all comments for a specific observation ID");
        discussionCommand.Arguments.Add(commentIdArgument);
        discussionCommand.SetAction(ParseResult => {
            string commentId = ParseResult.GetValue(commentIdArgument)
                ?? throw new ArgumentNullException(nameof(commentIdArgument), "Comment ID is required");

            Guid commentGuid = Guid.Parse(commentId);

            List<Comment> comments = database.Read().ToList();
            UserInterface.printOutput(comments.Where(c => c.Id == commentGuid).ToList());
        });
        
        return discussionCommand;
    }
}