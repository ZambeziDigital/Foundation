global using System.Data;
global using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MySqlConnector;
using ZambeziDigital.Multitenancy.Extensions;
using ZambeziDigital.Multitenancy.Middleware;
using ZambeziDigital.Multitenancy.Models.Contracts;

namespace ZambeziDigital.Multitenancy.Data
{
    public interface ITenantDbContext
    {
        // DbSet<TAttachment> Attachments { get; set; }
        // DbSet<TAddress> Addresses { get; set; }

        int SaveChanges();
        //protected void OnConfiguring(DbContextOptionsBuilder optionsBuilder);
        DatabaseFacade Database { get; } // Add this line
    }
}
