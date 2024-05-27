global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using ZambeziDigital.Multitenancy.Data;

namespace ZambeziDigital.Multitenancy.Extensions;

public static class MultipleDatabaseExtensions
{
    public static IServiceCollection AddAndMigrateTenantDatabases(this IServiceCollection services,   IConfiguration configuration)
    {
        try
        {
            // Company Db Context (reference context) - get a list of tenants
            using IServiceScope scopeTenant = services.BuildServiceProvider().CreateScope();
            BaseDbContext baseDbContext = scopeTenant.ServiceProvider.GetRequiredService<BaseDbContext>();

            if (baseDbContext.Database.GetPendingMigrations().Any())
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Applying BaseDb Migrations.");
                Console.ResetColor();
                baseDbContext.Database.Migrate(); // apply migrations on baseDbContext
            }


            List<Tenant> tenantsInDb = baseDbContext.Tenants.ToList();

            string defaultConnectionString = configuration.GetConnectionString("DefaultConnection"); // read default connection string from appsettings.json
            TenantDbAccessGuard.TurnOff();
            foreach (Tenant tenant in tenantsInDb) // loop through all tenants, apply migrations on applicationDbContext
            {
               
                string connectionString = string.IsNullOrEmpty(tenant.ConnectionString) ? defaultConnectionString : tenant.ConnectionString;

                // Application Db Context (app - per tenant)
                using IServiceScope scopeApplication = services.BuildServiceProvider().CreateScope();
                TenantDbContext dbContext = scopeApplication.ServiceProvider.GetRequiredService<TenantDbContext>();
                dbContext.Database.SetConnectionString(connectionString);
                if (dbContext.Database.GetPendingMigrations().Any())
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"Applying Migrations for '{tenant.Id}' tenant.");
                    Console.ResetColor();
                    dbContext.Database.Migrate();
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

public static class TenantDbAccessGuard
{
    private static bool guard = false;
    public static bool SystemActive { get; set; } = false;

    public static void TurnOn()
    {
        if (SystemActive == true)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Be careful, System is active, you can't turn on Tenant Database Guard. Tenant Database Guard is off.");
            Console.ResetColor();
            guard = false;
            return;
        };
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Tenant Database Guard is turning off.");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Tenant Database Guard is off.");
        Console.ResetColor();
        guard = true;
    }
    
    public static void TurnOff()
    {
        if (SystemActive == true)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Becareful, System is active, you can't turn on Tenant Database Guard. Tenant Database Guard is off.");
            Console.ResetColor();
            guard = false;
            return;
        };
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Tenant Database Guard is turning on.");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Tenant Database Guard is on.");
        Console.ResetColor();
        guard = false;
    }
    
    public static bool Guard
    {
        get => guard;
    }
}
