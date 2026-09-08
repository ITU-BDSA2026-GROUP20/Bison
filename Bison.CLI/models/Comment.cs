using System;

namespace Bison.CLI.models;

public class Comment
{
    public Comment() { }

    public Comment(string CommentVal, Guid CommentId, string Timestamp)
    {
        this.CommentVal = CommentVal;
        this.CommentId = CommentId;
        this.Timestamp = Timestamp;
    }

    public string CommentVal { get; set; } = string.Empty;
    public Guid CommentId { get; set; } = new Guid(); // Empty by default, needs to come from Constructor
    public string Timestamp { get; set; } = string.Empty;
}