namespace Bison.Core.models;

// Sent by the Razor app when a logged-in user creates an observation.
// AuthorId is the Author the user was previously linked to (null on first post);
// the server falls back to finding/creating an Author from AuthorEmail.
public record CreateObservationDTO(
    string Text,
    string AuthorName,
    string AuthorEmail,
    int? TaxonId = null,
    int? AuthorId = null
);
