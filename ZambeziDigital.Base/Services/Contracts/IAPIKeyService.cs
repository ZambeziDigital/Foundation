namespace ZambeziDigital.Base.Services.Contracts;

public interface IAPIKeyService<TAPIKey> : IBaseService<TAPIKey, int> where TAPIKey : class, IAPIKey, IHasKey<int>, new()
{
    Task<BaseResult<TAPIKey>> Create(string Name);
    Task<TAPIKey> Get(string key);
}