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

    //Functions for printing messages to the user
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