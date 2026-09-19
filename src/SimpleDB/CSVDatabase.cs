using SimpleDB;

public sealed class CSVDatabase
{
    private static CSVDatabase? instance;
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

    /// /////////////////////////////////////////////////////////////////////// ///
    /// IMPORTANT REFACTOR NEEDE IN FUTURE!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! ///
    /// /////////////////////////////////////////////////////////////////////// ///
    public IDatabaseRepository<T> GetRepository<T>(string database)
    {
        // IMPORTANT, remove ".." and make it "." once the project is fully finished so the data folder is made next to the exe and not the folder its inside
        string databasePath = $"../data/bison_{database}_cli_db.csv";

        // Make sure it exists or if its a new type create it automatically
        string? directory = Path.GetDirectoryName(Path.GetFullPath(databasePath));
        if (directory is not null)
            Directory.CreateDirectory(directory);

        if (!File.Exists(databasePath))
            File.Create(databasePath).Dispose();

        return new IDatabaseRepositoryImpl<T>(databasePath);
    }
}