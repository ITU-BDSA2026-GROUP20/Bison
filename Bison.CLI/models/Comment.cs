using System;

namespace Bison.CLI.models;

public class Comment : Printable
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
        DateTime fTimeStamp = DateTime.UnixEpoch.AddSeconds(long.Parse(Timestamp));
        return CommentVal + " @ " + fTimeStamp + " " + Id.ToString();
    }
}