using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ZambeziDigital.AspNetCore.Abstractions.Data;
using ZambeziDigital.Base.DTOs.Auth;
using ZambeziDigital.Base.Models.Auth;
using ZambeziDigital.Base.Services.Contracts.Auth;
using ZambeziDigital.Base.Services.Contracts.Mail;
using ForgotPasswordRequest = Microsoft.AspNetCore.Identity.Data.ForgotPasswordRequest;
using ResetPasswordRequest = Microsoft.AspNetCore.Identity.Data.ResetPasswordRequest;

namespace ZambeziDigital.AspNetCore.Implementations.Generics.Services;

public class UserService<TUser, TUserAdd, TUserInfo, TContext, TLoginRequest>(
    IServiceScopeFactory serviceScopeFactory,
    IMailService<BaseResult> mailService,
    TContext context) :
    BaseService<TUser, string, TContext>(context),
    IUserService<TUser, TUserAdd, TUserInfo, TLoginRequest>
    where TContext : DbContext, IBaseDbContext<TUser>
    where TUser : IdentityUser, IApplicationUser, ISearchable, new()
    where TUserInfo : class, IUserInfo
    where TUserAdd : class, IApplicationUserAddRequest
    where TLoginRequest : class, ILoginRequestDto
{
    public List<TUser> Objects { get; set; }

    public virtual async Task<BaseResult<List<TUser>>> Get(bool paged = false, int page = 0, int pageSize = 10, bool cached = false)
    {
        if (paged)
            Objects = await context.Set<TUser>().Include(x => x.Roles).Skip(page * pageSize).Take(pageSize)
                .ToListAsync();
        else
            Objects = await context.Set<TUser>().Include(x => x.Roles).ToListAsync();
        return new BaseResult<List<TUser>>() { Data = Objects };
    }

    public virtual async Task<BaseResult<TUser>> Get(string id)
    {
        // throw new NotImplementedException();
        var user = (await Get(false)).Data?.FirstOrDefault(u => u.Id == id) ??
                   (await Get(true)).Data?.FirstOrDefault(u => u.Id == id) ??
                   throw new Exception("User not found");
        return new BaseResult<TUser>() { Data = user };
    }

    public virtual async Task<BaseResult<TUser>> Update(TUser t)
    {
        var result = context.Set<TUser>().Update(t);
        await context.SaveChangesAsync();
        return new BaseResult<TUser>() { Data = t };
    }

    public virtual async Task<BaseResult<TUser>> Create(TUser dto)
    {


        // throw new NotImplementedException();
        //make sure to select the correct context for the user manager
        // var context = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<TContext>();
        var userMgr = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<UserManager<TUser>>();

        // var userMgr = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<UserManager<TUser>>();
        var result = await userMgr.CreateAsync(dto, dto.Password);
        if (!result.Succeeded)
        {
            return new BaseResult<TUser>()
            {
                Errors = result.Errors.ToList().Select(e => e.Description).ToList(),
                Message = "User not created",
                Succeeded = false,
            };
        }

        //Mail the new User the password
        var mail = new MailRequest
        {
            ToEmails = [dto.Email],
            Subject = "New User Account",
            Body =
                $"Dear {dto.UserName}, <br> Your account has been created successfully. <br> Your username is {dto.UserName} <br> Your password is {dto.Password} <br> Please change your password after login. <br> Regards, <br> System Administrator"
        };
        var mailresult = await mailService.SendMail(mail);
        return new BaseResult<TUser>()
        {
            Data = dto,
            Message = "User created" + (mailresult.Succeeded ? " and email sent" : " but email not sent"),
            Succeeded = true,
        };
    }

    public virtual async Task<BaseResult> Delete(string id)
    {
        // throw new NotImplementedException();
        var context = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<TContext>();
        var user = await context.Set<TUser>().FindAsync(id);
        if (user == null)
        {
            return new BaseResult { Succeeded = false, Errors = new List<string> { "User not found" } };
        }

        context.Set<TUser>().Remove(user);
        await context.SaveChangesAsync();
        return new BaseResult { Succeeded = true, Errors = new List<string> { "User deleted" } };
    }

    public virtual async Task<BaseResult<TUser?>> FindByEmailAsync(string email)
    {
        // TODO: Check if the result has succeed
        var user = (await Get(false)).Data.FirstOrDefault(u => u.Email == email) ??
                   (await Get(true)).Data.FirstOrDefault(u => u.Email == email);
        return new BaseResult<TUser?>() { Data = user };
    }

    public Task<BaseResult<TUser>> RequestPasswordReset(IForgotPasswordRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<BaseResult<TUser>> ResetPasswordRequest(IResetPasswordRequest request)
    {
        throw new NotImplementedException();
    }


    public virtual async Task<BaseResult<TUser>> Login(TLoginRequest loginDto)
    {
        // throw new NotImplementedException();

        var user = (await FindByEmailAsync(loginDto.Email)).Data;
        if (user == null)
        {
            throw new("User not found");
        }

        var userMgr = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<UserManager<TUser>>();
        var result = await userMgr.CheckPasswordAsync(user, loginDto.Password);
        if (!result)
        {
            throw new("Invalid password");
        }

        // CurrentUser = user;
        return new BaseResult<TUser> { Succeeded = true, Errors = new List<string> { "Login successful" } };
    }

    public virtual async Task<BaseResult<TUser>> RequestPasswordReset(ForgotPasswordRequest email)
    {
        // throw new NotImplementedException();
        var user = await FindByEmailAsync(email.Email);
        if (user == null)
        {
            return new BaseResult<TUser> { Succeeded = false, Errors = new List<string> { "User not found" } };
        }
        // TODO: send email

        return new BaseResult<TUser> { Succeeded = true, Errors = new List<string> { "Email sent" } };
    }

    public virtual async Task<BaseResult> ResetPasswordRequest(ResetPasswordRequest request)
    {

        // throw new NotImplementedException();
        var user = (await FindByEmailAsync(request.Email)).Data;
        if (user == null)
        {
            return new BaseResult { Succeeded = false, Errors = new List<string> { "User not found" } };
        }

        var userMgr = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<UserManager<TUser>>();
        var result = await userMgr.ResetPasswordAsync(user, request.ResetCode, request.NewPassword);
        return new BaseResult
        { Succeeded = result.Succeeded, Errors = [result.Errors.ToList().FirstOrDefault().ToString()] };
    }

    public virtual async Task<BaseResult> RequestResetPassword(ForgotPasswordRequest requestDto)
    {
        var user = (await FindByEmailAsync(requestDto.Email)).Data;
        if (user == null)
        {
            return new BaseResult { Succeeded = false, Errors = new List<string> { "User not found" }, Message = "User not found" };
        }

        var userMgr = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<UserManager<TUser>>();
        var token = await userMgr.GeneratePasswordResetTokenAsync(user);
        throw new Exception();
        // throw new NotImplementedException();


    }


    public virtual async Task<BaseResult> Logout(string? page)
    {
        // throw new NotImplementedException();
        CurrentUser = null;
        return new BaseResult { Succeeded = true, Message = "Logged out" };
    }

    public virtual async Task<BaseResult> AssignRole(string userId, string role)
    {
        // throw new NotImplementedException();
        var user = (await Get(userId)).Data;
        var userMgr = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<UserManager<TUser>>();
        var result = await userMgr.AddToRoleAsync(user, role);
        return new BaseResult { Succeeded = result.Succeeded, Errors = result.Errors.ToList().Select(e => e.Description).ToList(), Message = "Role assigned" };
        // { Succeeded = result.Succeeded, Errors = [result.Errors.ToList().FirstOrDefault().ToString()] };
    }

    public virtual TUser? CurrentUser { get; set; }


    public virtual async Task<BaseResult> AddToRoleAsync(string Id, string role)
    {
        // throw new NotImplementedException();
        var user = (await Get(Id)).Data;
        var userMgr = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<UserManager<TUser>>();
        var result = await userMgr.AddToRoleAsync(user, role);
        return new()
        { Succeeded = result.Succeeded, Errors = result.Errors.ToList().Select(e => e.Description).ToList() };
    }

    public virtual TUserInfo? BasicInfo { get; set; }

    public virtual async Task<BaseResult<TUser>> Create(TUserAdd dto)
    {
        // throw new NotImplementedException();
        var user = new TUser()
        {
            UserName = dto.FirstName + dto.LastName,
            Email = dto.Email,
            PhoneNumber = "",
        };
        //make sure to select the correct context for the user manager
        // var context = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<TContext>();
        var userMgr = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<UserManager<TUser>>();

        // var userMgr = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<UserManager<TUser>>();
        var result = await userMgr.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
        {
            return new BaseResult<TUser>()
            {
                Errors = result.Errors.ToList().Select(e => e.Description).ToList(),
                Message = "User not created",
                Succeeded = false,
            };
        }

        // var mailService = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IMailService>();
        //Mail the new User the password
        var mail = new MailRequest
        {
            ToEmails = [user.Email],
            Subject = "New User Account",
            Body =
                $"Dear {user.UserName}, <br> Your account has been created successfully. <br> Your username is {user.UserName} <br> Your password is {dto.Password} <br> Please change your password after login. <br> Regards, <br> System Administrator"
        };
        await mailService.SendMail(mail);
        return new BaseResult<TUser>()
        {
            Data = user,
            Message = "User created",
            Succeeded = true,
        };
    }
}