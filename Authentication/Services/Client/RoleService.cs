namespace ZambeziDigital.Authentication.Services.Client;

public class RoleService(IServiceScopeFactory serviceScopeFactory) : IRoleService
{
    public async Task<IdentityResult> CreateRole(string role)
    {
        var httpClient = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IHttpClientFactory>().CreateClient("Auth");
        var request = await httpClient.PostAsJsonAsync("api/Role", role);
        if (!request.IsSuccessStatusCode) throw new Exception(request.ReasonPhrase);
        return await request.Content.ReadFromJsonAsync<IdentityResult>()?? throw new Exception("No content found");
    }

    public async Task<IdentityResult> DeleteRole(string roleId)
    {
        var httpClient = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IHttpClientFactory>().CreateClient("Auth");
        var request = await httpClient.DeleteAsync($"api/Role/{roleId}");
        if (!request.IsSuccessStatusCode) throw new Exception(request.ReasonPhrase);
        return await request.Content.ReadFromJsonAsync<IdentityResult>()?? throw new Exception("No content found");
    }

    public async Task<IdentityRole> GetRole(string id)
    {
        var httpClient = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IHttpClientFactory>().CreateClient("Auth");
        var request = await httpClient.GetAsync($"api/Role/{id}");
        if (!request.IsSuccessStatusCode) throw new Exception(request.ReasonPhrase);
        return await request.Content.ReadFromJsonAsync<IdentityRole>() ?? throw new Exception("No content found");
    }

    public async Task<List<IdentityRole>> GetRoles()
    {
        var httpClient = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IHttpClientFactory>().CreateClient("Auth");
        var request = await httpClient.GetAsync("api/Role");
        if (!request.IsSuccessStatusCode) throw new Exception(request.ReasonPhrase);
        return await request.Content.ReadFromJsonAsync<List<IdentityRole>>() ?? throw new Exception("No content found");
    }

    public async Task<IdentityResult> UpdateRole(IdentityRole role)
    {
        var httpClient = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IHttpClientFactory>().CreateClient("Auth");
        var request = await httpClient.PutAsJsonAsync("api/Role", role);
        if (!request.IsSuccessStatusCode) throw new Exception(request.ReasonPhrase);
        return await request.Content.ReadFromJsonAsync<IdentityResult>() ?? throw new Exception("No content found");
    }
}
