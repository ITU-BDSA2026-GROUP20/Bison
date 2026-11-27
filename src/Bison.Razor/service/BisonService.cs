using Bison.Core.Repositories;
using Bison.Core.models;
using Bison.Core.models.BattlePass;
using Bison.Razor;

public class ObservationService : IObservationService
{
    private const int ObservationXp = 50;
    private const int ProposalXp = 20;
    private const int CommentXp = 10;

    private readonly IObservationRepository repository;
    private readonly ICommentRepository commentRepository;
    private readonly IProposalRepository proposalRepository;
    private readonly ITaxonRepository taxonRepository;
    private readonly IAuthorRepository authorRepository;
    private readonly IBattlePassRepository battlePassRepository;

    public ObservationService(
        IObservationRepository repository,
        ICommentRepository commentRepository,
        IProposalRepository proposalRepository,
        ITaxonRepository taxonRepository,
        IAuthorRepository authorRepository,
        IBattlePassRepository battlePassRepository)
    {
        this.repository = repository;
        this.commentRepository = commentRepository;
        this.proposalRepository = proposalRepository;
        this.taxonRepository = taxonRepository;
        this.authorRepository = authorRepository;
        this.battlePassRepository = battlePassRepository;
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
    public List<CommentDTO> GetComments(int id) => commentRepository.GetComments(id).GetAwaiter().GetResult().Select(ToCommentDTO).ToList();

    public List<ProposalDTO> GetProposals(int id) => proposalRepository.GetProposals(id).GetAwaiter().GetResult().Select(ToProposalDTO).ToList();

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
        author.AddXp(ObservationXp);

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

    public async Task<CommentDTO?> AddComment(CreateCommentDTO dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Text) || dto.Text.Length > 500) return null;
        if (string.IsNullOrWhiteSpace(dto.AuthorEmail) || string.IsNullOrWhiteSpace(dto.AuthorName)) return null;
        if (await repository.GetObservation(dto.ObservationId) is null) return null;

        var author = await authorRepository.GetOrCreateAuthor(dto.AuthorId, dto.AuthorName, dto.AuthorEmail);
        author.AddXp(CommentXp);
        var comment = await commentRepository.AddComment(new Comment { Text = dto.Text.Trim(), TimeStamp = DateTime.UtcNow, AuthorId = author.Id, Author = author, ObservationId = dto.ObservationId });
        return ToCommentDTO(comment);
    }

    public async Task<ProposalDTO?> AddProposal(CreateProposalDTO dto)
    {
        if (dto.Text.Length > 500) return null;
        if (string.IsNullOrWhiteSpace(dto.AuthorEmail) || string.IsNullOrWhiteSpace(dto.AuthorName)) return null;
        if (await repository.GetObservation(dto.ObservationId) is null) return null;

        var taxon = dto.TaxonId is int taxonId ? await taxonRepository.GetTaxon(taxonId) : null;
        if (taxon is null) return null;

        var author = await authorRepository.GetOrCreateAuthor(dto.AuthorId, dto.AuthorName, dto.AuthorEmail);
        author.AddXp(ProposalXp);
        var proposal = await proposalRepository.AddProposal(new Proposal { Text = dto.Text.Trim(), TimeStamp = DateTime.UtcNow, AuthorId = author.Id, Author = author, ObservationId = dto.ObservationId, TaxonId = taxon.TaxonId, Taxon = taxon });
        return ToProposalDTO(proposal);
    }

    public List<BattlePassItemDTO> GetBattlePassItems()
    {
        return battlePassRepository.getBattlePassItems()
            .GetAwaiter().GetResult()
            .Select(BattlePassItemDTO (t) => t switch
            {
                Unlock u => ToUnlockDTO(u),
                ReSkin r => ToReskinDTO(r),
                _ => throw new NotImplementedException($"Unknown BattlePassItem type: {t.GetType().Name}")
            })
            .ToList();
    }

    public Author GetAuthor(int id)
    {
        return authorRepository.GetAuthorById(id)
        .GetAwaiter().GetResult() ?? new Author();
    }

    public bool HasUpgradedBattlePass(int userId)
    {
        return GetAuthor(userId).BattlePassUpgraded;
    }

    public int GetBattlePassTier(int userId)
    {
        return GetAuthor(userId).BattlePassTier;
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

    private static UnlockDTO ToUnlockDTO(Unlock u) => new(u.Id, u.Tier, u.IsPaid, u.Feature);
    private static ReskinDTO ToReskinDTO(ReSkin r) => new(r.Id, r.Tier, r.IsPaid, r.PrimaryColor, r.SecondaryColor, r.BackgroundColor, r.TextColor);
    private static CommentDTO ToCommentDTO(Comment c) => new(c.AuthorId, c.Author.Name, c.Text, c.TimeStamp.ToString("yyyy-MM-dd"));
    private static ProposalDTO ToProposalDTO(Proposal p) => new(p.AuthorId, p.Author.Name, p.Taxon.VernacularName, p.Text, p.TimeStamp.ToString("yyyy-MM-dd"));
}