using SimpleDB;
using Bison.CLI.models;
using System.CommandLine;

namespace Bison.CLI.commands;

public class CommentCommand(IDatabaseRepository<Comment> database) : ICliCommand
{
    public Command Build()
    {
        Command commentCommand = new Command("comment", "Record a comment to an observation based on its ID");
        Argument<string> commentArgument = new Argument<string>("comment") { Description = "The comment to record" };
        Argument<string> commentIdArgument = new Argument<string>("commentId") { Description = "The ID of the observation to comment on" };
        commentCommand.Arguments.Add(commentArgument);
        commentCommand.Arguments.Add(commentIdArgument);
        commentCommand.SetAction(parseResult =>
        {
            string commentVal = parseResult.GetValue(commentArgument)
                ?? throw new ArgumentNullException(nameof(commentArgument), "Comment is required");

            string commentId = parseResult.GetValue(commentIdArgument)
                ?? throw new ArgumentNullException(nameof(commentIdArgument), "Comment ID is required");

            Guid commentGuid = Guid.Parse(commentId);
            Comment comment = new Comment(commentVal, commentGuid, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString());
            database.Store(comment);
            UserInterface.printLog("Comment recorded.");
        });

        return commentCommand;
    }
}