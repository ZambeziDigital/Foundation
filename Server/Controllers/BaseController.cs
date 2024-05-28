
namespace ZambeziDigital.Server.Controllers;



[ApiController]
// [EnableCors]
[Route("api/[controller]")]
public class BaseController<T, TKey>(IBaseService<T, TKey> service) : ControllerBase where T : class, IHasKey<TKey>, new() where TKey : IEquatable<TKey> 
{
    [HttpPost]
    public virtual async Task<T> Create(T entity)
    {
        return await service.Create(entity);
    }
    [HttpPut]
    public virtual async Task<ActionResult<T>> Update(T entity)
    {
        return await service.Update(entity);
    }
    [HttpDelete("{id}")]
    public virtual  async Task<ActionResult<BasicResult>> Delete(TKey id)
    {
        try
        {
            await service.Delete(id);
            return new BasicResult()
            {
                Succeeded = true,
            };
        }
        catch(Exception e)
        {
            return (new BasicResult()
            {
                Succeeded = false, Errors = [e.Message], Object = null
            });
        }
    }
    [HttpGet("{id}")]
    public virtual  async Task<T> Get(TKey id)
    {
        return await service.Get(id);
    }
    [HttpGet]
    public virtual  async Task<List<T>> Get()
    {
        return await service.Get();
    }
}



[ApiController]
// [EnableCors]
[Route("api/[controller]")]
public class BaseController<T, TNew, TKey>(IBaseService<T, TNew, TKey> service) : ControllerBase 
    where T : class, IHasKey<TKey>, new()
    where TKey : IEquatable<TKey>
    where TNew : class
{
    [HttpPost]
    public async Task<T> Create(TNew entity)
    {
        return await service.Create(entity);
    }
    [HttpPut]
    public async Task<T> Update(T entity)
    {
        return await service.Update(entity);
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult<BasicResult>> Delete(TKey id)
    {
        try
        {
            await service.Delete(id);
            return new BasicResult()
            {
                Succeeded = true,
            };
        }
        catch(Exception e)
        {
            return (new BasicResult()
            {
                Succeeded = false, Errors = [e.Message], Object = null
            });
        }
    }
    [HttpGet("{id}")]
    public async Task<T> Get(TKey id)
    {
        return await service.Get(id);
    }
    [HttpGet]
    public async Task<List<T>> Get()
    {
        return await service.Get();
    }
}
