namespace ZambeziDigital.Authentication.Services.Server;

public class RoleService(IServiceScopeFactory serviceScopeFactory) : IRoleService
{
    public async Task<IdentityResult> CreateRole(string role)
    {
        var roleManager = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        return await roleManager.CreateAsync(new IdentityRole(role));
        
    }

    public async Task<IdentityResult> DeleteRole(string roleId)
    {
        var roleManager = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var role = await roleManager.FindByIdAsync(roleId);
        return await roleManager.DeleteAsync(role);
    }

    public async Task<IdentityRole> GetRole(string id)
    {
        var roleManager = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        return await roleManager.FindByIdAsync(id);
    }

    public async Task<List<IdentityRole>> GetRoles()
    {
        var roleManager = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        return roleManager.Roles.ToList();
    }

    public async Task<IdentityResult> UpdateRole(IdentityRole role)
    {
        var roleManager = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        return await roleManager.UpdateAsync(role);
    }
}
