using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Bison.CLI;
using Bison.CLI.models;

class Program
{
    static void Main(string[] args)
    {
        inputParser parser = new inputParser();

        if(args.Length < 1)
        {
            UserInterface.printMissingArgument();
            return;
        }

        switch(args[0])
        {
            case "read":
                List<Reading> lines = parser.getFileData("./Bison.CLI/data/bison_observe_cli_db.csv");
                UserInterface.printOutput(lines);
                break;
            case "observe":
                handleObservation(args[1]);
                UserInterface.printObservationRecorded();
                break;
            default:
                UserInterface.printUnkownArgument();
                break;
        }
    }
    
    static void handleObservation(string observation)
    {
        string author = Environment.UserName;
        string timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
        string filePath = "./Bison.CLI/data/bison_observe_cli_db.csv";

        try
        {
            using (StreamWriter sw = File.AppendText(filePath))
            {
                sw.WriteLine($"{author},\"{observation}\",{timestamp}");
            }
        }
        catch (Exception e)
        {
            UserInterface.printExeptionError(e);
        }

    }
}