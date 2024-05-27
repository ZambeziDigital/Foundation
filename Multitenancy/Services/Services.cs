using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using ZambeziDigital.BasicAccess.Services.Client;
using ZambeziDigital.Multitenancy.Services.Contracts;

namespace ZambeziDigital.Multitenancy.Services;

public class TenantSubscriptionService(IServiceScopeFactory serviceScopeFactory) : BaseService<TenantSubscription, int>(serviceScopeFactory), ITenantSubscriptionService;
public class TenantService(IServiceScopeFactory serviceScopeFactory) : BaseService<Tenant, int>(serviceScopeFactory), ITenantService
{
    public Tenant? CurrentTenant { get; set; }
    public async Task<Tenant> GetCurrentTenant()
    {
        if (CurrentTenant != null)
            return CurrentTenant;
        var httpClient = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IHttpClientFactory>().CreateClient("Auth");
        var request = await httpClient.GetAsync("api/Tenant/Current");
        if (!request.IsSuccessStatusCode) throw new Exception(request.ReasonPhrase);
        CurrentTenant = await request.Content.ReadFromJsonAsync<Tenant>();
        return CurrentTenant;
    }
}
public class SubscriptionService(IServiceScopeFactory serviceScopeFactory) : BaseService<Subscription, int>(serviceScopeFactory), ISubscriptionService;
