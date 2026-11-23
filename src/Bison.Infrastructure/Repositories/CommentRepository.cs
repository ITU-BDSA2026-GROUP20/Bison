
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
        IQueryable<Comment> query = database.comments.Include(c => c.Author);
        if (observationId is not null)
            query = query.Where(c => c.ObservationId == observationId);

        return await query.OrderBy(c => c.TimeStamp).ToListAsync();
    }
    public async Task<Comment> AddComment(Comment comment)
    {
        database.comments.Add(comment);
        await database.SaveChangesAsync();
        return comment;
    }
    public async Task AddComments(IEnumerable<Comment> comments)
    {
        database.comments.AddRange(comments);
        await database.SaveChangesAsync();
    }
}