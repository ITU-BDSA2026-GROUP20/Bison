using Bison.Core.models;

namespace Bison.Core.Repositories;


public interface ICommentRepository
{
    Task<IReadOnlyList<Comment>> GetComments(int? id);
    Task<Comment> AddComment(Comment comment);
}