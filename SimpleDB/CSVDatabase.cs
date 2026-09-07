using System.Globalization;
using CsvHelper;

namespace SimpleDB;

public sealed class CSVDatabase<T> : IDatabaseRepository<T>
{
    private readonly string csvFilePath;

    public CSVDatabase(string filePath)
    {
        csvFilePath = filePath; 
    }

    public IEnumerable<T> Read(int? limit = null)
    {
        using (var reader = new StreamReader(csvFilePath)) 
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            IEnumerable<T> records = csv.GetRecords<T>();

            if (limit.HasValue)
            {
                records = records.Take(limit.Value);
            }

            return records.ToList();

        }
    
    }
    
    public void Store(T record)
    {
        bool fileNeedsHeader = !File.Exists(csvFilePath) || new FileInfo(csvFilePath).Length==0; 

        using (var writer = new StreamWriter(csvFilePath, true))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
           if(fileNeedsHeader)
            {
                csv.WriteHeader<T>();
                csv.NextRecord();
            }

            csv.WriteRecord(record);
            csv.NextRecord();
        }
    }

}