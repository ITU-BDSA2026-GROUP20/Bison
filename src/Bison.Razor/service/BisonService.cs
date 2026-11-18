using Bison.Core.Repositories;
using Bison.Core.models;
using Bison.Razor;
using Bison.Infrastructure;
using Microsoft.EntityFrameworkCore;

public class ObservationService : IObservationService
{

    private readonly IObservationRepository repository;
    private readonly ICommentRepository commentRepository;
    private readonly IProposalRepository proposalRepository;
    private readonly ITaxonRepository taxonRepository;
    private readonly Database database;
    public ObservationService(
        IObservationRepository repository,
        ICommentRepository commentRepository,
        IProposalRepository proposalRepository,
        ITaxonRepository taxonRepository,
        Database database)
    {
        this.repository = repository;
        this.commentRepository = commentRepository;
        this.proposalRepository = proposalRepository;
        this.taxonRepository = taxonRepository;
        this.database = database;
    }

    public List<Observation> GetObservationEntities()
    {
        return repository.GetObservations().GetAwaiter().GetResult().ToList();
    }
    public List<ObservationDTO> GetAllObservations()
    {
        return GetObservationEntities()
        .Select(ToObservationDTO)
        .ToList();
    }
    public List<ObservationDTO> GetObservations(int? page = null)
    {
        return repository.GetTimelineObservations(null, page ?? 1)
        .GetAwaiter().GetResult()
        .Select(o => ToObservationDTO(o) with { TaxonName = o.Taxon?.VernacularName ?? string.Empty })
        .ToList();
    }
    public List<ObservationDTO> GetObservations(int userId, int? page = null)
    {
        return repository.GetTimelineObservations(userId, page ?? 1)
            .GetAwaiter().GetResult()
            .Select(o => ToObservationDTO(o) with { TaxonName = o.Taxon?.VernacularName ?? string.Empty })
            .ToList();
    }

    public ObservationDTO? GetObservation(int? id)
    {
        if (id is null)
            return null;

        var observation = repository.GetObservation(id.Value)
            .GetAwaiter().GetResult();

        if (observation is null)
            return null;

        return ToObservationDTO(observation);
    }
    public List<CommentDTO> GetComments(int id)
    {
        return commentRepository.GetComments(id)
            .GetAwaiter().GetResult()
            .Select(c => new CommentDTO(
                c.Text,
                c.TimeStamp.ToString("yyyy-MM-dd")))
            .ToList();
    }

    public List<ProposalDTO> GetProposals(int id)
    {
        return proposalRepository.GetProposals(id)
            .GetAwaiter().GetResult()
            .Select(p => new ProposalDTO(
                p.Taxon?.VernacularName,
                p.TimeStamp.ToString("yyyy-MM-dd")))
            .ToList();
    }

    public List<TaxonDTO> GetUniqueTaxons()
    {
        return taxonRepository.GetTaxons()
            .GetAwaiter().GetResult()
            .Where(t => t.VernacularName != null)
            .OrderBy(t => t.VernacularName)
            .Select(t => new TaxonDTO(
                t.TaxonId,
                t.ParentTaxonId,
                t.VernacularName))
            .ToList();
    }

    public async Task<ObservationDTO?> AddObservation(CreateObservationDTO dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Text) || dto.Text.Length > 500)
            return null;

        if (string.IsNullOrWhiteSpace(dto.AuthorEmail) ||
            string.IsNullOrWhiteSpace(dto.AuthorName))
            return null;

        Taxon? taxon = null;
        if (dto.TaxonId is int taxonId)
        {
            taxon = await database.taxons.FindAsync(taxonId);
            if (taxon is null)
                return null;
        }

        Author? author = dto.AuthorId is int authorId
            ? await database.users.FindAsync(authorId)
            : null;

        author ??= await database.users.FirstOrDefaultAsync(
            a => a.Email == dto.AuthorEmail);

        if (author is null)
        {
            author = new Author
            {
                Name = dto.AuthorName,
                Email = dto.AuthorEmail
            };
            database.users.Add(author);
            await database.SaveChangesAsync();
        }

        var observation = await repository.AddObservation(new Observation
        {
            Text = dto.Text.Trim(),
            TimeStamp = DateTime.UtcNow,
            AuthorId = author.Id,
            TaxonId = taxon?.TaxonId
        });

        return new ObservationDTO(
            observation.Id,
            observation.Text,
            author.Id,
            author.Name,
            observation.TimeStamp.ToString("yyyy-MM-dd"),
            taxon?.TaxonId,
            taxon?.VernacularName);
    }

    public Taxon GetTaxon(int id)
    {
        return taxonRepository.GetTaxon(id)
        .GetAwaiter().GetResult() ?? new Taxon();
    }

    private static ObservationDTO ToObservationDTO(Observation o)
    {
        return new ObservationDTO(
            o.Id,
            o.Text,
            o.AuthorId,
            o.Author.Name,
            o.TimeStamp.ToString("yyyy-MM-dd"),
            o.TaxonId,
            o.Taxon?.VernacularName);
    }

}