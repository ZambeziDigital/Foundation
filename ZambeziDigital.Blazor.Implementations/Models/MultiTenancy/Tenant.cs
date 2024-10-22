using ZambeziDigital.Base.Contracts.Tenancy;
using ZambeziDigital.Base.Models;

namespace ZambeziDigital.Blazor.Implementations.Models.MultiTenancy;

public class Tenant : BaseModel<int>, ITenant
{
    public string? ConnectionString { get; set; }
    public string? Logo { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Website { get; set; }
    public int? SubscriptionId { get; set; }
    // public ISubscription? Subscription { get; set; }
}