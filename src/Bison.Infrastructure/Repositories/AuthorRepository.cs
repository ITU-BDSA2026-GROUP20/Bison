using Bison.Core.models;
using Bison.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Bison.Infrastructure.Repositories;
public class AuthorRepository(Database database) : IAuthorRepository
{
    public async Task<Author> GetOrCreateAuthor(int? authorId, string name, string email)
    {
        Author? author = authorId is int id ? await database.users.FindAsync(id) : null;
        author ??= await database.users.FirstOrDefaultAsync(a => a.Email == email);
        if (author is null)
        {
            author = new Author { Name = name, Email = email };
            database.users.Add(author);
            await database.SaveChangesAsync();
        }
        return author;
    }
}