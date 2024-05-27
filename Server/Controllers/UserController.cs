using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ZambeziDigital.Authentication.Data;
using ZambeziDigital.Authentication.DataTransferObjects;
using ZambeziDigital.Authentication.Models;
using ZambeziDigital.Authentication.Services.Contracts;

namespace ZambeziDigital.Server.Controllers;

public class UserController(IUserService service, IServiceScopeFactory _scopeFactory)
    : BaseController<ApplicationUser, string>(service)
{
    [HttpGet("FindByEmailAsync/{email}")]
    public virtual async Task<ActionResult<ApplicationUser>> FindByEmailAsync(string email)
    {
        try
        {
            var userManager = _scopeFactory.CreateScope().ServiceProvider
                .GetRequiredService<UserManager<ApplicationUser>>();
            var user = await userManager.FindByEmailAsync(email);
            if (user is null)
                return new BadRequestObjectResult(new BasicResult()
                    { Succeeded = false, Errors = new() { $"No user with email {email}" } });
            return new OkObjectResult(user);
        }
        catch (Exception ex)
        {
            return new BadRequestObjectResult(new BasicResult() { Succeeded = false, Errors = [ex.Message] });
        }
    }


    [HttpPost("GetUserIdAsync")]
    public virtual  async Task<ActionResult<string>> GetUserIdAsync(ApplicationUser user)
    {
        try
        {
            var userManager = _scopeFactory.CreateScope().ServiceProvider
                .GetRequiredService<UserManager<ApplicationUser>>();
            var result = await userManager.GetUserIdAsync(user);
            return new OkObjectResult(result);
        }
        catch (Exception ex)
        {
            return new BadRequestObjectResult(new BasicResult()
                { Succeeded = false, Errors = new() { $"Error getting user Id: {ex.Message}" } });
        }
    }

    [HttpGet("AssignRole/{Id}/{role}")]
    public  virtual async Task<ActionResult<BasicResult>> AssignRole(string Id, string role)
    {
        try
        {
            var userManager = _scopeFactory.CreateScope().ServiceProvider
                .GetRequiredService<UserManager<ApplicationUser>>();
            var user = await userManager.FindByIdAsync(Id);
            if (user is null)
                return new BadRequestObjectResult(new BasicResult()
                    { Succeeded = false, Errors = new() { $"No user with Id {Id}" } });
            var result = await userManager.AddToRoleAsync(user, role);
            return result.Succeeded
                ? new OkObjectResult(new BasicAccess.Models.BasicResult<ApplicationUser>() { Succeeded = true, Object = user })
                : new BadRequestObjectResult(new BasicResult()
                    { Succeeded = false, Errors = result.Errors.Select(x => x.Description).ToList() });
        }
        catch (Exception ex)
        {
            return new BadRequestObjectResult(new BasicResult()
                { Succeeded = false, Errors = new() { $"Error assigning role: {ex.Message}" } });
        }
    }


    [HttpGet("Manage/Info")]
    public virtual  async Task<ActionResult<UserInfo>> GetUserInfo()
    {
        try
        {
            // var _dataContext = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<TenantDbContext>();
            // var roleManager = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = _scopeFactory.CreateScope().ServiceProvider
                .GetRequiredService<UserManager<ApplicationUser>>();
            var user = await userManager.GetUserAsync(User);
            if (user == null) throw new Exception($"No {nameof(ApplicationUser)} with {User.Identity.Name}");
            user.Roles = (await userManager.GetRolesAsync(user)).Select( r => new IdentityRole(r)).ToList();
            var userInformation = new UserInfo(user);
            return new OkObjectResult(userInformation);
        }
        catch (Exception ex)
        {
            return new UnauthorizedObjectResult(new BasicResult() { Succeeded = false, Errors = new() { ex.Message } });
        }
    }

    //should return cookie stuff in the header
    [HttpPost("Login")]
    public virtual  async Task<ActionResult<BasicResult>> Login(LoginRequestDto loginDto)
    {
        try
        {
            var dataContext = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<BaseDbContext>();
            ApplicationUser user = dataContext.Set<ApplicationUser>().AsNoTracking().FirstOrDefault(x => x.Email == loginDto.Email);
            if (user is null)
                return new UnauthorizedObjectResult(new BasicResult
                {
                    Succeeded = false, Errors = new() { $"No {nameof(ApplicationUser)} with email {loginDto.Email}" }
                });
            var result = await _scopeFactory.CreateScope().ServiceProvider
                .GetRequiredService<SignInManager<ApplicationUser>>()
                .PasswordSignInAsync(user.UserName, loginDto.Password, true, true);
            if (result.Succeeded)
                return new OkObjectResult(new BasicAccess.Models.BasicResult<ApplicationUser> { Succeeded = true, Object = user });
            return new UnauthorizedObjectResult(new BasicResult
            {
                Succeeded = false, Errors =
                    [result.ToString()]
            });
        }
        catch (Exception ex)
        {
            return new BadRequestObjectResult(ex.Message);
        }
    }

    [HttpPost("Logout")]
    [AllowAnonymous]
    [EnableCors]
    public virtual  async Task<ActionResult<BasicResult>> Logout()
    {
        try
        {
            await _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<SignInManager<ApplicationUser>>()
                .SignOutAsync();
            return new OkObjectResult(new BasicResult() { Succeeded = true });
        }
        catch (Exception ex)
        {
            return new BadRequestObjectResult(new BasicResult() { Succeeded = false, Errors = new() { ex.Message } });
        }
    }
}
