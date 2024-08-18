using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ZambeziDigital.AspNetCore.Implementations.Generics.Data
{
    public interface ITenantDbContext
    {
        // DbSet<TAttachment> Attachments { get; set; }
        // DbSet<TAddress> Addresses { get; set; }

        // int SaveChanges();
        //protected void OnConfiguring(DbContextOptionsBuilder optionsBuilder);
        DatabaseFacade Database { get; } // Add this line
    }
}
