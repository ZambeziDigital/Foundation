using ZambeziDigital.Base.Models.Auth;

namespace ZambeziDigital.Base.Services.Contracts.Auth;


public interface IUserServiceBase<TUser, TUserInfo, TLoginRequestDto> 
    where TUser : class, IApplicationUser, new()
    where TUserInfo : class, IUserInfo
    where TLoginRequestDto : class, ILoginRequestDto
{
    Task<BaseResult<TUser?>> FindByEmailAsync(string email);
    Task<BaseResult<TUser>> RequestPasswordReset(IForgotPasswordRequest request);
    Task<BaseResult<TUser>> ResetPasswordRequest(IResetPasswordRequest request);
    Task<BaseResult<TUser>> Login(TLoginRequestDto loginDto);
    // Task<TUser> Create(TUserAdd userAddRequest);
    Task<BaseResult> Logout(string? page = null);
    Task<BaseResult> AssignRole(string userId, string role);
    TUser? CurrentUser { get; set; }
    Task<BaseResult> AddToRoleAsync(string Id, string role);
    TUserInfo? BasicInfo { get; set; }
}
public interface IUserServiceBase<TUser, TUserInfo, TForgotPasswordRequest, TResetPasswordRequest, TLoginRequestDto> 
    where TUser : class, IApplicationUser, new()
    where TUserInfo : class, IUserInfo
{
    Task<BaseResult<TUser?>> FindByEmailAsync(string email);
    Task<BaseResult> RequestPasswordReset(TForgotPasswordRequest request);
    Task<BaseResult> ResetPasswordRequest(TResetPasswordRequest request);
    Task<BaseResult> Login(TLoginRequestDto loginDto);
    // Task<TUser> Create(TUserAdd userAddRequest);
    Task<BaseResult> Logout(string? page = null);
    Task<BaseResult> AssignRole(string userId, string role);
    TUser? CurrentUser { get; set; }
    Task<BaseResult> AddToRoleAsync(string Id, string role);
    TUserInfo? BasicInfo { get; set; }
}

public interface IUserService<TUser, TUserInfo, TLoginRequestDto> : IBaseService<TUser, string>, IUserServiceBase<TUser, TUserInfo, TLoginRequestDto>
    where TUser : class, IApplicationUser, new()
    where TUserInfo : class, IUserInfo
    where TLoginRequestDto : class, ILoginRequestDto;

public interface IUserService<TUser, TUserInfo, TForgotPasswordRequest, TResetPasswordRequest, TLoginRequestDto> : IBaseService<TUser, string>, 
    IUserServiceBase<TUser, TUserInfo, TForgotPasswordRequest, TResetPasswordRequest, TLoginRequestDto>
    where TUser : class, IApplicationUser, new()
    where TUserInfo : class, IUserInfo;

public interface IUserService<TUser, TUserAdd, TUserInfo, TLoginRequestDto> : IBaseService<TUser, TUserAdd, string>, IUserServiceBase<TUser, TUserInfo, TLoginRequestDto>
    where TUser : class, IApplicationUser, new()
    where TUserAdd : class, IApplicationUserAddRequest
    where TUserInfo : class, IUserInfo
    where TLoginRequestDto : class, ILoginRequestDto;

public interface IUserService<TUser, TUserAdd, TUserInfo, TForgotPasswordRequest, TResetPasswordRequest, TLoginRequestDto> 
    : IBaseService<TUser, TUserAdd, string>, IUserServiceBase<TUser, TUserInfo, TForgotPasswordRequest, TResetPasswordRequest, TLoginRequestDto>
    where TUser : class, IApplicationUser, new()
    where TUserAdd : class, IApplicationUserAddRequest
    where TUserInfo : class, IUserInfo;