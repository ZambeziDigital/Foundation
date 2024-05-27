global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ZambeziDigital.Multitenancy.Data
{
    public interface ITenantDbContext
    {
        // DbSet<TAttachment> Attachments { get; set; }
        // DbSet<TAddress> Addresses { get; set; }

        int SaveChanges();
        void OnConfiguring(DbContextOptionsBuilder optionsBuilder);
        DatabaseFacade Database { get; } // Add this line
    }
}
