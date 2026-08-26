using System;
using System.Collections.Generic;

class Program
{

    static void Main()
    {
        string csvFilePath = "./data/bison_observe_cli_db.csv";
        List<string[]> ouput = CsvParser.readInput(csvFilePath);
    }
}