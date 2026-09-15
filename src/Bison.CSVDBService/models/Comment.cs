using System;
using Bison.CLI.utils;

namespace Bison.CSVDBService.models;

public class Comment : IPrintable
{
    public Comment() { }

    public Comment(string CommentVal, Guid Id, string Timestamp)
    {
        this.CommentVal = CommentVal;
        this.Id = Id;
        this.Timestamp = Timestamp;
    }

    public string CommentVal { get; set; } = string.Empty;
    public Guid Id { get; set; } = new Guid(); // Empty by default, needs to come from Constructor
    public string Timestamp { get; set; } = string.Empty;

    public override string ToString()
    {
        DateTime fTimeStamp = TimestampConverter.FromUnixSeconds(Timestamp);
        return CommentVal + " @ " + fTimeStamp + " " + Id.ToString();
    }
}