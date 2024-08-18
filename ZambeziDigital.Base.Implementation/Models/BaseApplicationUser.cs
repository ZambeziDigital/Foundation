using Microsoft.AspNetCore.Identity;
using ZambeziDigital.Base.DTOs.Auth;
using ZambeziDigital.Base.Models.Auth;

namespace ZambeziDigital.Base.Implementation.Models;

public class BaseApplicationUser : IdentityUser, IApplicationUser
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string? Password { get; set; }
    public List<IdentityRole>? Roles { get; set; }
    public UserState Active { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

public class BaseApplicationUserInfo : IUserInfo
{
    public string Id { get; set; }
    public string Name { get; set; }
    public List<string> Roles { get; set; }
    public string UserId { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string? Password { get; set; }
    public UserState Active { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class BaseApplicationUserAddRequest : IApplicationUserAddRequest
{
    public string Name { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string? OtherNames { get; set; }
    public string? PhoneNumber { get; set; }
    public List<string> Roles { get; set; }
    public UserState Active { get; set; }
}