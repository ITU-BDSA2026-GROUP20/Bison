using Bogus;
using Bison.Core.models;

namespace Bison.Tests;
/// <summary>
/// This class generates random data for testing purposes, including readings, comments, and proposals.
/// We use this for our End2End Fuzz testing. The random generated data is 'mostly valid', this is on purpose
/// to test and make sure we catch invalid readings, comments and proposals. 
/// </summary>
public sealed class RandomDataGenerator
{
    private readonly Faker faker = new Faker();
    private readonly Random random = new Random();
    private readonly IReadOnlyList<string> validTaxonIdsList;
    private const double InvalidReferenceProbability = 0.1;
    public RandomDataGenerator(IEnumerable<string> validTaxonIds, int? seed = null)
    {
        int actualSeed = seed ?? Random.Shared.Next();
        random = new Random(actualSeed);

        faker.Random = new Randomizer(actualSeed);

        validTaxonIdsList = validTaxonIds.ToList();

        if(validTaxonIdsList.Count == 0)
            throw new ArgumentException("validTaxonIds cannot be empty.", nameof(validTaxonIds));
    
    }
    private string PickTaxonId()
    {
        if(random.NextDouble() >= InvalidReferenceProbability)
        return validTaxonIdsList[random.Next(validTaxonIdsList.Count)];
        
        return "invalid" + Guid.NewGuid();
    }
    public Reading NextReading()
    {
        return new Reading
        {
            Id = Guid.NewGuid(),
            Author = faker.Name.FullName(),
            Observation = faker.Lorem.Sentence(),
            Timestamp = RandomUnixTimestamp().ToString()
        };
    }
    public Comment NextComment(Guid targetId, bool valid)
    {
        return new Comment
        {
            Id = valid ? targetId : Guid.NewGuid(),
            CommentVal = faker.Lorem.Sentence(),
            Timestamp = RandomUnixTimestamp().ToString()
        };
    }
    public Proposal NextProposal(IReadOnlyList<Guid> knownIds)
    {
        return new Proposal(
            TaxonID: PickTaxonId(),
            Id: ReferenceId(knownIds),
            Timestamp: RandomUnixTimestamp().ToString()
        );
    }
    private Guid ReferenceId(IReadOnlyList<Guid> knownIds)
    {
        if(knownIds.Count > 0 && random.NextDouble() >= InvalidReferenceProbability)
            return knownIds[random.Next(knownIds.Count)];
        
        return Guid.NewGuid();
    }
    public long RandomUnixTimestamp()
    {
        return random.NextInt64(
            DateTimeOffset.UnixEpoch.ToUnixTimeMilliseconds(),
            DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
        );
    }
}