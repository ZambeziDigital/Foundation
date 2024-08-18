using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ZambeziDigital.AspNetCore.Abstractions.Data;

public interface IBaseDbContext<TUser> 
    where TUser : IdentityUser, IHasKey<string>, new()
{
    DbSet<TUser> AspNetUsers { get; set; }
}

