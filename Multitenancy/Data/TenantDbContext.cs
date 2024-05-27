global using System.Data;
global using Microsoft.EntityFrameworkCore;
global using Server.Middleware;
using System.Net.Mail;
using MySqlConnector;
using ZambeziDigital.Multitenancy.Extensions;
using ZambeziDigital.Multitenancy.Middleware;
using ZambeziDigital.Multitenancy.Models.Contracts;

namespace ZambeziDigital.Multitenancy.Data
{
    public partial class TenantDbContext(ICurrentTenantService currentTenantService, DbContextOptions<TenantDbContext> options) : DbContext(options)
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (TenantDbAccessGuard.Guard)
                if (currentTenantService.TenantId == null ||  currentTenantService.TenantId == 0)
                {
                    throw new DataException(
                        "You have own a Database to use this service, if you are switching users or tenants, please make sure to set the Tenant Id and User Id");
                }

            if (!string.IsNullOrEmpty(currentTenantService.ConnectionString)) // use tenant db if one is specified
            {
                //check if the database exists
                try
                {
                    using var connection = new MySqlConnection(currentTenantService.ConnectionString);
                    connection.Open();
                    connection.Close();
                }
                catch (Exception e)
                {
                    throw new DataException("Database does not exist");
                }
                
                
                _ = optionsBuilder.UseMySql(currentTenantService.ConnectionString,
                    new MySqlServerVersion(new Version(8, 0, 21)));
            }


        }

        // On Save Changes - write tenant Id to table
        public override int SaveChanges()
        {        
            foreach (var entry in ChangeTracker.Entries<IMustHaveCompany>().ToList()) 
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                    case EntityState.Modified:
                        entry.Entity.CompanyId = currentTenantService.TenantId; 
                        break;
                }
            }
            var result = base.SaveChanges();
            return result;
        }

        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<Address> Addresses { get; set; }
    }
}
