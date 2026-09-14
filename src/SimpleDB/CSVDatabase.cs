using System.Collections;
using SimpleDB;

public sealed class CSVDatabase
{
    private static CSVDatabase? instance;
    private readonly string csvReadFilePath;
    private readonly string csvCommentFilePath;
    private CSVDatabase()
    {
        csvReadFilePath = "./data/bison_observe_cli_db.csv";
        csvCommentFilePath = "./data/bison_comment_cli_db.csv";
    }
    public static CSVDatabase Instance
    {
        get // The singleton pattern for the CSVDatabase
        {
            if (instance == null)
            {
                instance = new CSVDatabase();
            }
            return instance;
        }
    }

    public IDatabaseRepository<T> GetRepository<T>(CSVFile file)
    {
        switch(file)
        {
            case CSVFile.Reading:
            return new IDatabaseRepositoryImpl<T>(csvReadFilePath);
            
            case CSVFile.Comment:
            return new IDatabaseRepositoryImpl<T>(csvCommentFilePath);

            default:
            throw new ArgumentOutOfRangeException(nameof(file));
        }
    }
}