using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        if(args.Length < 1)
        {
            Console.WriteLine("Please provide an argument");
            return;
        }

        switch(args[0])
        {
            case "read":
                List<string[]> lines = getFileData();
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
        for (int i = 1; i < list.Count(); i++)
        {
            string[] row = list[i];

            string author = row[0];
            string observation = row[1];
            DateTime timestamp = DateTime.UnixEpoch.AddSeconds(long.Parse(row[2]));
            
            Console.WriteLine(author + " @ " + timestamp + " " + observation);
        }
    }

    // In the future make a class we can parse into
    static List<string[]> getFileData()
    {
        List<string[]> rows = new();

        try
        {
            string[] lines = File.ReadAllLines("./data/bison_observe_cli_db.csv");

            foreach (string line in lines)
            {
                string[] values = line.Split(",");
                rows.Add(values);
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return rows;
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