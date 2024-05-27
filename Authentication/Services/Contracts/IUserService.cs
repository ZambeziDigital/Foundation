global using ZambeziDigital.Authentication.DataTransferObjects;
global using ZambeziDigital.Authentication.Models;
global using ZambeziDigital.BasicAccess.Services.Contracts;

namespace ZambeziDigital.Authentication.Services.Contracts;


public interface IUserService : IBaseService<ApplicationUser, string>
{
    Task<ApplicationUser?> FindByEmailAsync(string email);
    Task<BasicResult> RequestPasswordReset(ForgotPasswordRequest request);
    Task<BasicResult> ResetPasswordRequest(ResetPasswordRequest request);
    Task<BasicResult<ApplicationUser>> Login(LoginRequestDto loginDto);
    Task<ApplicationUser> Create(ApplicationUserAddRequest userAddRequest);
    Task<BasicResult> Logout(string? page = null);
    Task<BasicResult> AssignRole(string userId, string role);
    ApplicationUser? CurrentUser { get; set; }
    Task<BasicResult> AddToRoleAsync(string Id, string role);
    UserInfo? BasicInfo { get; set; }
    
        
}