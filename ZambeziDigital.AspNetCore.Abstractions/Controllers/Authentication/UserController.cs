using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using ZambeziDigital.AspNetCore.Abstractions.Data;
using ZambeziDigital.Base.DTOs.Auth;
using ZambeziDigital.Base.Models.Auth;

namespace ZambeziDigital.AspNetCore.Abstractions.Controllers.Authentication;

public interface IUserController<TApplicationUser, TUserInfo, TLoginRequestDto>
    : IBaseController<TApplicationUser, string>
    where TApplicationUser : IdentityUser, IApplicationUser, new()
    where TUserInfo : class, IUserInfo
{
    [HttpGet("FindByEmailAsync/{email}")]
    Task<ActionResult<BaseResult<TApplicationUser>>> FindByEmailAsync(string email);

    // [HttpPost("GetUserIdAsync")]
    // Task<ActionResult<TResult>> GetUserIdAsync(TApplicationUser user);

    [HttpGet("AssignRole/{Id}/{role}")]
    Task<ActionResult<BaseResult>> AssignRole(string Id, string role);


    [HttpGet("Manage/Info")]
    Task<ActionResult<TUserInfo>> GetUserInfo();

    //should return cookie stuff in the header
    [HttpPost("Login")]
    Task<ActionResult<BaseResult>> Login(TLoginRequestDto loginDto);

    [HttpPost("Logout")]
    [AllowAnonymous]
    [EnableCors]
    Task<ActionResult<BaseResult>> Logout();
}
