using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using ZambeziDigital.Base.Contracts.Tenancy;
// using ZambeziDigital.AspNetCore.Abstractions.Authorization.Data;


namespace ZambeziDigital.AspNetCore.Implementations.Generics.Data;

public interface IBaseDbContext<TUser, TTenant> : ZambeziDigital.AspNetCore.Abstractions.Data.IBaseDbContext<TUser>
    where TUser : IdentityUser, IHasKey<string>,IMustHaveTenant, new()
    where TTenant : class, IHasKey<int>
{
    DbSet<TTenant> Tenants { get; set; }
    DatabaseFacade Database { get; } // Add this line
}