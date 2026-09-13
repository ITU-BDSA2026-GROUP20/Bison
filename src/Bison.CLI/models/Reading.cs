using System;
using Bison.CLI.utils;

namespace Bison.CLI.models;

public class Reading : IPrintable
{
    public Reading() { }

    public Reading(string Author, string Observation, string Timestamp)
    {
        this.Author = Author;
        this.Observation = Observation;
        this.Timestamp = Timestamp;
    }

    public string Author { get; set; } = string.Empty;
    public string Observation { get; set; } = string.Empty;
    public string Timestamp { get; set; } = string.Empty;
    public Guid Id { get; set; } = Guid.NewGuid();

    public override string ToString()
    {
        DateTime fTimeStamp = TimestampConverter.FromUnixSeconds(Timestamp);
        return Author + " @ " + fTimeStamp + " " + Observation + " " + Id.ToString();
    }
}