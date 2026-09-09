using System.CommandLine;
using Bison.CLI.models;
using Bison.CLI;
using SimpleDB;

class Program
{
    static int Main(string[] args)
    {
        IDatabaseRepository<Reading> csvDatabase = new IDatabaseRepositoryImpl<Reading>("./data/bison_observe_cli_db.csv");
        IDatabaseRepository<Comment> commentDatabase = new IDatabaseRepositoryImpl<Comment>("./data/bison_comment_cli_db.csv");

        var readCommand = new Command("read", "Print recorded observations");
        readCommand.SetAction(parseResult =>
        {
            List<Reading> lines = csvDatabase.Read().ToList();
            UserInterface.printOutput(lines);
        });

        var observationArgument = new Argument<string>("observation")
        {
            Description = "The observation to record"
        };

        var observeCommand = new Command("observe", "Record a new observation");
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

            csvDatabase.Store(reading);
            UserInterface.printLog("Observation recorded.");
        });


        var commentArgument = new Argument<string>("comment")
        {
            Description = "The comment to record"
        };

        var commentIdArgument = new Argument<string>("commentId")
        {
            Description = "The ID of the observation to comment on"
        };

        var commentCommand = new Command("comment", "Record a comment to an observation based on its ID");
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
            commentDatabase.Store(comment);
        });

        var discussionCommand = new Command("discussion", "Print all comments for a specific observation ID");
        discussionCommand.Arguments.Add(commentIdArgument);
        discussionCommand.SetAction(ParseResult => {
            string commentId = ParseResult.GetValue(commentIdArgument)
                ?? throw new ArgumentNullException(nameof(commentIdArgument), "Comment ID is required");

            Guid commentGuid = Guid.Parse(commentId);

            List<Comment> comments = commentDatabase.Read().ToList();
            UserInterface.printOutput(comments.Where(c => c.Id == commentGuid).ToList());
        });

        var root = new RootCommand("Bison observation tracker")
        {
            Subcommands = { readCommand, observeCommand, commentCommand, discussionCommand }
        };

        return root.Parse(args).Invoke();
    }
}