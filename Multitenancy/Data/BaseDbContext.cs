using System.Net.Mail;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using ZambeziDigital.Multitenancy.Models.Base;

namespace ZambeziDigital.Multitenancy.Data;

public interface IBaseDbContext<TUser, TTenant> 
    where TUser : IdentityUser, IHasKey<string>,IMustHaveTenant, new()
    where TTenant : class, ITenant
{
    DbSet<TTenant> Tenants { get; set; }
    DbSet<TUser> AspNetUsers { get; set; }

    int SaveChanges();
    void OnConfiguring(DbContextOptionsBuilder optionsBuilder);
    DatabaseFacade Database { get; } // Add this line
}