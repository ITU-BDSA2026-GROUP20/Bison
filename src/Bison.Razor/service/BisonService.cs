using Bison.Core.Repositories;
using Bison.Core.models;
using Bison.Razor;

public class ObservationService : IObservationService
{

    private readonly IObservationRepository repository;
    private readonly ICommentRepository commentRepository;
    private readonly IProposalRepository proposalRepository;
    private readonly ITaxonRepository taxonRepository;
    private readonly IAuthorRepository authorRepository;
    public ObservationService(
        IObservationRepository repository,
        ICommentRepository commentRepository,
        IProposalRepository proposalRepository,
        ITaxonRepository taxonRepository,
        IAuthorRepository authorRepository)
    {
        this.repository = repository;
        this.commentRepository = commentRepository;
        this.proposalRepository = proposalRepository;
        this.taxonRepository = taxonRepository;
        this.authorRepository = authorRepository;
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
            taxon = await taxonRepository.GetTaxon(taxonId);
            if (taxon is null)
                return null;
        }
        var author = await authorRepository.GetOrCreateAuthor(dto.AuthorId, dto.AuthorName, dto.AuthorEmail);

        var observation = await repository.AddObservation(new Observation
        {
            Text = dto.Text.Trim(),
            TimeStamp = DateTime.UtcNow,
            AuthorId = author.Id,
            Author = author,
            TaxonId = taxon?.TaxonId,
            Taxon = taxon,
            PhotoName = dto.PhotoName
        });

        return ToObservationDTO(observation);
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
            o.Taxon?.VernacularName,
            o.PhotoName);
    }
}