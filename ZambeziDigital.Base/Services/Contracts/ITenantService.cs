using ZambeziDigital.Base.Contracts.Tenancy;

namespace ZambeziDigital.Base.Services.Contracts;

public interface ITenantService<TTenant> : IBaseService<TTenant, int>
    where TTenant : class, ITenant, IHasKey<int>, new() 
{
    TTenant? CurrentTenant { get; set; }
    Task<TTenant> GetCurrentTenant();
}

