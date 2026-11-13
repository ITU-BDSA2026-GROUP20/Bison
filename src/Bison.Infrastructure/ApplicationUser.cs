using Microsoft.AspNetCore.Identity;
using Bison.Core.models;

namespace Bison.Infrastructure;

public class ApplicationUser : IdentityUser
{
    public int? AutherId {get; set;}
    public Author author {get; set;}
}