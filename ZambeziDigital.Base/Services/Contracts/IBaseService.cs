using ZambeziDigital.Base.Contracts;
using ZambeziDigital.Base.Contracts.Base;
using ZambeziDigital.Base.Models;

namespace ZambeziDigital.Base.Services.Contracts;

public interface IBaseService<T, TKey> 
    where T : class, IHasKey<TKey>, new() 
    where TKey : IEquatable<TKey>
{
    Task<BaseListResult<T>> Get(bool paged = false, int page = 0, int pageSize = 10, bool cached = false, string? sortBy = null, bool reversed = false);
    Task<BaseListResult<T>> Search(string query, bool paged = false, int page = 0, int pageSize = 10, bool cached = false, string? sortBy = null, bool reversed = false); 
    Task<BaseResult<T>> Create(T dto);
    List<T> Objects { get; set; }
    Task<BaseResult<T>> Get(TKey id, bool cached = false);
    Task<BaseResult<T>> Update(T t);
    Task<BaseResult> Delete(TKey id);
    Task<BaseResult> Delete(List<TKey> id);
    Task<BaseResult> Delete(List<SelectableModel<T>> selectableModels);
    Task<BaseResult<IQueryable<T>>> SearchAsQueryableAsync(string query);
}


public interface IBaseService<T, TNew, TKey> : IBaseService<T, TKey> 
    where T : class, IHasKey<TKey>, new() 
    where TKey : IEquatable<TKey> 
    where TNew : class
{
    Task<BaseResult<T>> Create(TNew dto);
}