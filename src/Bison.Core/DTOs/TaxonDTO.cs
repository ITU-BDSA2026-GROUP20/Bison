namespace Bison.Core.models;
public record TaxonDTO(
    int Id,
    int? ParentId,
    string? VernacularName
);