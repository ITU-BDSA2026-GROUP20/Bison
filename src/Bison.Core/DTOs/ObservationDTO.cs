namespace Bison.Core.models;
public record ObservationDTO(
    int Id,
    string Text,
    int AuthorId,
    string AuthorName,
    string TimeStamp
);