// using System.Net.Http.Json;
// using Microsoft.Extensions.DependencyInjection;
// using ZambeziDigital.Base.Contracts.Base;
// using ZambeziDigital.Base.Implementation.Models;
// using ZambeziDigital.Base.Models;
// using ZambeziDigital.Base.Services.Contracts;
//
// namespace ZambeziDigital.Blazor.Implementations.Services;
// public class BaseService<T, TKey>(IServiceScopeFactory serviceScopeFactory) : IBaseService<T,TKey> 
//     
//     where T : class, IHasKey<TKey>, new() 
//     where TKey : IEquatable<TKey>
// {
//     public List<T> Objects { get; set; } = new();
//     public async Task<List<T>> Get(bool forceRefresh = false)
//     {
//         try
//         {
//             if(Objects.Count > 0 && !forceRefresh) return Objects;
//             var httpClient = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IHttpClientFactory>().CreateClient("Auth");
//             var controller = typeof(T).Name;
//             var request = await httpClient.GetAsync($"api/{controller}");
//             if (!request.IsSuccessStatusCode) throw new Exception(request.ReasonPhrase);
//             var objects = await request.Content.ReadFromJsonAsync<BaseResult<List<T>>>();
//             Objects = objects.Object ?? throw new Exception("No objects were found");
//             return  Objects;
//         }
//         catch (Exception e)
//         {
//             throw new Exception(e.Message);
//         }
//     }
//
//     public async Task<BaseResult<T>> Get(TKey id)
//     {
//         var obj = Objects.FirstOrDefault(x => x.Id.Equals(id));
//         if (obj is null)
//         {
//             var httpClient = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IHttpClientFactory>().CreateClient("Auth");
//             var request = await httpClient.GetAsync($"api/{typeof(T).Name}/{id}");
//             if (!request.IsSuccessStatusCode) throw new Exception(request.ReasonPhrase);
//             var result = await request.Content.ReadFromJsonAsync<BaseResult<T>>();
//             obj = result.Object ?? throw new Exception("No object found");
//                   
//         }
//         return new BaseResult<T>()
//         {
//             Succeeded = obj != null,
//             Object = obj,
//             Errors = obj == null ? new List<string>() { $"{typeof(T).Name} with Id {id} not found" } : new List<string>()
//         };
//     }
//
//     public async Task<T> Get(int id)
//     {
//         try
//         {
//             if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));
//             var obj = (await Get()).FirstOrDefault(t => t.Id.Equals(id)) 
//                       ?? (await Get(true)).FirstOrDefault(t => t.Id.Equals(id)) 
//                       ?? throw new Exception($"No object found with Id: {id}");
//             return obj;
//         }
//         catch (Exception e)
//         {
//             throw new Exception(e.Message);
//         }
//     }
//
//     public Task<BaseResult<List<T>>> Get(bool paged = false, int page = 0, int pageSize = 10, bool cached = false)
//     {
//         
//     }
//
//     public Task<BaseResult<List<T>>> Search(string query, bool paged = false, int page = 0, int pageSize = 10, bool cached = false)
//     {
//         
//     }
//
//     public async Task<BaseResult<T>> Create(T dto)
//     {
//         try
//         {
//             var httpClient = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IHttpClientFactory>().CreateClient("Auth");
//             var request = await httpClient.PostAsJsonAsync($"api/{typeof(T).Name}", dto);
//             if (!request.IsSuccessStatusCode) throw new Exception(request.ReasonPhrase);
//             var obj = await request.Content.ReadFromJsonAsync<BaseResult<T>>();
//             Objects.Add(obj.Object ?? throw new ($"{nameof(T)} is null"));
//             return new BaseResult<T>()
//             {
//                 Succeeded = true,
//                 Object = obj.Object
//             };
//         }
//         catch (Exception e)
//         {
//             throw new Exception(e.Message);
//         }
//     }
//
//     public async Task<BaseResult> Delete(TKey id)
//     {
//         throw new NotImplementedException();
//     }
//
//     public async Task<BaseResult<T>> Update(T t)
//     {
//         try
//         {
//             var httpClient = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IHttpClientFactory>().CreateClient("Auth");
//             var request = await httpClient.PutAsJsonAsync($"api/{typeof(T).Name}", t);
//             if (!request.IsSuccessStatusCode) throw new Exception(request.ReasonPhrase);
//             var  obj = await request.Content.ReadFromJsonAsync<BaseResult<T>>() ?? throw new ($"{nameof(T)} is null");
//             var selected = obj.Object ?? throw new Exception("Object cant be null");
//             return new BaseResult<T>()
//             {
//                 Succeeded = true,
//                 Object = selected
//             };
//         }
//         catch (Exception e)
//         {
//             throw new Exception(e.Message);
//         }
//     }
//
//     // public async Task<TResult<>> Delete(int id)
//     // {
//     //     try
//     //     {
//     //         var httpClient = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IHttpClientFactory>().CreateClient("Auth");
//     //         var request = await httpClient.DeleteAsync($"api/{typeof(T).Name}/{id}");
//     //         if (!request.IsSuccessStatusCode) throw new Exception(request.ReasonPhrase);
//     //         var obj =  (await Get()).FirstOrDefault(x => x.Id.Equals(id) ) ?? throw new Exception("No object found");
//     //         Objects.Remove(obj);
//     //         return new TResult<object> { Succeeded = true, Errors = new List<string> { "Object deleted" } };
//     //     }
//     //     catch (Exception e)
//     //     {
//     //         throw new Exception(e.Message);
//     //     }
//     // }
// }
//
// public class BaseService<T, TNew, TKey>(IServiceScopeFactory serviceScopeFactory) 
//     : BaseService<T,TKey>(serviceScopeFactory),  IBaseService<T,TNew, TKey>  
//     where T : class, IHasKey<TKey>, new() 
//     where TKey : IEquatable<TKey> 
//     where TNew : class 
// {
//     public async Task<BaseResult<T>> Create(TNew dto)
//     {
//         try
//         {
//             var httpClient = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IHttpClientFactory>().CreateClient("Auth");
//             var request = await httpClient.PostAsJsonAsync($"api/{typeof(T).Name}", dto);
//             if (!request.IsSuccessStatusCode) throw new Exception(request.ReasonPhrase);
//             var obj = await request.Content.ReadFromJsonAsync<BaseResult<T>>();
//             Objects.Add(obj.Object ?? throw new Exception("object cant be null"));
//             return new BaseResult<T>()
//             {
//                 Succeeded = true,
//                 Object = obj.Object
//             };
//         }
//         catch (Exception e)
//         {
//             throw new Exception(e.Message);
//         }
//     }
// }