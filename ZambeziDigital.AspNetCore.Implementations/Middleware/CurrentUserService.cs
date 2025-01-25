using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using ZambeziDigital.AspNetCore.Abstractions.Data;
using ZambeziDigital.Base.Contracts.Tenancy;
using ZambeziDigital.Base.Models.Auth;

namespace ZambeziDigital.AspNetCore.Implementations.Middleware;
public interface ICurrentUserService<TUser> where TUser : IApplicationUser
{
    Task<bool> SetUser(StringValues userFromHeader);
    string UserId { get; set; }
    TUser User { get; set; }
}
public class CurrentUserService<TUser, TBaseContext>(IServiceScopeFactory serviceScopeFactory) : 
    ICurrentUserService<TUser> 
    where TUser : IdentityUser, IApplicationUser, IMustHaveTenant, new()
    where TBaseContext : DbContext, IBaseDbContext<TUser>

{

    public async Task<bool> SetUser(StringValues userFromHeader)
    {
        try
        {
            UserId = userFromHeader;
            var context = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<TBaseContext>();
            var user = await context.AspNetUsers.Where(x => x.Id == UserId)
                .FirstOrDefaultAsync(); // check if user exists
            if (user != null)
            {
                UserId = user.Id;
                User = user;
                return true;
            }
            else
            {
                throw new Exception("User invalid");
            }
        }
        catch
        {
            Console.WriteLine("Error setting user");


        }

        return false;

    }

    public string UserId { get; set; }
    public TUser User { get; set; }
}