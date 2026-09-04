using System.Transactions;
using Bison.CLI.Models;

namespace Bison.CLI;

class UserInterface
{
    public static void printOutput(List<Reading> list)
    {
        for (int i = 1; i < list.Count; i++)
        {
            Reading current = list[i];
            
            string author = current.Author;
            string observation = current.Observation;
            DateTime timestamp = DateTime.UnixEpoch.AddSeconds(long.Parse(current.Timestamp));
            
            Console.WriteLine(author + " @ " + timestamp + " " + observation);
        }
    }

    public static void printObservationRecorded()
    {
        Console.WriteLine("Observation recorded.");
    }

    public static void printMissingArgument()
    {
        Console.WriteLine("Please provide an argument");
    }

    public static void printUnkownArgument()
    {
        Console.WriteLine("Unknown argument");
    }

    public static void printExeptionError(Exception e)
    {
        Console.WriteLine(e);
    }

}