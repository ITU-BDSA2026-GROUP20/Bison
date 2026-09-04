using System.Globalization;
using Bison.CLI.models;
using CsvHelper;


public class InputParser
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
}