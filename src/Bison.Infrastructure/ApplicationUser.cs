using Microsoft.AspNetCore.Identity;
using Bison.Core.models;

namespace Bison.Infrastructure;

public class ApplicationUser : IdentityUser
{
    public int? AuthorId { get; set; }
    public Author? Author { get; set; }
}
