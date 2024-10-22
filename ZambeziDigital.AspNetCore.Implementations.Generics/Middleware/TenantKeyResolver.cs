using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ZambeziDigital.Base.Models.Auth;

namespace ZambeziDigital.AspNetCore.Implementations.Generics.Middleware;

public class TenantKeyResolver<TUser, TApiKey>(RequestDelegate next, IServiceScopeFactory serviceScopeFactory) where TApiKey : class, IAPIKey, IHasKey<int>, new() where TUser : class, IApplicationUser
{
    private readonly RequestDelegate _next = next;
    
    // Get Company Id from incoming requests 
    public async Task InvokeAsync(HttpContext context, IBaseCurrentTenantService<TUser> currentTenantService)
    {
        try
        {
            Log.Information("TenantKeyResolver Middleware: Invoked");
            // try get from api key
            context.Request.Headers.TryGetValue("X-DigitalKey", out var keyFromHeader);
            // Log.Information($"TenantKeyResolver Middleware: Key: {keyFromHeader}");
            if (string.IsNullOrEmpty(keyFromHeader) == false)
            {
                // Log.Information("TenantKeyResolver Middleware: Key found");
                var apiKeyService = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IAPIKeyService<TApiKey>>();
                var apiKey = await apiKeyService.Get(keyFromHeader);
                // Log.Information($"TenantKeyResolver Middleware: Key: {apiKey}");
                if (apiKey is not null)
                {
                    // Log.Information("TenantKeyResolver Middleware: Key found");
                    await currentTenantService.SetTenant(apiKey.TenantId);
                    await currentTenantService.SetUser(apiKey.UserId);
                    await currentTenantService.TurnOnKeyedAccess();
                }
                // Log.Information("TenantKeyResolver Middleware: Key not found");
                //Authenticate the user
                var userManager = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<UserManager<TUser>>();
                var user = await userManager.FindByIdAsync(apiKey.UserId);
                if (user is not null)
                {
                    await currentTenantService.SetUser(user.Id);
                }
                //set context.User.Identity.IsAuthenticated to true
                context.Request.Headers.Append("User", user.Id);
                
                context.SignInAsync(IdentityConstants.ApplicationScheme, new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Id),
                    new Claim(ClaimTypes.Role, "Tenant Admin"),
                    new Claim(ClaimTypes.AuthenticationMethod, "ApiKey"),
                    new Claim(ClaimTypes.NameIdentifier, user.Id)
                }, "ApiKey")));
                context.User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Id),
                    new Claim(ClaimTypes.Role, "Tenant Admin"),
                    new Claim(ClaimTypes.AuthenticationMethod, "ApiKey"),
                    new Claim(ClaimTypes.NameIdentifier, user.Id)
                }, "ApiKey"));
                // context.User.AddIdentity
                context.User.AddIdentity(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Id),
                    new Claim(ClaimTypes.Role, "Tenant Admin"),
                    new Claim(ClaimTypes.AuthenticationMethod, "ApiKey"),
                    new Claim(ClaimTypes.NameIdentifier, user.Id)
                }, "ApiKey"));
            } 
            // Log.Information("TenantKeyResolver Middleware: Key not found");
        }
        catch (Exception e)
        {
            // Log.Error(e, "TenantKeyResolver Middleware: Error");
        }

        await _next(context);
    }


}