global using ZambeziDigital.BasicAccess.Models;
global using ZambeziDigital.BasicAccess.Contracts;
global using ZambeziDigital.Multitenancy.Models.Shared;

namespace ZambeziDigital.Multitenancy.Models.Base;

public class Tenant : BaseModel, IHasKey<int>
{
    public string? ConnectionString { get; set; }
    public string? Logo { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public Address? Address { get; set; }
    public string? Website { get; set; }
    public int? SubscriptionId { get; set; }

    //Navigation Properties
    public Subscription? Subscription { get; set; }
    public List<ApplicationUser> Users { get; set; } = new();
}




