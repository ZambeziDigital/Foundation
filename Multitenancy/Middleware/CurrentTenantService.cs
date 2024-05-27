namespace ZambeziDigital.Multitenancy.Middleware;
public interface ICurrentTenantService
{
    public string? ConnectionString { get; set; }
    int TenantId { get; set; }
    public Task<bool> SetTenant(int tenant);
    Task<bool> SetUser(string userFromHeader);
    
    string UserId { get; set; }
    ApplicationUser User { get; set; }
}
public class CurrentTenantService(IServiceScopeFactory serviceScopeFactory) : ICurrentTenantService
{
    public string? ConnectionString { get; set; }
    public int TenantId { get; set; }

    public async Task<bool> SetTenant(int tenant)
    {
        try
        {
            var context = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<BaseDbContext>();
            var tenantInfo = await context.Tenants.Where(x => x.Id == tenant).FirstOrDefaultAsync(); // check if tenant exists
            if (tenantInfo != null)
            {
                TenantId = tenant;
                ConnectionString = tenantInfo.ConnectionString;
                return true;
            }
            else
            {
                throw new Exception("Tenant invalid"); 
            }
        }catch(Exception e)
        {
            throw new Exception("Tenant invalid");
        }
       

    }

    public async Task<bool> SetUser(string userFromHeader)
    {
        try
        {
            
            UserId = userFromHeader;
            var context = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<BaseDbContext>();
            var user = context.AspNetUsers.FirstOrDefault(x => x.Id == UserId); // check if user exists
            if (user != null)
            {
                UserId = user.Id;
                User = user;
                return true;
            }
            else
            {
                throw new Exception("User invalid"); 
            }
        }catch(Exception e)
        {
            throw new Exception("User invalid");
        }
    }

    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
}