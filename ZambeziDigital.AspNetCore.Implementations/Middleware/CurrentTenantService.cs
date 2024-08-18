// using Microsoft.AspNetCore.Identity;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.Primitives;
// using ZambeziDigital.AspNetCore.Implementations.Generics.Data;
// using ZambeziDigital.Base.Contracts.Base;
// using ZambeziDigital.Base.Contracts.Tenancy;
// using ZambeziDigital.Base.Models.Auth;
// // using ZambeziDigital.AspNetCore.Abstractions.Data;
//
// namespace ZambeziDigital.AspNetCore.Implementations.Middleware;
// public interface IBaseCurrentTenantService<TUser> where TUser : IApplicationUser
// {
//     public string? ConnectionString { get; set; }
//     int TenantId { get; set; }
//     public Task<bool> SetTenant(int tenant);
//     Task<bool> SetUser(StringValues userFromHeader);
//     
//     string UserId { get; set; }
//     TUser User { get; set; }
// }
// public class BaseCurrentTenantService<TUser, TBaseContext, TTenant>(IServiceScopeFactory serviceScopeFactory) : 
//     IBaseCurrentTenantService<TUser> 
//     where TUser : IdentityUser, IApplicationUser, IMustHaveTenant, new()
//     where TBaseContext : DbContext, IBaseDbContext<TUser, TTenant>
//     where TTenant : class, IHasKey<int>, ITenant
//
// {
//     public string? ConnectionString { get; set; }
//     public int TenantId { get; set; }
//
//     public async Task<bool> SetTenant(int tenantId)
//     {
//         try
//         {
//
//         var context = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<TBaseContext>();
//         var tenantInfo = await context.Tenants.FirstOrDefaultAsync(x => x.Id == tenantId); // check if tenant exists
//         if (tenantInfo != null)
//         {
//             TenantId = tenantId;
//             ConnectionString = tenantInfo.ConnectionString;
//             return true;
//         }
//         else
//         {
//             throw new Exception("Company invalid"); 
//         }
//         }catch{
//             Console.WriteLine("Error setting company");
//             
//         }
//         return false;
//
//
//     }
//
//     public async Task<bool> SetUser(StringValues userFromHeader)
//     {
//         try{
//         UserId = userFromHeader;
//         var context = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<TBaseContext>();
//         var user = await context.AspNetUsers.Where(x => x.Id == UserId).FirstOrDefaultAsync(); // check if user exists
//         if (user != null)
//         {
//             UserId = user.Id;
//             User = user;
//             return true;
//         }
//         else
//         {
//             throw new Exception("User invalid"); 
//         }
//         }catch{
//             Console.WriteLine("Error setting user");
//             
//         
//         }
//         return false;
//
//     }
//
//     public string UserId { get; set; }
//     public TUser User { get; set; }
// }