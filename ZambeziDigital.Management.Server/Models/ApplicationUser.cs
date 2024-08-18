using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using ZambeziDigital.Base.Contracts.Base;
using ZambeziDigital.Base.Contracts.Tenancy;
using ZambeziDigital.Base.DTOs.Auth;

namespace ZambeziDigital.Management.Server
{
    public class ApplicationUser : IdentityUser, IHasKey<string>, IMustHaveTenant
    {
        public string Name { get; set; } = string.Empty;
        public int? TenantId { get; set; } = null;
        [NotMapped] public string? Password { get; set; }
        public bool IsOwner { get; set; } = false;
        
        //Navigation properties
        // public Tenant? Tenant { get; set; }
        public List<IdentityRole>? Roles { get; set; } = new();
        public UserState Active { get; set; } = UserState.Active;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        // public ApplicationUser() { }
        // public ApplicationUser(ApplicationUserAddRequest request)
        // {
        //     Email = request.Email;
        //     Password = request.Password;
        //     TenantId = request.TenantId;
        //     UserName = request.Email;
        //     Active = UserState.Active;
        //     Name = $"{request.FirstName} {request.LastName}".Replace("  ", " ");
        // }

    }
}