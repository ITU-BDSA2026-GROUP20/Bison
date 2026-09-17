using Bison.Core.models;

namespace Bison.CLI;

class UserInterface
{
    public static void printOutput<T>(IEnumerable<T> output) where T : IPrintable
    {
        foreach (T item in output)
        {
            Console.WriteLine(item.ToString());
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