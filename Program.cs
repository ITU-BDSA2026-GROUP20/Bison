using System;
using System.Collections.Generic;

class Program
{

    static void Main()
    {
        string csvFilePath = "./data/bison_observe_cli_db.csv";
        List<string[]> output = CsvParser.readInput(csvFilePath);

        printOutput(output);

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
}