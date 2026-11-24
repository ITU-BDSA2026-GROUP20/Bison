namespace Bison.Core.models;
public record CommentDTO(
    int AuthorId,
    string AuthorName,
    string Text,
    string TimeStamp
);