using System.Globalization;
using Bison.CLI.models;
using CsvHelper;
using Bison.CLI;


public class CsvHandler
{
    public List<Reading> getFileData(string filePath)
    {
        List<Reading> readings = new();

        try
        {
            using (var reader = new StreamReader(filePath))
            using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                readings = csvReader.GetRecords<Reading>().ToList();
                Console.Out.WriteLine(readings);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return readings;
    }
    
    public void handleObservation(string observation)
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
            UserInterface.printExceptionError(e);
        }

    }
}