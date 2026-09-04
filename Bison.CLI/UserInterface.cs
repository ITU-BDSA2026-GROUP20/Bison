using Bison.CLI.models;
using System;
using System.Collections.Generic;

namespace Bison.CLI;

// KEEP ALL Console.WriteLine(...) IN HERE
class UserInterface
{
    public static void printOutput(List<Reading> list)
    {
        // (Author,Observation,Timestamp) is not a reading, so start with index 0
        for (int i = 0; i < list.Count; i++)
        {
            Reading current = list[i];
            
            string author = current.Author;
            string observation = current.Observation;
            DateTime timestamp = DateTime.UnixEpoch.AddSeconds(long.Parse(current.Timestamp));
            
            Console.WriteLine(author + " @ " + timestamp + " " + observation);
        }
    }

    //Function for printing messages to the user.
    public static void printLog(string log)
    {
        Console.WriteLine(log);
    }

    // Print the error log
    public static void printExeptionError(Exception e)
    {
        Console.WriteLine(e);
    }
    
    // Print an error message
    public static void printExeptionError(string message)
    {
        Console.WriteLine(message);
    }
}