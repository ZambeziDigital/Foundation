using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ZambeziDigital.Base.Accounting.Models;
using ZambeziDigital.Base.Implementation.Models;
using ZambeziDigital.Management.Server.Models;

namespace ZambeziDigital.Management.Server.Data;

public class DataContext(DbContextOptions<DataContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<SAAS> Saases { get; set; }
}