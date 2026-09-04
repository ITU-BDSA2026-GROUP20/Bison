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
    
    public void handleObservation(string observation, string path)
    {
        string author = Environment.UserName;
        string timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();

        var records = new List<Reading>
        {
            new Reading { Author = author, Observation = observation, Timestamp = timestamp }
        };

        try
        {
            using (var writer = new StreamWriter(path))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(records);
            }
        }
        catch (Exception e)
        {
            UserInterface.printExceptionError(e);
        }

    }
}