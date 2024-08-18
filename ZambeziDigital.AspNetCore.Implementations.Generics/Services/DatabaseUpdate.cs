using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ZambeziDigital.AspNetCore.Implementations.Generics.Data;
using ZambeziDigital.Base.Contracts.Tenancy;
using ZambeziDigital.Base.Implementation;

namespace ZambeziDigital.AspNetCore.Implementations.Generics.Services;

public class BaseDatabaseUpdate<TUser, TTenant, TTenantContext, TBaseContext>(
    TBaseContext baseDbContext,
    TTenantContext tenantDbContext)
    where TBaseContext : IdentityDbContext<TUser>, IBaseDbContext<TUser, TTenant>
    where TTenantContext : DbContext//, ITenantDbContext
    where TUser : IdentityUser, IHasKey<string>, IMustHaveTenant, new()
    where TTenant : class, ITenant
{
    public void Migrate()
    {
        try
        {
                // AddAndMigrateTenantDatabases<ApplicationUser, Institution, BaseDbContext, InstitutionDataContext>(services);
            // Company Db Context (reference context) - get a list of tenants
           
            // BaseDbContext baseDbContext = serviceScopeFactory.ServiceProvider.GetRequiredService<BaseDbContext>();

            MigrateBase();
            List<TTenant> tenantsInDb = baseDbContext.Tenants.ToList();

            // string defaultConnectionString = configuration.GetConnectionString("DefaultConnection"); // read default connection string from appsettings.json
            TenantDbAccessGuard.TurnOff();
            foreach (TTenant tenant in tenantsInDb) // loop through all tenants, apply migrations on applicationDbContext
            {
                MigrateTenant(tenant);
            }
            TenantDbAccessGuard.TurnOn();
        }
        catch (Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(e);
            Console.ResetColor();
        }
    }
    
    void MigrateBase()
    {
        if (baseDbContext.Database.GetPendingMigrations().Any())
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Applying BaseDb Migrations.");
            Console.ResetColor();
            baseDbContext.Database.Migrate(); // apply migrations on baseDbContext
        }
    }
    
    void MigrateTenant(TTenant tenant)
    {
        // string defaultConnectionString = configuration.GetConnectionString("DefaultConnection"); // read default connection string from appsettings.json
        if(string.IsNullOrEmpty(tenant.ConnectionString)) throw new Exception("Tenant Connection String is null");
        string connectionString =  tenant.ConnectionString;

        // Application Db Context (app - per tenant)
        tenantDbContext.Database.SetConnectionString(connectionString);
        if (tenantDbContext.Database.GetPendingMigrations().Any())
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Applying Migrations for '{tenant.Id}' tenant.");
            Console.ResetColor();
            tenantDbContext.Database.Migrate();
        }
    }
}