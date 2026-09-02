public class inputParser
{
    public List<string[]> getFileData(string filePath)
    {
        List<string[]> rows = new();

        try
        {
            string[] lines = File.ReadAllLines("./data/bison_observe_cli_db.csv");

            foreach (string line in lines)
            {
                string[] values = line.Split(",");
                rows.Add(values);
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return rows;
    }
}