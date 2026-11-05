
using Bison.Core.models;
using Microsoft.EntityFrameworkCore;
using Bison.Core.Repositories;

namespace Bison.Infrastructure.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly Database database; 
    public CommentRepository(Database database)
    {
        this.database = database;
    }
    public async Task<IReadOnlyList<Comment>> GetComments(int? observationId)
    {
        if(observationId is null)
        return await database.comments.ToListAsync();

        return await database.comments.Where(c=> c.ObservationId == observationId).ToListAsync();
    }
    public async Task AddComment(Comment comment)
    {
        database.comments.Add(comment);
        await database.SaveChangesAsync();
    }
    public async Task AddComments(IEnumerable<Comment> comments)
    {
        database.comments.AddRange(comments);
        await database.SaveChangesAsync();
    }
}