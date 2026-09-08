using Bison.CLI.models;

namespace Bison.CLI;

class UserInterface
{
    public static void printOutput(List<Reading> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Reading current = list[i];
            
            string author = current.Author;
            string observation = current.Observation;
            DateTime timestamp = DateTime.UnixEpoch.AddSeconds(long.Parse(current.Timestamp));
            
            Console.WriteLine(author + " @ " + timestamp + " " + observation);
        }
    }

    public static void printLog(string log)
    {
        Console.WriteLine(log);
    }
    
    public static void printExceptionError(Exception e)
    {
        if(e.InnerException != null)
        {
            Console.WriteLine($"Error Occurred: {e} ({e.InnerException})");
        }
        else
        {
            Console.WriteLine($"Error Occurred: {e}");
        }
    }
    
}