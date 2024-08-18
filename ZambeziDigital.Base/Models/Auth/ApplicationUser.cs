using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using ZambeziDigital.Base.Contracts.Base;
using ZambeziDigital.Base.DTOs.Auth;

namespace ZambeziDigital.Base.Models.Auth
{
    public interface IApplicationUser : IHasKey<string>
    {
        public string Name { get; set; }
        [NotMapped] public string? Password { get; set; }
        //Navigation properties
        public List<IdentityRole>? Roles { get; set; }
        public UserState Active { get; set; } 
        public DateTime CreatedAt { get; set; }
        public string PhoneNumber { get; set; }
    }
}
