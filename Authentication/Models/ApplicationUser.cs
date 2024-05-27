global using System.ComponentModel.DataAnnotations.Schema;
global using Microsoft.AspNetCore.Identity;
global using ZambeziDigital.Authentication.DataTransferObjects;
global using ZambeziDigital.BasicAccess.Contracts;

namespace ZambeziDigital.Authentication.Models
{
    public class ApplicationUser : IdentityUser, IHasKey<string>
    {
        public string Name { get; set; } = string.Empty;
        [NotMapped] public string? Password { get; set; }
        
        //Navigation properties
        public List<IdentityRole>? Roles { get; set; } = new();
        public UserState Active { get; set; } = UserState.Active;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public ApplicationUser() { }
        public ApplicationUser(ApplicationUserAddRequest request)
        {
            Email = request.Email;
            Password = request.Password;
            UserName = request.Email;
            Active = UserState.Active;
            Name = $"{request.FirstName} {request.LastName}".Replace("  ", " ");
        }
    }
}