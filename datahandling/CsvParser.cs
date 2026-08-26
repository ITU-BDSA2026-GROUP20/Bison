using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;

class CsvParser
{
    public static List<string[]> readInput(string path)
    {
        List<string[]> rows = new List<string[]>();

        try
        {
            string[] lines = File.ReadAllLines(path);

            foreach(string line in lines) {
                string[] values = line.Split(",");
                rows.Add(values);
            }

        } catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return rows;
    }
}