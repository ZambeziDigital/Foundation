global using ZambeziDigital.BasicAccess.Services.Contracts;
namespace ZambeziDigital.Multitenancy.Services.Contracts;

public interface ITenantSubscriptionService : IBaseService<TenantSubscription,int>;
public interface ISubscriptionService : IBaseService<Subscription,int>;


public interface ITenantService : IBaseService<Tenant, int>
{
    Tenant? CurrentTenant { get; set; }
    Task<Tenant> GetCurrentTenant();
}

