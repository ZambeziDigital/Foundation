global using ZambeziDigital.Authentication.DataTransferObjects;
global using ZambeziDigital.Multitenancy.Models.Base;

namespace ZambeziDigital.Multitenancy.Models.Shared
{
    public class ApplicationUser : ZambeziDigital.Authentication.Models.ApplicationUser
    {
        public int? TenantId { get; set; }
        public bool IsOwner { get; set; } = false;
        //Navigation properties
        public Tenant? Tenant { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public ApplicationUser() { }
        public ApplicationUser(ApplicationUserAddRequest request)
        {
            Email = request.Email;
            Password = request.Password;
            TenantId = request.TenantId;
            UserName = request.Email;
            Active = UserState.Active;
            Name = $"{request.FirstName} {request.LastName}".Replace("  ", " ");
        }

    }
    
    public class ApplicationUserAddRequest :  ZambeziDigital.Authentication.DataTransferObjects.ApplicationUserAddRequest
    {
        public int TenantId { get; set; }
    }
}