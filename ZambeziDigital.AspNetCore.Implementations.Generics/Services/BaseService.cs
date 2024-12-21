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

    public virtual async Task<BaseListResult<T>> Get(bool paged = false, int page = 0, int pageSize = 10, bool cached = false, string? sortBy = null, bool reversed = false)
    {
        var Property = typeof(T).GetProperties().FirstOrDefault(x => x.Name == sortBy);
        if (Property == null)
        {
            Property = typeof(T).GetProperties().FirstOrDefault(x => x.Name == "Id");
        }
        return new BaseListResult<T>
        {
            Succeeded = true,
            Data = paged
                ? reversed ? await context.Set<T>()
                    // .OrderByDescending(x => Property.GetValue(x))
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .ToListAsync()
                    : await context.Set<T>()
                    // .OrderBy(x => Property.GetValue(x))
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .ToListAsync()
                : reversed ?
                    await context.Set<T>()
                    // .OrderByDescending(x => Property.GetValue(x))
                    .ToListAsync()
                    : await context.Set<T>()
                    .ToListAsync(),
            TotalCount = await context.Set<T>().CountAsync(),
            CurrentPage = page,
            PageSize = pageSize,
            SortBy = sortBy
        };

    }

    public virtual async Task<BaseListResult<T>> Search(string query, bool paged = false, int page = 0, int pageSize = 10, bool cached = false, string? sortBy = null, bool reversed = false)
    {
        try
        {
            var Property = typeof(T).GetProperties().FirstOrDefault(x => x.Name == sortBy);
            if (Property == null)
            {
                Property = typeof(T).GetProperties().FirstOrDefault(x => x.Name == "Id");
            }
            return new BaseListResult<T>
            {
                Succeeded = true,
                Data = paged
                    ? reversed ? await context.Set<T>().Where(x => x.SearchString.Contains(query))
                            // .OrderByDescending(x => Property.GetValue(x))
                            .Skip(page * pageSize)
                            .Take(pageSize)
                            .ToListAsync()
                        : await context.Set<T>()
                            // .OrderBy(x => Property.GetValue(x))
                            .Skip(page * pageSize)
                            .Take(pageSize)
                            .ToListAsync()
                    : reversed ?
                        await context.Set<T>().Where(x => x.SearchString.Contains(query))
                            // .OrderByDescending(x => Property.GetValue(x))
                            .ToListAsync()
                        : await context.Set<T>()
                            .ToListAsync(),
                TotalCount = await context.Set<T>().Where(x => x.SearchString.Contains(query)).CountAsync(),
                CurrentPage = page,
                PageSize = pageSize,
                SortBy = sortBy
            };
        }
        catch (Exception e)
        {
           var result = new BaseListResult<T>
           {
               Succeeded = false,
               Errors = new List<string>
               {
                   e.Message
               },
               Message = e.Message,
               Data = new(),
               TotalCount = 0,
               CurrentPage = 0,
               PageSize = 0,
               SortBy = null
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
        var entity = await context.Set<T>().FirstOrDefaultAsync(x => x.Id.Equals(id));
        if (entity == null)
        {
            return new BaseResult { Succeeded = false, Errors = ["Entity not found"] };
        }
        entity.IsDeleted = true;
        await context.SaveChangesAsync();
        return new BaseResult { Succeeded = true, Errors = ["Entity deleted"], Message = "Entity deleted" };
    }

    public async Task<BaseResult> Delete(List<Tkey> ids)
    {
        try
        {
            var entities = await context.Set<T>().Where(x => ids.Contains(x.Id)).ToListAsync();
            entities.ForEach(x => x.IsDeleted = true);
            await context.SaveChangesAsync();
            return new BaseResult { Succeeded = true, Errors = ["Entities deleted"], Message = $"{ids.Count} deleted" };
        }
        catch (Exception e)
        {
            return new BaseResult { Succeeded = false, Errors = [e.Message], Message = e.Message };
        }
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

public class BaseService<T, TKey>(DbContext context) : BaseService<T, TKey, DbContext>(context)
    where T : class, IHasKey<TKey>, ISearchable, new()
    where TKey : IEquatable<TKey>;
    
public class BaseService<T>(DbContext context) : BaseService<T, int, DbContext>(context)
    where T : class, IHasKey<int>, ISearchable, new();