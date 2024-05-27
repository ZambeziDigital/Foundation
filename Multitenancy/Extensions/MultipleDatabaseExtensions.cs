global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using ZambeziDigital.Multitenancy.Data;

namespace ZambeziDigital.Multitenancy.Extensions;
public static class MultipleDatabaseExtensions
{
    public static IServiceCollection AddAndMigrateTenantDatabases<TUser, TTenant>(
        this IServiceCollection services, 
        IConfiguration configuration, 
        IBaseDbContext<TUser, TTenant>  baseDbContext, 
        ITenantDbContext tenantDbContext)
        where TUser : IdentityUser, IHasKey<string>, IMustHaveTenant, new()
        where TTenant : class, ITenant, new()
    {
        try
        {
            // Company Db Context (reference context) - get a list of tenants
            using IServiceScope scopeTenant = services.BuildServiceProvider().CreateScope();
            if (baseDbContext.Database.GetPendingMigrations().Any())
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Applying BaseDb Migrations.");
                Console.ResetColor();
                baseDbContext.Database.Migrate(); // apply migrations on baseDbContext
            }


            List<TTenant> tenantsInDb = baseDbContext.Tenants.ToList();

            string defaultConnectionString = configuration.GetConnectionString("DefaultConnection"); // read default connection string from appsettings.json
            TenantDbAccessGuard.TurnOff();
            foreach (TTenant tenant in tenantsInDb) // loop through all tenants, apply migrations on applicationDbContext
            {
               
                string connectionString = string.IsNullOrEmpty(tenant.ConnectionString) ? defaultConnectionString : tenant.ConnectionString;

                // Application Db Context (app - per tenant)
                using IServiceScope scopeApplication = services.BuildServiceProvider().CreateScope();
                tenantDbContext.Database.SetConnectionString(connectionString);
                if (tenantDbContext.Database.GetPendingMigrations().Any())
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"Applying Migrations for '{tenant.Id}' tenant.");
                    Console.ResetColor();
                    tenantDbContext.Database.Migrate();
                }
            }
            TenantDbAccessGuard.TurnOn();
        }
        catch (Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(e);
            Console.ResetColor();
        }
        return services;
    }

}
