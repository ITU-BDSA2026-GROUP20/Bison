using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        List<string[]> lines = getFileData();

        printOutput(lines);
    }

    static void printOutput(List<string[]> list)
    {
        foreach(string[] vals in list)
        {
            foreach(string item in vals)
            {
                Console.Write(item + " ");
            }
            Console.WriteLine();
        }
    }

    // In the future make a class we can parse into
    static List<string[]> getFileData()
    {
        List<string[]> rows = new();

        try
        {
            string[] lines = File.ReadAllLines("../../../data/bison_observe_cli_db.csv");

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
}