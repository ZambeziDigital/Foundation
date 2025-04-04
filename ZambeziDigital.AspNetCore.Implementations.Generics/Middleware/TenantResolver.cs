using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ZambeziDigital.Base.Contracts.Tenancy;
using ZambeziDigital.Base.Models.Auth;

namespace ZambeziDigital.AspNetCore.Implementations.Generics.Middleware;

public class TenantResolver<TUser, TCurrentTenantService, TTenant>(RequestDelegate next, IServiceScopeFactory serviceScopeFactory) 
    where TUser : class, IApplicationUser, IMustHaveTenant
    where TCurrentTenantService : class, IBaseCurrentTenantService<TUser, TTenant>
{
    private readonly RequestDelegate _next = next;
    // Get Company Id from incoming requests 
    public async Task InvokeAsync(HttpContext context, TCurrentTenantService currentTenantService)
    {
          // Read the tenant ID from the cookie in the incoming request
        int tenantIdFromCookie = 0;
        string userIdFromCookie = string.Empty;
        try
        {
            tenantIdFromCookie = int.Parse(context.Request.Cookies["tenant-id"] ?? throw new Exception("Tenant ID is null"));
            await currentTenantService.SetTenant(tenantIdFromCookie);
        }
        catch
        {
            
            context.Request.Headers.TryGetValue("Tenant", out var tenantFromHeader); // Institution Id from incoming request header
            if (string.IsNullOrEmpty(tenantFromHeader) == false)
            {
                int tenantId = 0;
                if (int.TryParse(tenantFromHeader, out tenantId))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"InstitutionResolver Middleware: InstitutionId: {tenantId}");
                    await currentTenantService.SetTenant(tenantId);
                    
                }
                else
                {
                    
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("REQUEST  HAS  NO  COOKIE WITH TENANT DETAILS");
                    
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Problems with tenant id.");
                    throw new Exception("Problems with tenant id.");
                }
            }
            
        }


          
         
        
        // If there's no tenant ID in the cookie and the user is authenticated, set it
        if (context.User.Identity != null && tenantIdFromCookie is 0 && context.User.Identity.IsAuthenticated)
        {
            // Retrieve tenant ID from the user claims or other source
            var userManager = serviceScopeFactory.CreateScope().ServiceProvider
                .GetRequiredService<UserManager<TUser>>();
            var user = (await userManager.GetUserAsync(context.User));
            if(user is null) return;
            var tenantId = user.TenantId;
        
            // Set the tenant ID in the cookie for the response
            if (tenantId is not null and not 0)
            {
                await currentTenantService.SetTenant((int)tenantId);
                await currentTenantService.SetUser(user.Id);
                context.Response.Cookies.Append("tenant-id", tenantId.ToString() ?? throw new Exception("Tenant ID is null"), 
                    new CookieOptions
                {
                    SameSite = SameSiteMode.None,
                    Expires = DateTimeOffset.UtcNow.AddHours(1),
                    HttpOnly = true,
                    Secure = context.Request.IsHttps
                });
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Problems with tenant id.");
                // throw new Exception("Problems with tenant id.");
            }           
            
            // Set the user ID in the cookie for the response
            context.Response.Cookies.Append("user-id", user.Id, new CookieOptions
            {
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddHours(1),
                HttpOnly = true,
                Secure = context.Request.IsHttps
            });
            context.Request.Headers.Append("User", user.Id);
        }
        else
        {
            // // Set the tenant ID in the cookie for the response
            // context.Response.Cookies.Append("tenant-id", tenantIdFromCookie.ToString(), new CookieOptions
            // {
            //     Expires = DateTimeOffset.UtcNow.AddHours(1),
            //     HttpOnly = true,
            //     Secure = context.Request.IsHttps
            // });
            // context.Request.Headers.Append("Tenant", tenantIdFromCookie.ToString());
        }
        
        
        
         


   
        await _next(context);
    }


}