global using Microsoft.AspNetCore.Mvc;
global using ZambeziDigital.Base.Services.Contracts;
global using ZambeziDigital.Base.Contracts.Base;
global using ZambeziDigital.AspNetCore.Abstractions.Controllers;
global using Serilog;
global using ZambeziDigital.Base.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace ZambeziDigital.AspNetCore.Implementations.Generics.Controllers;

[ApiController]

[Route("api/[controller]")]
/// <summary>
/// BaseController is a generic controller class that provides basic CRUD operations for entities.
/// </summary>
/// <typeparam name="T">The type of the entity.</typeparam>
/// <typeparam name="TKey">The type of the entity's key.</typeparam>
public class BaseController<T, TKey>(IBaseService<T, TKey> service) :
    ControllerBase, IBaseController<T, TKey>
        where T : class, IHasKey<TKey>, new() where TKey : IEquatable<TKey>
{
    /// <summary>
    /// Creates a new entity.
    /// </summary>
    /// <param name="entity">The entity to create.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created entity.</returns>
    [HttpPost]
    [SwaggerOperation(
        Summary = $"Create",
        Description = "Create entity"
        )]
    public virtual async Task<ActionResult<BaseResult<T>>> Create(T entity)
    {
        Log.Information("Creating entity {@entity}", entity);
        return await service.Create(entity);

    }
    
    
    /// <summary>
    /// Updates an existing entity.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated entity.</returns>
    [HttpPut]
    [SwaggerOperation(
        Summary = $"Update",
        Description = "Update entity"
        )]
    public virtual async Task<ActionResult<BaseResult<T>>> Update(T entity)
    {
        Log.Information("Updating entity {@entity}", entity);
        return await service.Update(entity);
    }
    
    
    /// <summary>
    /// Updates an existing entity.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated entity.</returns>
    [HttpDelete("{id}")]
    [SwaggerOperation(
        Summary = $"Delete by Id",
        Description = "Delete entity by Id", Tags = new[] { "Delete" },
        OperationId = "Delete" + nameof(T) + "ById"
        )]
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
    
    /// <summary>
    /// Gets an entity by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the entity to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the retrieved entity.</returns>
    [HttpGet("{id}")]
    [SwaggerOperation(
        Summary = $"Get by Id",
        Description = "Get entity by Id"
        )]
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
    
    /// <summary>
    /// Gets a list of entities with optional pagination.
    /// </summary>
    /// <param name="pageNumber">The page number for pagination (optional).</param>
    /// <param name="pageSize">The page size for pagination (optional).</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of entities.</returns>
    [HttpGet]
    [SwaggerOperation(
        Summary = "Get all",
        Description = "Get"
        )]
    public virtual async Task<ActionResult<BaseResult<List<T>>>> Get(int? pageNumber = null, int? pageSize = null)
    {
        return await service.Get(pageNumber is not null, pageNumber ?? 0, pageSize ?? 10);
    }
    
    /// <summary>
    /// Searches for entities based on a query string with optional pagination.
    /// </summary>
    /// <param name="query">The search query string.</param>
    /// <param name="paged">Indicates whether to paginate the results.</param>
    /// <param name="page">The page number for pagination.</param>
    /// <param name="pageSize">The page size for pagination.</param>
    /// <param name="cached">Indicates whether to use cached results.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of entities that match the search query.</returns>
    [HttpGet("Search")]
    [SwaggerOperation(
        Summary = "Search",
        Description = "Search for entities by SearchString"
        )]
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
