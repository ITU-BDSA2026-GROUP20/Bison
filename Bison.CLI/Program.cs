using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        inputParser parser = new inputParser();

        if(args.Length < 1)
        {
            Console.WriteLine("Please provide an argument");
            return;
        }

        switch(args[0])
        {
            case "read":
                List<string[]> lines = parser.getFileData("./data/bison_observe_cli_db.csv");
                printOutput(lines);
                break;
            case "observe":
                handleObservation(args[1]);
                Console.WriteLine("Observation recorded");
                break;
            default:
                Console.WriteLine("Unknown argument");
                break;
        }
    }

    static void printOutput(List<string[]> list)
    {
        for (int i = 1; i < list.Count; i++)
        {
            string[] row = list[i];

            string author = row[0];
            string observation = row[1];
            DateTime timestamp = DateTime.UnixEpoch.AddSeconds(long.Parse(row[2]));
            
            Console.WriteLine(author + " @ " + timestamp + " " + observation);
        }
    }
    
    
    static void handleObservation(string observation)
    {
        string author = Environment.UserName;
        string timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
        string filePath = "./data/bison_observe_cli_db.csv";

        try
        {
            using (StreamWriter sw = File.AppendText(filePath))
            {
                sw.WriteLine($"{author},\"{observation}\",{timestamp}");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

    }
}