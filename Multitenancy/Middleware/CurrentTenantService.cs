global using Microsoft.AspNetCore.Identity;

namespace ZambeziDigital.Multitenancy.Middleware;
public interface ICurrentTenantService<TUser> where TUser : IdentityUser, IHasKey<string>,IMustHaveTenant, new() 
{
    public string? ConnectionString { get; set; }
    int? TenantId { get; set; }
    public Task<bool> SetTenant(int tenant);
    Task<bool> SetUser(string userFromHeader);
    string? UserId { get; set; }
    TUser? User { get; set; }
}
public class CurrentTenantService<TUser, TTenant> (IBaseDbContext<TUser, TTenant> baseDbContext) : ICurrentTenantService<TUser> 
    where TUser : IdentityUser, IHasKey<string>, IMustHaveTenant, new()
    where TTenant : class, ITenant
{
    public string? ConnectionString { get; set; }
    public int? TenantId { get; set; }

    public async Task<bool> SetTenant(int tenant)
    {
        try
        {
            var tenantInfo = await baseDbContext.Tenants.Where(x => x.Id == tenant).FirstOrDefaultAsync(); // check if tenant exists
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
            TUser? user = baseDbContext.AspNetUsers.FirstOrDefault(x => x.Id == UserId); // check if user exists
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
    public TUser? User { get; set; }
}