using System.CommandLine;
using Bison.CLI.models;
using Bison.CLI.commands;
using SimpleDB;
using System.Linq.Expressions;
using Bison.CLI;

class Program
{
    static int Main(string[] args)
    {
        // Singleton instance of the CSVDatabase (No possibilities for duplicate databases)
        CSVDatabase csvDatabase = CSVDatabase.Instance;
        IDatabaseRepository<Reading> readingDatabase = csvDatabase.GetRepository<Reading>("reading");
        IDatabaseRepository<Comment> commentDatabase = csvDatabase.GetRepository<Comment>("comment");

        ICliCommand[] commands = [
            new ReadCommand(readingDatabase),
            new ObserveCommand(readingDatabase),
            new CommentCommand(commentDatabase),
            new DiscussionCommand(commentDatabase),
        ];

        RootCommand root = new RootCommand("Bison observation tracker");
        foreach (ICliCommand command in commands)
            root.Subcommands.Add(command.Build());
            
        return root.Parse(args).Invoke();
    }
}