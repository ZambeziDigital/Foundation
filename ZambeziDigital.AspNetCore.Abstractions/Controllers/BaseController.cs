global using ZambeziDigital.Base.Contracts.Base;

namespace ZambeziDigital.AspNetCore.Abstractions.Controllers;


public interface IBaseControllerBase<T, TKey> 
    where T : class, IHasKey<TKey>, new() 
    where TKey : IEquatable<TKey>
{
    [HttpPut]
    Task<ActionResult<BaseResult<T>>> Update(T entity);
    [HttpDelete("{id}")]
    Task<ActionResult<BaseResult>> Delete(TKey id);
    [HttpGet("{id}")]
    Task<ActionResult<BaseResult<T>>> Get(TKey id);
    [HttpGet]
    Task<ActionResult<BaseResult<List<T>>>> Get(int? pageNumber = null, int? pageSize = null);

    [HttpGet("Search")]
    Task<ActionResult<BaseResult<List<T>>>> Search(string query, bool paged = false, int page = 0, int pageSize = 10,
        bool cached = false);
}


public interface IBaseController<T, TKey> : IBaseControllerBase<T, TKey> 
    where T : class, IHasKey<TKey>, new() 
    where TKey : IEquatable<TKey>
{
    [HttpPost]
    Task<ActionResult<BaseResult<T>>> Create(T entity);
}

public interface IBaseController<T, TNew, TKey>  : IBaseControllerBase<T, TKey> 
    where T : class, IHasKey<TKey>, new()
    where TKey : IEquatable<TKey>
    where TNew : class

{
    [HttpPost]
    Task<ActionResult<BaseResult<T>>> Create(TNew entity);
}
