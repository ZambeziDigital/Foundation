using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using ZambeziDigital.Base.Contracts.Base;
using ZambeziDigital.Base.Models;
using ZambeziDigital.Base.Services.Contracts;

namespace ZambeziDigital.Base.Implementation.Services;
public class BaseService<T, TKey>(IServiceScopeFactory serviceScopeFactory) : IBaseService<T, TKey>

    where T : class, IHasKey<TKey>, new()
    where TKey : IEquatable<TKey>
{
    public List<T> Objects { get; set; } = new();
    // public virtual async Task<List<T>> Get(bool forceRefresh = false)
    // {
    //     try
    //     {
    //         if(Objects.Count > 0 && !forceRefresh) return Objects;
    //         var httpClient = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IHttpClientFactory>().CreateClient("Auth");
    //         var controller = typeof(T).Name;
    //         var request = await httpClient.GetAsync($"api/{controller}");
    //         if (!request.IsSuccessStatusCode) throw new Exception(request.ReasonPhrase);
    //         var objects = await request.Content.ReadFromJsonAsync<BaseResult<List<T>>>();
    //         Objects = objects.Object ?? throw new Exception("No objects were found");
    //         return  Objects;
    //     }
    //     catch (Exception e)
    //     {
    //         throw new Exception(e.Message);
    //     }
    // }

    public virtual async Task<BaseResult<T>> Get(TKey id, bool cached = false)
    {
        try
        {
            var obj = Objects.FirstOrDefault(x => x.Id.Equals(id));
            if (obj is null || !cached)
            {
                var httpClient = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IHttpClientFactory>().CreateClient("Auth");
                var request = await httpClient.GetAsync($"api/{typeof(T).Name}/{id}");
                if (!request.IsSuccessStatusCode) throw new Exception(request.ReasonPhrase);
                obj = (await request.Content.ReadFromJsonAsync<BaseResult<T>>()).Data;

            }
            return new BaseResult<T>()
            {
                Succeeded = obj != null,
                Data = obj,
                Errors = obj == null ? new List<string>() { $"{typeof(T).Name} with Id {id} not found" } : new List<string>()
            };
        }
        catch (Exception e)
        {
            return
                new BaseResult<T>()
                {
                    Succeeded = false,
                    Errors = new List<string>() { e.Message },
                    Message = e.Message,
                    Data = null
                };
        }


    }


    public virtual async Task<BaseResult<List<T>>> Get(bool paged = false, int page = 0, int pageSize = 10, bool cached = false)
    {
        try
        {
            if (Objects is not null && Objects.Count > 0 && cached) return new BaseResult<List<T>>()
            {
                Succeeded = true,
                Data = Objects
            };
            var httpClient = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IHttpClientFactory>().CreateClient("Auth");
            var controller = typeof(T).Name;
            HttpResponseMessage request;
            if (paged)
                request = await httpClient.GetAsync($"api/{controller}?pageNumber={page}&pageSize={pageSize}");
            else
                request = await httpClient.GetAsync($"api/{controller}");
            if (!request.IsSuccessStatusCode) throw new Exception(request.ReasonPhrase);
            var objects = await request.Content.ReadFromJsonAsync<BaseResult<List<T>>>();
            if (paged)
                Objects.AddRange(objects?.Data ?? throw new Exception("failed to add incoming list of objects to existing list"));
            else
                Objects = objects.Data ?? throw new Exception("No objects were found");
            return new BaseResult<List<T>>()
            {
                Succeeded = true,
                Data = Objects
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return
                new BaseResult<List<T>>()
                {
                    Succeeded = false,
                    Errors = new List<string>() { e.Message },
                    Message = e.Message,
                    Data = null
                };
        }
    }

    public virtual async Task<BaseResult<List<T>>> Search(string query, bool paged = false, int page = 0, int pageSize = 10, bool cached = false)
    {
        try
        {
            var httpClient = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IHttpClientFactory>().CreateClient("Auth");
            var request = await httpClient.GetAsync($"api/{typeof(T).Name}/search?query={query}");
            if (!request.IsSuccessStatusCode) throw new Exception(request.ReasonPhrase);
            var objects = await request.Content.ReadFromJsonAsync<BaseResult<List<T>>>();
            // Objects = objects.Data ?? throw new Exception("No objects were found");
            return new BaseResult<List<T>>()
            {
                Succeeded = true,
                Data = Objects
            };
        }
        catch (Exception e)
        {
            return
                new BaseResult<List<T>>()
                {
                    Succeeded = false,
                    Errors = new List<string>() { e.Message },
                    Message = e.Message,
                    Data = null
                };
        }
    }

    public virtual async Task<BaseResult<T>> Create(T dto)
    {
        try
        {
            var httpClient = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IHttpClientFactory>().CreateClient("Auth");
            var request = await httpClient.PostAsJsonAsync($"api/{typeof(T).Name}", dto);
            if (!request.IsSuccessStatusCode) throw new Exception(request.ReasonPhrase);
            var obj = (await request.Content.ReadFromJsonAsync<BaseResult<T>>()).Data;
            Objects.Add(obj ?? throw new($"{nameof(T)} is null"));
            return new BaseResult<T>()
            {
                Succeeded = true,
                Data = obj
            };
        }
        catch (Exception e)
        {
            return
                new BaseResult<T>()
                {
                    Succeeded = false,
                    Errors = new List<string>() { e.Message },
                    Message = e.Message,
                    Data = null
                };
        }
    }

    public virtual async Task<BaseResult> Delete(TKey id)
    {
        try
        {
            var httpClient = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IHttpClientFactory>().CreateClient("Auth");
            var request = await httpClient.DeleteAsync($"api/{typeof(T).Name}/{id}");
            if (!request.IsSuccessStatusCode) throw new Exception(request.ReasonPhrase);
            return new BaseResult()
            {
                Succeeded = true
            };
        }
        catch (Exception e)
        {
            return
                new BaseResult
                {
                    Succeeded = false,
                    Errors = new List<string>() { e.Message },
                    Message = e.Message,
                    Data = null
                };
        }
    }

    public virtual async Task<BaseResult<T>> Update(T t)
    {
        try
        {
            var httpClient = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IHttpClientFactory>().CreateClient("Auth");
            var request = await httpClient.PutAsJsonAsync($"api/{typeof(T).Name}", t);
            if (!request.IsSuccessStatusCode) throw new Exception(request.ReasonPhrase);
            T obj = (await request.Content.ReadFromJsonAsync<BaseResult<T>>()).Data ?? throw new($"{nameof(T)} is null");
            var selected = obj;
            return new BaseResult<T>()
            {
                Succeeded = true,
                Data = selected
            };
        }
        catch (Exception e)
        {
            return
                new BaseResult<T>()
                {
                    Succeeded = false,
                    Errors = new List<string>() { e.Message },
                    Message = e.Message,
                    Data = null
                };
        }
    }

}

public class BaseService<T, TNew, TKey, TResult>(IServiceScopeFactory serviceScopeFactory) : BaseService<T, TKey>(serviceScopeFactory), IBaseService<T, TNew, TKey> where T : class, IHasKey<TKey>, new() where TKey : IEquatable<TKey> where TNew : class
{
    public virtual async Task<BaseResult<T>> Create(TNew dto)
    {
        try
        {
            var httpClient = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IHttpClientFactory>().CreateClient("Auth");
            var request = await httpClient.PostAsJsonAsync($"api/{typeof(T).Name}", dto);
            if (!request.IsSuccessStatusCode) throw new Exception(request.ReasonPhrase);
            var obj = (await request.Content.ReadFromJsonAsync<BaseResult<T>>()).Data;
            Objects.Add(obj ?? throw new Exception("object cant be null"));
            return new BaseResult<T>()
            {
                Succeeded = true,
                Data = obj
            };
        }
        catch (Exception e)
        {
            return
                new BaseResult<T>()
                {
                    Succeeded = false,
                    Errors = new List<string>() { e.Message },
                    Message = e.Message,
                    Data = null
                };
        }
    }
}