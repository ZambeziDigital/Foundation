using ZambeziDigital.BasicAccess.Services.Contracts;

namespace ZambeziDigital.Multitenancy.Services.Contracts;


public interface ITenantService<TTenant> : IBaseService<TTenant, int>
    where TTenant : class, ITenant, new()
{
    TTenant? CurrentTenant { get; set; }
    Task<TTenant> GetCurrentTenant();
}

