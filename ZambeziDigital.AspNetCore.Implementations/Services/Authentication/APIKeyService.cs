using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using ZambeziDigital.AspNetCore.Implementations.Generics.Middleware;
using ZambeziDigital.AspNetCore.Implementations.Generics.Services;
using ZambeziDigital.Base.Contracts.Base;
using ZambeziDigital.Base.Contracts.Tenancy;
using ZambeziDigital.Base.Enums;
using ZambeziDigital.Base.Implementation.Models;
using ZambeziDigital.Base.Models;
using ZambeziDigital.Base.Models.Auth;
using ZambeziDigital.Base.Services.Contracts;

namespace ZambeziDigital.AspNetCore.Implementations.Services.Authentication;

public class APIKeyService<TContext,TICurrentTenantService,TTenant, TUser>(
    IServiceScopeFactory serviceScopeFactory, 
    TContext context, TICurrentTenantService currentTenantService) : 
    BaseService<APIKey, int, TContext>(context), 
    IAPIKeyService<APIKey> where TContext : DbContext
    where TICurrentTenantService : IBaseCurrentTenantService<TUser, TTenant>
    where TUser : IApplicationUser
    where TTenant : class, IHasKey<int>, ITenant
{
    public Task<BaseResult<APIKey>> Create(string name)
    {
        // var currentTenantService = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<ICurrentTenantService>();
        // var userService = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IUserService>();
        // var user = userService.Get(currentTenantService.)
        var newKey = new APIKey()
        {
            Key = Guid.NewGuid().ToString(),
            CreatedAt = DateTime.Now,
            Email = currentTenantService.Tenant.Email,
            UserId = currentTenantService.UserId,
            // Tenant = currentTenantService.Tenant,
            Status = APIStatus.Inactive,
            Role = "Tenant Admin",
            Name = name,
            UserName = currentTenantService.User.Name,
            Password = string.Empty,
            TenantId = currentTenantService.TenantId,
            UpdatedAt = DateTime.Now,
            Token = string.Empty,
            // Tenant = null,
            Creator = currentTenantService.User.Name
        };
        return Create(newKey);
    }

    public async Task<APIKey> Get(string key)
    {
        var result = context.Set<APIKey>().FirstOrDefault(k => k.Key == key);
        return result;
    }

}