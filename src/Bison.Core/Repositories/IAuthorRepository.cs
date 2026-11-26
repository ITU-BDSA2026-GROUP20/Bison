using Bison.Core.models;
namespace Bison.Core.Repositories;

public interface IAuthorRepository
{
    Task<Author> GetOrCreateAuthor(int? authorId, string name, string email);
    Task<Author?> GetAuthorById(int authorId);
}