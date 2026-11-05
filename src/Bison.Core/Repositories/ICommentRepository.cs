using Bison.Core.models;

namespace Bison.Core.Repositories;


public interface ICommentRepository
{
    Task<IReadOnlyList<Comment>> GetComments(int? id);
    Task AddComment(Comment comment);
}