using System.CommandLine;
using Bison.Core.models;
using Bison.CLI.commands;
using SimpleDB;
using System.Linq.Expressions;
using Bison.CLI;

class Program
{
    static int Main(string[] args)
    {
        ICliCommand[] commands = [
            new ReadCommand(),
            new ObserveCommand(),
            new CommentCommand(),
            new DiscussionCommand(),
            new ProposalCommand(),
            new ProposalsCommand(),
        ];

        RootCommand root = new RootCommand("Bison observation tracker");
        foreach (ICliCommand command in commands)
            root.Subcommands.Add(command.Build());
            
        return root.Parse(args).Invoke();
    }
}