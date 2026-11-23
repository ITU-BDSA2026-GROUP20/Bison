namespace Bison.Core.models;
public record ProposalDTO(
    int AuthorId,
    string AuthorName,
    string? TaxonName,
    string Text,
    string TimeStamp
);