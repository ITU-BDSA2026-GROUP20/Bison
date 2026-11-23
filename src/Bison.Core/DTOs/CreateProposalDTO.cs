namespace Bison.Core.models;

public record CreateProposalDTO(
    int ObservationId,
    int? TaxonId,
    string Text,
    string AuthorName,
    string AuthorEmail,
    int? AuthorId = null
);
