using System.CommandLine;

public interface ICliCommand
{
    Command Build();
}