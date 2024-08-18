using Microsoft.AspNetCore.Identity;

namespace ZambeziDigital.Base.Services.Contracts.Auth;

public interface IRoleService
{ 
    List<IdentityRole> Roles { get; set; }
    Task<IdentityResult> CreateRole(string role);
    Task<IdentityResult> DeleteRole(string roleId);
    Task<IdentityRole> GetRole(string id);
    Task<List<IdentityRole>> GetRoles();
    Task<IdentityResult> UpdateRole(IdentityRole role);
}