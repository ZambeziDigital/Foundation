global using Microsoft.AspNetCore.Mvc;
global using ZambeziDigital.Base.Services.Contracts;
global using ZambeziDigital.Base.Contracts.Base;
global using ZambeziDigital.AspNetCore.Abstractions.Controllers;
global using Serilog;
global using ZambeziDigital.Base.Models;

namespace ZambeziDigital.AspNetCore.Implementations.Generics.Controllers;

[ApiController]
// [EnableCors]
[Route("api/[controller]")]
public class BaseController<T, TKey>(IBaseService<T, TKey> service) :
    ControllerBase, IBaseController<T, TKey>
        where T : class, IHasKey<TKey>, new() where TKey : IEquatable<TKey>
{
    [HttpPost]
    public virtual async Task<ActionResult<BaseResult<T>>> Create(T entity)
    {
        Log.Information("Creating entity {@entity}", entity);
        return await service.Create(entity);

    }
    [HttpPut]
    public virtual async Task<ActionResult<BaseResult<T>>> Update(T entity)
    {
        Log.Information("Updating entity {@entity}", entity);
        return await service.Update(entity);
    }
    [HttpDelete("{id}")]
    public virtual async Task<ActionResult<BaseResult>> Delete(TKey id)
    {
        try
        {
            Log.Information("Deleting entity with id {@id}", id);
            await service.Delete(id);
            Log.Information("Entity with id {@id} deleted", id);
            return new BaseResult()
            {
                Succeeded = true,
            };
        }
        catch (Exception e)
        {
            Log.Error(e, "Error deleting entity with id {@id}", id);
            return (new BaseResult()
            {
                Succeeded = false,
                Errors = [e.Message],
                Data = null
            });
        }
    }
    [HttpGet("{id}")]
    public virtual async Task<ActionResult<BaseResult<T>>> Get(TKey id)
    {
        try
        {
            Log.Information("Getting entity with id {@id}", id);
            return await service.Get(id);
            // return new BaseResult<T>()
            // {
            //     Succeeded = true, Errors = null, Object = result
            // };
        }
        catch (Exception e)
        {
            return (new BaseResult<T>()
            {
                Succeeded = false,
                Errors = [e.Message],
                Data = null
            });
        }

    }
    [HttpGet]
    public virtual async Task<ActionResult<BaseResult<List<T>>>> Get(int? pageNumber = null, int? pageSize = null)
    {
        return await service.Get(pageNumber is not null, pageNumber ?? 0, pageSize ?? 10);
    }
    [HttpGet("Search")]
    public virtual async Task<ActionResult<BaseResult<List<T>>>> Search(string query, bool paged = false, int page = 0, int pageSize = 10, bool cached = false)
    {
        return await service.Search(query, paged, page, pageSize, cached);
    }
}


[ApiController]
// [EnableCors]
[Route("api/[controller]")]
public class BaseController<T, TNew, TKey>(IBaseService<T, TKey> service) : ControllerBase,
    IBaseController<T, TNew, TKey>
    where T : class, IHasKey<TKey>, new()
    where TKey : IEquatable<TKey>
    where TNew : class
{
    [HttpPost]
    public virtual async Task<ActionResult<BaseResult<T>>> Create(T entity)
    {
        return await service.Create(entity);

    }
    [HttpPut]
    public virtual async Task<ActionResult<BaseResult<T>>> Update(T entity)
    {
        return await service.Update(entity);
    }
    [HttpDelete("{id}")]
    public virtual async Task<ActionResult<BaseResult>> Delete(TKey id)
    {
        try
        {
            await service.Delete(id);
            return new BaseResult()
            {
                Succeeded = true,
            };
        }
        catch (Exception e)
        {
            return (new BaseResult()
            {
                Succeeded = false,
                Errors = [e.Message],
                Data = null
            });
        }
    }
    [HttpGet("{id}")]
    public virtual async Task<ActionResult<BaseResult<T>>> Get(TKey id)
    {
        try
        {
            return await service.Get(id);
        }
        catch (Exception e)
        {
            return (new BaseResult<T>()
            {
                Succeeded = false,
                Errors = [e.Message],
                Data = null
            });
        }

    }
    [HttpGet]
    public virtual async Task<ActionResult<BaseResult<List<T>>>> Get(int? pageNumber = null, int? pageSize = null)
    {
        return await service.Get(pageNumber is not null, pageNumber ?? 0, pageSize ?? 10);
    }

    [HttpGet("Search")]
    public virtual async Task<ActionResult<BaseResult<List<T>>>> Search(string query, bool paged = false, int page = 0, int pageSize = 10, bool cached = false)
    {
        return await service.Search(query, paged, page, pageSize, cached);
    }

    [HttpPost("Create")]
    public async Task<ActionResult<BaseResult<T>>> Create(TNew entity)
    {
        throw new NotImplementedException();
    }
}

//For single Tenant systems
// public class BaseController<T, Tkey, TContext>(IBaseService<T, Tkey> service, TContext context) : 
//     BaseController<T, Tkey>(service) where T : class, IHasKey<Tkey>, new() where Tkey : IEquatable<Tkey>
// {
//     protected readonly TContext context = context;
// }
