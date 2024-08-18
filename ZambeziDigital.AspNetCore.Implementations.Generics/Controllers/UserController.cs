using MailKit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using ZambeziDigital.Base.DTOs.Auth;
using ZambeziDigital.Base.Models.Auth;
using ZambeziDigital.AspNetCore.Abstractions.Controllers.Authentication;
using ZambeziDigital.AspNetCore.Abstractions.Data;
using ZambeziDigital.Base.Models;
using ZambeziDigital.Base.Services.Contracts.Auth;

namespace ZambeziDigital.AspNetCore.Implementations.Generics.Controllers;

public class UserController<
    TApplicationUser,
    TContext,
    TUserInfo,
    TApplicationUserAddRequest,
    TLoginRequestDto>(
        UserManager<TApplicationUser> userManager,
        SignInManager<TApplicationUser> signInManager,
        IUserService<
            TApplicationUser,
            TApplicationUserAddRequest,
            TUserInfo,
            TLoginRequestDto> service,
        TContext context)
    : BaseController<
            TApplicationUser,
            string>(service),
        IUserController<TApplicationUser,
            TUserInfo,
            TLoginRequestDto>
    where TContext : DbContext, IBaseDbContext<TApplicationUser>
    where TApplicationUser : IdentityUser, IApplicationUser, new()
    where TUserInfo : class, IUserInfo, new()
    where TApplicationUserAddRequest : class, IApplicationUserAddRequest
    where TLoginRequestDto : class, ILoginRequestDto
    // where BaseResult : class, IBasicResult<TApplicationUser>
{
    [HttpGet("FindByEmailAsync/{email}")]
    public async Task<ActionResult<BaseResult<TApplicationUser>>> FindByEmailAsync(string email)
    {
        try
        {
            Log.Information("Finding user by email: {email}", email);
            var user = await userManager.FindByEmailAsync(email);
            if (user is null)
            {
                Log.Error("No user with email {email}", email);
                return new BadRequestObjectResult(new BaseResult
                {
                    Succeeded = false,
                    Errors = new() { $"No user with email {email}" },
                });
            }
            Log.Information("User found: {user}", user);
            return new OkObjectResult(user);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error finding user by email: {email}", email);
            Log.Error(ex, ex.Message);
            return new BadRequestObjectResult(new BaseResult() { Succeeded = false, Errors = [ex.Message] });
        }
    }

    [HttpGet("AssignRole/{Id}/{role}")]
    public async Task<ActionResult<BaseResult>> AssignRole(string Id, string role)
    {
        try
        {
            Log.Information("Assigning role {role} to user with Id {Id}", role, Id);
            var user = await userManager.FindByIdAsync(Id);
            if (user is null)
            {
                Log.Error("No user with Id {Id}", Id);
                return new BadRequestObjectResult(new BaseResult()
                {
                    Succeeded = false,
                    Errors = new() { $"No user with Id {Id}" }
                });
            }
            Log.Information("User found: {user}", user);
            Log.Information("Assigning role {role} to user", role);
            var result = await userManager.AddToRoleAsync(user, role);
            if (result.Succeeded)
            {
                Log.Information("Role {role} assigned to user", role);
                return new OkObjectResult(new BaseResult()
                {
                    Succeeded = true,
                    Data = user
                });
            }
            else
            {
                Log.Error("Error assigning role: {errors}", result.Errors);
                return new BadRequestObjectResult(new BaseResult()
                {
                    Succeeded = false,
                    Errors = result.Errors.Select(x => x.Description).ToList()
                });
            }

        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error assigning role: {role} to user with Id {Id}", role, Id);
            return new BadRequestObjectResult(new BaseResult()
            {
                Succeeded = false,
                Errors = new() { $"Error assigning role: {ex.Message}" }
            });
        }
    }

    [HttpGet("Manage/Info")]
    public async Task<ActionResult<TUserInfo>> GetUserInfo()
    {
        try
        {
            Log.Information("Getting user information");
            var user = await userManager.GetUserAsync(User);
            Log.Information("User found: {user}", user);
            if (user == null) throw new Exception($"No {nameof(TApplicationUser)} with {User.Identity.Name}");
            Log.Information("Getting user roles");
            user.Roles = (await userManager.GetRolesAsync(user)).Select(r => new IdentityRole(r)).ToList();
            Log.Information("User roles found: {roles}", user.Roles);
            var userInformation = new TUserInfo()
            {
                Email = user.Email,
                UserId = user.Id,
                Name = user.Name,
                Roles = user.Roles.Select(r => r.Name).ToList() ?? new List<string>()
            };
            Log.Information("User information: {userInformation}", userInformation);
            return new OkObjectResult(userInformation);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error getting user information");
            return new UnauthorizedObjectResult(new BaseResult() { Succeeded = false, Errors = new() { ex.Message } });
        }
    }


    [HttpPost("Login")]
    public async Task<ActionResult<BaseResult>> Login(TLoginRequestDto loginDto)
    {
        try
        {
            Log.Information("Logging in user with email {email}", loginDto.Email);
            var user = context.Set<TApplicationUser>().AsNoTracking().FirstOrDefault(x => x.Email == loginDto.Email);
            if (user is null)
            {
                Log.Error("No {TApplicationUser} with email {email}", nameof(TApplicationUser), loginDto.Email);
                return new UnauthorizedObjectResult(new BaseResult()
                {
                    Succeeded = false,
                    Errors = new() { $"No {nameof(TApplicationUser)} with email {loginDto.Email}" }
                });
            }
            Log.Information("User found: {user}", user);
            var result = await signInManager
                .PasswordSignInAsync(user.UserName, loginDto.Password, true, true);
            Log.Information("Login result: {result}", result);
            if (result.Succeeded)
            {
                Log.Information("User logged in: {user}", user);
                return new OkObjectResult(new BaseResult()
                {
                    Succeeded = true,
                    Data = user
                });
            }
            Log.Error("Error logging in user: {result}", result);
            return new UnauthorizedObjectResult(new BaseResult
            {
                Succeeded = false,
                Errors = [result.ToString()]
            });
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error logging in user with email {email}", loginDto.Email);
            return new BadRequestObjectResult(ex.Message);
        }
    }

    [HttpPost("Logout")]
    [AllowAnonymous]
    [EnableCors]
    public async Task<ActionResult<BaseResult>> Logout()
    {
        try
        {
            Log.Information("Logging out user");
            await signInManager.SignOutAsync();
            Log.Information("User logged out");
            //TODO: Set user to null
            return new OkObjectResult(new BaseResult() { Succeeded = true });
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error logging out user");
            return new BadRequestObjectResult(new BaseResult()
            {
                Succeeded = false,
                Errors = new() { ex.Message }
            });
        }
    }
}

public class UserController<
    TApplicationUser,
    TContext,
    TUserInfo,
    TApplicationUserAddRequest,
    TForgotPasswordRequest,
    TResetPasswordRequest,
    TLoginRequestDto>(
        UserManager<TApplicationUser> userManager,
        SignInManager<TApplicationUser> signInManager,
        IUserService<
            TApplicationUser,
            TApplicationUserAddRequest,
            TUserInfo,
            TForgotPasswordRequest,
            TResetPasswordRequest,
            TLoginRequestDto> service,
            TContext context)
    : BaseController<
            TApplicationUser,
            string>(service),
            IUserController<
                TApplicationUser,
                TUserInfo,
                TLoginRequestDto>
    where TLoginRequestDto : class, ILoginRequestDto
    where TContext : DbContext, IBaseDbContext<TApplicationUser>
    where TApplicationUser : IdentityUser, IApplicationUser, new()
    where TUserInfo : class, IUserInfo, new()
    where TApplicationUserAddRequest : class, IApplicationUserAddRequest
    // where BaseResult : class, IBasicResult<TApplicationUser>
{
    [HttpGet("FindByEmailAsync/{email}")]
    public async Task<ActionResult<BaseResult<TApplicationUser>>> FindByEmailAsync(string email)
    {
        try
        {
            Log.Information("Finding user by email: {email}", email);
            var user = await userManager.FindByEmailAsync(email);
            if (user is null)
            {
                Log.Error("No user with email {email}", email);
                return new BadRequestObjectResult(new BaseResult
                {
                    Succeeded = false,
                    Errors = new() { $"No user with email {email}" },
                });
            }
            Log.Information("User found: {user}", user);
            return new OkObjectResult(user);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error finding user by email: {email}", email);
            return new BadRequestObjectResult(new BaseResult() { Succeeded = false, Errors = [ex.Message] });
        }
    }

    [HttpGet("AssignRole/{Id}/{role}")]
    public async Task<ActionResult<BaseResult>> AssignRole(string Id, string role)
    {
        try
        {
            Log.Information("Assigning role {role} to user with Id {Id}", role, Id);
            var user = await userManager.FindByIdAsync(Id);
            if (user is null)
            {
                Log.Error("No user with Id {Id}", Id);
                return new BadRequestObjectResult(new BaseResult()
                {
                    Succeeded = false,
                    Errors = new() { $"No user with Id {Id}" }
                });
            }
            Log.Information("User found: {user}", user);
            var result = await userManager.AddToRoleAsync(user, role);
            Log.Information("Assigning role {role} to user", role);
            if (result.Succeeded)
            {
                Log.Information("Role {role} assigned to user", role);
                return new OkObjectResult(new BaseResult()
                {
                    Succeeded = true,
                    Data = user
                });
            }
            else
            {
                Log.Error("Error assigning role: {errors}", result.Errors);
                return new BadRequestObjectResult(new BaseResult()
                {
                    Succeeded = false,
                    Errors = result.Errors.Select(x => x.Description).ToList()
                });
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error assigning role: {role} to user with Id {Id}", role, Id);
            return new BadRequestObjectResult(new BaseResult()
            {
                Succeeded = false,
                Errors = new() { $"Error assigning role: {ex.Message}" }
            });
        }
    }

    [HttpGet("Manage/Info")]
    public async Task<ActionResult<TUserInfo>> GetUserInfo()
    {
        try
        {
            Log.Information("Getting user information");
            var user = await userManager.GetUserAsync(User);
            Log.Information("User found: {user}", user);
            if (user == null) throw new Exception($"No {nameof(TApplicationUser)} with {User.Identity.Name}");
            user.Roles = (await userManager.GetRolesAsync(user)).Select(r => new IdentityRole(r)).ToList();
            Log.Information("User roles found: {roles}", user.Roles);
            var userInformation = new TUserInfo()
            {
                Email = user.Email,
                UserId = user.Id,
                Name = user.Name,
                Roles = user.Roles.Select(r => r.Name).ToList() ?? new List<string>()
            };
            Log.Information("User information: {userInformation}", userInformation);
            return new OkObjectResult(userInformation);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error getting user information");
            return new UnauthorizedObjectResult(new BaseResult() { Succeeded = false, Errors = new() { ex.Message } });
        }
    }


    [HttpPost("Login")]
    public async Task<ActionResult<BaseResult>> Login(TLoginRequestDto loginDto)
    {
        try
        {
            Log.Information("Logging in user with email {email}", loginDto.Email);
            var user = context.Set<TApplicationUser>().AsNoTracking().FirstOrDefault(x => x.Email == loginDto.Email);
            if (user is null)
            {
                Log.Error("No {TApplicationUser} with email {email}", nameof(TApplicationUser), loginDto.Email);
                return new UnauthorizedObjectResult(new BaseResult()
                {
                    Succeeded = false,
                    Errors = new() { $"No {nameof(TApplicationUser)} with email {loginDto.Email}" }
                });
            }
            Log.Information("User found: {user}", user);
            var result = await signInManager
                .PasswordSignInAsync(user.UserName, loginDto.Password, true, true);
            if (result.Succeeded)
            {
                Log.Information("User logged in: {user}", user);
                return new OkObjectResult(new BaseResult()
                {
                    Succeeded = true,
                    Data = user
                });
            }
            Log.Error("Error logging in user: {result}", result);
            return new UnauthorizedObjectResult(new BaseResult
            {
                Succeeded = false,
                Errors = [result.ToString()]
            });
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error logging in user with email {email}", loginDto.Email);
            return new BadRequestObjectResult(ex.Message);
        }
    }

    [HttpPost("Logout")]
    [AllowAnonymous]
    [EnableCors]
    public async Task<ActionResult<BaseResult>> Logout()
    {
        try
        {
            Log.Information("Logging out user");
            await signInManager.SignOutAsync();
            //TODO: Set user to null
            Log.Information("User logged out");
            return new OkObjectResult(new BaseResult() { Succeeded = true });
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error logging out user");
            return new BadRequestObjectResult(new BaseResult()
            {
                Succeeded = false,
                Errors = new() { ex.Message }
            });
        }
    }
}


