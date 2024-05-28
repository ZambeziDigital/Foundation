using ZambeziDigital.BasicAccess.Services.Contracts;

namespace ZambeziDigital.BasicAccess.Services.Client;
public class BaseService<T, TKey>(IServiceScopeFactory serviceScopeFactory) : IBaseService<T,TKey>  where T : class, IHasKey<TKey>, new() where TKey : IEquatable<TKey>
{
    public List<T> Objects { get; set; } = new();
    public async Task<List<T>> Get(bool forceRefresh = false)
    {
        try
        {
            if(Objects.Count > 0 && !forceRefresh) return Objects;
            var httpClient = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IHttpClientFactory>().CreateClient("Auth");
            var controller = typeof(T).Name;
            var request = await httpClient.GetAsync($"api/{controller}");
            if (!request.IsSuccessStatusCode) throw new Exception(request.ReasonPhrase);
            var objects = await request.Content.ReadFromJsonAsync<List<T>>();
            Objects = objects ?? throw new Exception("No objects were found");
            return  Objects;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<T> Get(TKey id)
    {
        var obj = Objects.FirstOrDefault(x => x.Id.Equals(id));
        if (obj is null)
        {
            var httpClient = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IHttpClientFactory>().CreateClient("Auth");
            var request = await httpClient.GetAsync($"api/{typeof(T).Name}/{id}");
            if (!request.IsSuccessStatusCode) throw new Exception(request.ReasonPhrase);
            obj = await request.Content.ReadFromJsonAsync<T>();
                  
        }
        return obj ?? throw new ($"{nameof(T)} with Id {id} not found");
    }

    public async Task<T> Get(int id)
    {
        try
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));
            var obj = (await Get()).FirstOrDefault(t => t.Id.Equals(id)) 
                      ?? (await Get(true)).FirstOrDefault(t => t.Id.Equals(id)) 
                      ?? throw new Exception($"No object found with Id: {id}");
            return obj;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<T> Create(T dto)
    {
        try
        {
            var httpClient = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IHttpClientFactory>().CreateClient("Auth");
            var request = await httpClient.PostAsJsonAsync($"api/{typeof(T).Name}", dto);
            if (!request.IsSuccessStatusCode) throw new Exception(request.ReasonPhrase);
            var obj = await request.Content.ReadFromJsonAsync<T>();
            Objects.Add(obj ?? throw new ($"{nameof(T)} is null"));
            return obj;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public Task<BasicResult> Delete(TKey id)
    {
        throw new NotImplementedException();
    }

    public async Task<T> Update(T t)
    {
        try
        {
            var httpClient = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IHttpClientFactory>().CreateClient("Auth");
            var request = await httpClient.PutAsJsonAsync($"api/{typeof(T).Name}", t);
            if (!request.IsSuccessStatusCode) throw new Exception(request.ReasonPhrase);
            T obj = await request.Content.ReadFromJsonAsync<T>() ?? throw new ($"{nameof(T)} is null");
            var selected = obj;
            return selected;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<BasicResult<object>> Delete(int id)
    {
        try
        {
            var httpClient = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IHttpClientFactory>().CreateClient("Auth");
            var request = await httpClient.DeleteAsync($"api/{typeof(T).Name}/{id}");
            if (!request.IsSuccessStatusCode) throw new Exception(request.ReasonPhrase);
            var obj =  (await Get()).FirstOrDefault(x => x.Id.Equals(id) ) ?? throw new Exception("No object found");
            Objects.Remove(obj);
            return new BasicResult<object> { Succeeded = true, Errors = new List<string> { "Object deleted" } };
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}

public class BaseService<T, TNew, TKey>(IServiceScopeFactory serviceScopeFactory) : BaseService<T,TKey>(serviceScopeFactory),  IBaseService<T,TNew, TKey>  where T : class, IHasKey<TKey>, new() where TKey : IEquatable<TKey> where TNew : class
{
    public async Task<T> Create(TNew dto)
    {
        try
        {
            var httpClient = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IHttpClientFactory>().CreateClient("Auth");
            var request = await httpClient.PostAsJsonAsync($"api/{typeof(T).Name}", dto);
            if (!request.IsSuccessStatusCode) throw new Exception(request.ReasonPhrase);
            var obj = await request.Content.ReadFromJsonAsync<T>();
            Objects.Add(obj ?? throw new Exception("object cant be null"));
            return obj;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}