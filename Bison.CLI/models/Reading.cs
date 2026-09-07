namespace Bison.CLI.models;

internal class Reading
{
    internal Reading() { }

    internal Reading(string Author, string Observation, string Timestamp)
    {
        this.Author = Author;
        this.Observation = Observation;
        this.Timestamp = Timestamp;
    }

    internal string Author { get; set; } = string.Empty;
    internal string Observation { get; set; } = string.Empty;
    internal string Timestamp { get; set; } = string.Empty;
}