namespace Bison.Core.models;

public record CreateCommentDTO(
    int ObservationId,
    string Text,
    string AuthorName,
    string AuthorEmail,
    int? AuthorId = null
);
