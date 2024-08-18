using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using ZambeziDigital.Base.Contracts.Tenancy;
using ZambeziDigital.Base.Implementation.Models;
using ZambeziDigital.Base.Implementation.Services;
using ZambeziDigital.Base.Models;
using ZambeziDigital.Base.Services.Contracts;

namespace ZambeziDigital.Blazor.Implementations.Services;

public class TenantService<TTenant>(IServiceScopeFactory serviceScopeFactory) :
    BaseService<TTenant, int>(serviceScopeFactory), ITenantService<TTenant>
    where TTenant : class, ITenant, new()
{
    public TTenant? CurrentTenant { get; set; }
    public async Task<TTenant> GetCurrentTenant()
    {
        if (CurrentTenant != null)
            return CurrentTenant;
        var httpClient = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IHttpClientFactory>().CreateClient("Auth");
        var request = await httpClient.GetAsync("api/Tenant/Current");
        if (!request.IsSuccessStatusCode) throw new Exception(request.ReasonPhrase);
        CurrentTenant = await request.Content.ReadFromJsonAsync<TTenant>();
        return CurrentTenant;
    }
}
