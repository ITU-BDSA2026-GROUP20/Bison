using System;
using System.Collections.Generic;
using System.Data;
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
        for (int i = 1; i < list.Count(); i++)
        {
            string[] row = list[i];

            for(int j = 0; j < row.Length; j++)
            {
                
                if(j == 2)
                {
                    Console.Write(DateTime.UnixEpoch.AddSeconds(double.Parse(row[j])));
                } else
                {
                    Console.Write(row[j]);
                }
                Console.Write(" ");

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
}
