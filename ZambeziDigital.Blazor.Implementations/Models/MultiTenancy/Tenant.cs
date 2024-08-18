using ZambeziDigital.Base.Contracts.Tenancy;

namespace ZambeziDigital.Blazor.Implementations.Models.MultiTenancy;

public class Tenant : ITenant
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string? ConnectionString { get; set; }
    public string? Logo { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Website { get; set; }
    public int? SubscriptionId { get; set; }
    // public ISubscription? Subscription { get; set; }
}