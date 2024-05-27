namespace ZambeziDigital.Authentication.Services.Contracts;

public interface IRoleService
{ 
    Task<IdentityResult> CreateRole(string role);
    Task<IdentityResult> DeleteRole(string roleId);
    Task<IdentityRole> GetRole(string id);
    Task<List<IdentityRole>> GetRoles();
    Task<IdentityResult> UpdateRole(IdentityRole role);
}