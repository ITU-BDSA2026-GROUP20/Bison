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
        for (int i = 0; i < list.Count(); i++)
        {
            if(i == 0) continue;

            string[] row = list[i];

            for(int j = 0; j < row.Length; j++)
            {
                
                if(j == 2)
                {
                    Console.Write(UnixTimestampToDateTime(double.Parse(row[j])));
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

    public static DateTime UnixTimestampToDateTime(double UnixTimeStamp)
    {
        DateTime dateTime = new DateTime(1970,1,1,0,0,0,0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(UnixTimeStamp).ToLocalTime();
        return dateTime;

    }
}