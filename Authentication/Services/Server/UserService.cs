namespace ZambeziDigital.Authentication.Services.Server;

public class UserService<TUser,TContext>(IServiceScopeFactory serviceScopeFactory) : IUserService
    where TContext : DbContext, IBaseDbContext<TUser>
    where TUser : IdentityUser, IHasKey<string>, new()
{
    public DbContext context { get; set; }
    public List<ApplicationUser> Objects { get; set; }

    public async Task<List<ApplicationUser>> Get(bool forceRefresh = false)
    {
        // throw new NotImplementedException();
        if (forceRefresh || Objects == null || !Objects.Any())
        {
            var context = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<TContext>();
            Objects = context.Set<ApplicationUser>()
                .Include(x=>x.Roles)
                // .Include(u=>u.Tenant)
                .ToList();
        }

        return Objects;
    }

    public async Task<ApplicationUser> Get(string id)
    {
        // throw new NotImplementedException();
        var user = (await Get(false)).FirstOrDefault(u => u.Id == id) ??
        (await Get(true)).FirstOrDefault(u => u.Id == id) ?? 
        throw new Exception("User not found");
        return user;
    }

    public async Task<ApplicationUser> Update(ApplicationUser t)
    {
        // throw new NotImplementedException();
        var context = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<TContext>();
        var result = context.Set<ApplicationUser>().Update(t);
        await context.SaveChangesAsync();
        return t;
    }

    public async Task<ApplicationUser> Create(ApplicationUser dto)
    {
        
        
        // throw new NotImplementedException();
        //make sure to select the correct context for the user manager
        // var context = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<TContext>();
        var userMgr = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        
        // var userMgr = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var result = await userMgr.CreateAsync(dto, dto.Password);
        if (!result.Succeeded)
        {
            return null;
        }
        
        var mailService = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IMailService>();
        //Mail the new User the password
        var mail = new MailRequest
        {
            ToEmail = dto.Email,
            Subject = "New User Account",
            Body = $"Dear {dto.UserName}, <br> Your account has been created successfully. <br> Your username is {dto.UserName} <br> Your password is {dto.Password} <br> Please change your password after login. <br> Regards, <br> System Administrator"
        };
        await mailService.SendMail(mail);
        return dto;
        
        // throw new NotSupportedException("Please use the DTO Version");
    }

    public async Task<BasicResult> Delete(string id)
    {
        // throw new NotImplementedException();
        var context = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<TContext>();
        var user = await context.Set<ApplicationUser>().FindAsync(id);
        if (user == null)
        {
        return new BasicResult { Succeeded = false, Errors = new List<string> { "User not found" } };
        }
        context.Set<ApplicationUser>().Remove(user);
        await context.SaveChangesAsync();
        return new BasicResult { Succeeded = true, Errors = new List<string> { "User deleted" } };
    }

    public async Task<ApplicationUser?> FindByEmailAsync(string email)
    {
        // throw new NotImplementedException();
        var user = (await Get(false)).FirstOrDefault(u => u.Email == email) ??
        (await Get(true)).FirstOrDefault(u => u.Email == email);
        return user;
    }

    public async Task<BasicResult> RequestPasswordReset(ForgotPasswordRequest email)
    {
        // throw new NotImplementedException();
        var user = await FindByEmailAsync(email.Email);
        if (user == null)
        {
        return new BasicResult { Succeeded = false, Errors = new List<string> { "User not found" } };
        }
        // TODO: send email
        
        return new BasicResult { Succeeded = true, Errors = new List<string> { "Email sent" } };
    }

    public async Task<BasicResult> ResetPasswordRequest(ResetPasswordRequest request)
    {
       
        // throw new NotImplementedException();
        var user = await FindByEmailAsync(request.Email);
        if (user == null)
        {
        return new BasicResult { Succeeded = false, Errors = new List<string> { "User not found" } };
        }
        var userMgr = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var result = await userMgr.ResetPasswordAsync(user, request.ResetCode, request.NewPassword);
        return new BasicResult { Succeeded = result.Succeeded, Errors = [result.Errors.ToList().FirstOrDefault().ToString()] };
    }

    public async Task<BasicResult> RequestResetPassword(ForgotPasswordRequest requestDto)
    {
        var user = await FindByEmailAsync(requestDto.Email);
        if (user == null)
        {
        return new BasicResult { Succeeded = false, Errors = new List<string> { "User not found" } };
        }
        var userMgr = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var token = await userMgr.GeneratePasswordResetTokenAsync(user);
        throw new Exception();
        // throw new NotImplementedException();


    }

    public async Task<BasicResult<ApplicationUser>> Login(LoginRequestDto loginDto)
    {
        // throw new NotImplementedException();

        var user = await FindByEmailAsync(loginDto.Email);
        if (user == null)
        {
        return new BasicResult<ApplicationUser> { Succeeded = false, Errors = new List<string> { "User not found" } };
        }
        var userMgr = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var result = await userMgr.CheckPasswordAsync(user, loginDto.Password);
        if (!result)
        {
        return new BasicResult<ApplicationUser> { Succeeded = false, Errors = new List<string> { "Invalid password" } };
        }
        CurrentUser = user;
        return new BasicResult<ApplicationUser> { Succeeded = true, Errors = new List<string> { "Login successful" } };
    }

    public async Task<BasicResult> Logout(string? page)
    {
        // throw new NotImplementedException();
        CurrentUser = null;
        return new BasicResult { Succeeded = true, Errors = new List<string> { "Logout successful" } };
    }

    public async Task<BasicResult> AssignRole(string userId, string role)
    {
        // throw new NotImplementedException();
        var user = await Get(userId);
        var userMgr = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var result = await userMgr.AddToRoleAsync(user, role);
        return new BasicResult { Succeeded = result.Succeeded, Errors = [result.Errors.ToList().FirstOrDefault().ToString()] };
    }

    public ApplicationUser? CurrentUser { get; set; }


    public async Task<BasicResult> AddToRoleAsync(string Id, string role)
    {
        // throw new NotImplementedException();
        var user = await Get(Id);
        var userMgr = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var result = await userMgr.AddToRoleAsync(user, role);
        return new BasicResult { Succeeded = result.Succeeded, Errors = [result.Errors.ToList().FirstOrDefault().ToString()] };
    }

    public UserInfo? BasicInfo { get; set; }
    public async Task<ApplicationUser> Create(ApplicationUserAddRequest dto)
    {
        // throw new NotImplementedException();
        var user = new ApplicationUser(dto);
        //make sure to select the correct context for the user manager
        // var context = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<TContext>();
        var userMgr = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        
        // var userMgr = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var result = await userMgr.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
        {
            return null;
        }
        var mailService = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IMailService>();
        //Mail the new User the password
        var mail = new MailRequest
        {
            ToEmail = user.Email,
            Subject = "New User Account",
            Body = $"Dear {user.UserName}, <br> Your account has been created successfully. <br> Your username is {user.UserName} <br> Your password is {dto.Password} <br> Please change your password after login. <br> Regards, <br> System Administrator"
        };
        await mailService.SendMail(mail);
        return user;
    }
}