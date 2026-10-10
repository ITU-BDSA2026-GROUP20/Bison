namespace Bison.Core.models;
public record ObservationDTO(
    int Id,
    string Text,
    string AuthorId,
    string AuthorName,
    DateTime TimeStamp
);