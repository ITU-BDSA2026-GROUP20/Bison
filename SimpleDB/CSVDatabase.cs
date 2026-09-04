using System.Globalization;
using CsvHelper;

namespace SimpleDB;

public sealed class CSVDatabase<T> : IDatabaseRepository<T>
{

    private readonly string _filePath;

    public CSVDatabase(string filePath)
    {
        _filePath = filePath; 
    }

    public IEnumerable<T> Read(int? limit = null)
    {
        using (var reader = new StreamReader(_filePath)) 
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
        throw new NotImplementedException();
    }

}