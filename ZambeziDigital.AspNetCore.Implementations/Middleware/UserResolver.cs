using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ZambeziDigital.AspNetCore.Implementations.Generics.Middleware;
using ZambeziDigital.Base.Implementation.Models;
using ZambeziDigital.Base.Models;
using ZambeziDigital.Base.Models.Auth;

namespace ZambeziDigital.AspNetCore.Implementations.Middleware;

public class UserResolver<TUser>(RequestDelegate next, IServiceScopeFactory serviceScopeFactory) where TUser : IApplicationUser
{
    private readonly RequestDelegate _next = next;


    // Get Company Id from incoming requests 
    public async Task InvokeAsync(HttpContext context, ICurrentUserService<TUser> currentUserService)
    {
        string userIdFromCookie = string.Empty;
        
        try
        {
            userIdFromCookie = context.Request.Cookies["user-id"] ?? throw new Exception("User is null");
            await currentUserService.SetUser(userIdFromCookie);
        }
        catch
        {
            
            context.Request.Headers.TryGetValue("User", out var userFromHeader); // Institution Id from incoming request header
            if (string.IsNullOrEmpty(userFromHeader) == false)
            {
                if (!string.IsNullOrEmpty(userFromHeader))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"InstitutionResolver Middleware: InstitutionId: {userFromHeader}");
                    await currentUserService.SetUser(userFromHeader);
                    
                }
                else
                {
                    
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("REQUEST  HAS  NO  COOKIE WITH TENANT DETAILS");
                    
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Problems with user id.");
                    throw new Exception("Problems with user id.");
                }
            }
            
        }
        
        // If there's no user ID in the cookie and the user is authenticated, set it
        if (context.User.Identity != null && context.User.Identity.IsAuthenticated)
        {
            // Retrieve user ID from the user claims or other source
            var userManager = serviceScopeFactory.CreateScope().ServiceProvider
                .GetRequiredService<UserManager<BaseApplicationUser>>();
            var user = (await userManager.GetUserAsync(context.User));
            if(user is null) return;
            var userId = user.Id;
            await currentUserService.SetUser(userId);
            await currentUserService.SetUser(user.Id);
        
            // Set the user ID in the cookie for the response
            if (!string.IsNullOrEmpty(userId))
            {
                context.Response.Cookies.Append("user-id", userId.ToString() ?? throw new Exception("User ID is null"), 
                    new CookieOptions
                {
                    SameSite = SameSiteMode.None,
                    Expires = DateTimeOffset.UtcNow.AddHours(1),
                    HttpOnly = true,
                    Secure = context.Request.IsHttps
                });
            }
            context.Request.Headers.Append("User", user.Id);
        }
        await _next(context);
    }


}
