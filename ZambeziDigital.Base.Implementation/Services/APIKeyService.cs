using Microsoft.Extensions.DependencyInjection;
using ZambeziDigital.Base.Implementation.Models;
using ZambeziDigital.Base.Implementation.Services.Contracts;
using ZambeziDigital.Base.Models;

namespace ZambeziDigital.Base.Implementation.Services;

public class APIKeyService(IServiceScopeFactory serviceScopeFactory) : BaseService<APIKey, int>(serviceScopeFactory), IAPIKeyService
{
    public virtual async Task<BaseResult<APIKey>> Create(string Name)
    {
        throw new NotImplementedException();
    }

    public  virtual async  Task<APIKey> Get(string key)
    {
        throw new NotImplementedException();
    }
}