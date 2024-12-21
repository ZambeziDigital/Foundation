using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using ZambeziDigital.Base.Implementation.Models;
using ZambeziDigital.Base.Implementation.Services.Contracts;
using ZambeziDigital.Base.Models;

namespace ZambeziDigital.Base.Implementation.Services;

public class APIKeyService(IServiceScopeFactory serviceScopeFactory) : BaseService<APIKey, int>(serviceScopeFactory), IAPIKeyService
{
    public async Task<BaseResult<APIKey>> Create(string name)
    {
        var httpClient = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IHttpClientFactory>().CreateClient("Auth");
        var request = await httpClient.PostAsync($"api/APIKey/Create/{name}", null);
        if (!request.IsSuccessStatusCode) throw new Exception(request.ReasonPhrase);
        var result = await request.Content.ReadFromJsonAsync<BaseResult<APIKey>>();
        if (result.Succeeded)
        {
            Objects.Add(result.Data);
        }
        return result;

    }

    public async Task<APIKey> Get(string key)
    {
        var httpClient = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IHttpClientFactory>().CreateClient("Auth");
        var request = await httpClient.GetAsync($"api/APIKey/Get/{key}");
        if (!request.IsSuccessStatusCode) throw new Exception(request.ReasonPhrase);
        return await request.Content.ReadFromJsonAsync<APIKey>();
    }
}