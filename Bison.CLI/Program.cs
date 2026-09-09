using System.CommandLine;
using Bison.CLI.models;
using Bison.CLI.commands;
using SimpleDB;

class Program
{
    static int Main(string[] args)
    {
        IDatabaseRepository<Reading> csvDatabase = new IDatabaseRepositoryImpl<Reading>("./data/bison_observe_cli_db.csv");
        IDatabaseRepository<Comment> commentDatabase = new IDatabaseRepositoryImpl<Comment>("./data/bison_comment_cli_db.csv");

        ICliCommand[] commands = [
            new ReadCommand(csvDatabase),
            new ObserveCommand(csvDatabase),
            new CommentCommand(commentDatabase),
            new DiscussionCommand(commentDatabase),
        ];

        RootCommand root = new RootCommand("Bison observation tracker");
        foreach (ICliCommand command in commands)
            root.Subcommands.Add(command.Build());
            
        return root.Parse(args).Invoke();
    }
}