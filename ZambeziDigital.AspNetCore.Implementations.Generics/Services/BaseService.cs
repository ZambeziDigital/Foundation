using Microsoft.EntityFrameworkCore;
using ZambeziDigital.AspNetCore.Abstractions.Services;
using ZambeziDigital.Base.Implementation.Services;
using ZambeziDigital.Base.Models;

namespace ZambeziDigital.AspNetCore.Implementations.Generics.Services;
public class BaseService<T, Tkey, TContext>(TContext context)
    : IDbBaseService<T, Tkey>
    where T : class, IHasKey<Tkey>, ISearchable, new()
    where Tkey : IEquatable<Tkey>
    where TContext : DbContext
{
    protected readonly TContext context = context;

    public virtual async Task<BaseResult<List<T>>> Get(bool paged = false, int page = 0, int pageSize = 10, bool cached = false)
    {

        return new BaseResult<List<T>>()
        {
            Succeeded = true,
            Data = paged
                ? await context.Set<T>().Skip(page * pageSize).Take(pageSize).ToListAsync()
                : await context.Set<T>().ToListAsync()
        };

    }

    public virtual async Task<BaseResult<List<T>>> Search(string query, bool paged = false, int page = 0, int pageSize = 10, bool cached = false)
    {
        try
        {
            
            return new BaseResult<List<T>>()
            {
                Succeeded = true,
                Data = (await Get(paged, page, pageSize, cached)).Data?.Where(x => x.SearchString.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList()

            };
        }
        catch (Exception e)
        {
           var result = new BaseResult<List<T>>()
            {
                Succeeded = false,
                Errors = new List<string> { e.Message },
                Message = e.Message,
                Data = new()
            };
            return result;
        }
    }

    public virtual async Task<BaseResult<T>> Create(T entity)
    {
        var result = await context.Set<T>().AddAsync(entity);
        await context.SaveChangesAsync();
        return new BaseResult<T>() { Succeeded = true, Data = result.Entity };
    }

    public virtual async Task<BaseResult<T>> Update(T entity)
    {
        var result = context.Set<T>().Update(entity);
        await context.SaveChangesAsync();
        return new BaseResult<T>() { Succeeded = true, Data = result.Entity };
    }

    public virtual async Task<BaseResult> Delete(Tkey id)
    {
        var entity = await context.Set<T>().FindAsync(id);
        entity.IsDeleted = true;
        if (entity == null)
        {
            return new BaseResult { Succeeded = false, Errors = ["Entity not found"] };
        }
        await context.SaveChangesAsync();
        return new BaseResult { Succeeded = true, Errors = ["Entity deleted"], Message = "Entity deleted" };
    }

    public async Task<BaseResult> Delete(List<Tkey> id)
    {
        var entities = await context.Set<T>().Where(x => id.Contains(x.Id)).ToListAsync();
        entities.ForEach(x => x.IsDeleted = true);
        await context.SaveChangesAsync();
        return new BaseResult { Succeeded = true, Errors = ["Entities deleted"], Message = $"{id.Count} deleted" };
    }

    public async Task<BaseResult> Delete(List<SelectableModel<T>> selectableModels)
    {
        return await Delete(selectableModels.Where(x => x.Selected).Select(x => x.Object.Id).ToList());
    }

    public virtual async Task<BaseResult<T>> Get(Tkey id, bool cached = false)
    {
        return new BaseResult<T>()
        {
            Succeeded = true,
            Data = await context.Set<T>().FindAsync(id)
        };
    }


    public virtual List<T> Objects { get; set; }

}

public abstract class BaseService<T, TNew, Tkey, TContext>(TContext context) :
    BaseService<T, Tkey, TContext>(context),
    IDbBaseService<T, TNew, Tkey>
    where T : class, IHasKey<Tkey>, ISearchable, new()
    where Tkey : IEquatable<Tkey>
    where TNew : class
    where TContext : DbContext
{
    public abstract Task<BaseResult<T>> Create(TNew dto);
}
