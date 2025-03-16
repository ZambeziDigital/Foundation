using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ZambeziDigital.AspNetCore.Abstractions.Services;
using ZambeziDigital.Base.Implementation.Services;
using ZambeziDigital.Base.Models;
using ZambeziDigital.Functions.Helpers;

namespace ZambeziDigital.AspNetCore.Implementations.Generics.Services;
public class BaseService<T, Tkey, TContext>(TContext context)
    : IDbBaseService<T, Tkey>
    where T : class, IHasKey<Tkey>, new()
    where Tkey : IEquatable<Tkey>
    where TContext : DbContext
{
    protected readonly TContext context = context;

    public virtual async Task<BaseListResult<T>> Get(bool paged = false, int page = 0, int pageSize = 10, bool cached = false, string? sortBy = null, bool reversed = false, DateTime startDate = default, DateTime endDate = default)
    
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

/// <summary>
/// Method to search for items based on a query string, with optional pagination and sorting.
/// </summary>
/// <param name="query">The query string to search for.</param>
/// <param name="paged">Whether to apply pagination.</param>
/// <param name="page">The page number to retrieve.</param>
/// <param name="pageSize">The number of items per page.</param>
/// <param name="cached">Whether to use cached results.</param>
/// <param name="sortBy">The property to sort by.</param>
/// <param name="reversed">Whether to sort in descending order.</param>
/// <returns>A task that represents the asynchronous operation. The task result contains a BaseListResult of items.</returns>
/// <remarks>
/// <para>
/// Determine Sorting Property: The code first determines which property to sort by. If the specified sortBy property is not found, it defaults to sorting by "Id".
/// </para>
/// <para>
/// Get Searchable Properties: It retrieves all properties of the Item class that are annotated with the Searchable attribute.
/// </para>
/// <para>
/// Initial Queryable: It starts with the full set of items from the database context.
/// </para>
/// <para>
/// Build Dynamic Filter:
/// <list type="bullet">
/// <item><description>If a query string is provided, it dynamically builds a LINQ expression to filter items.</description></item>
/// <item><description>It creates a parameter expression representing an item (x).</description></item>
/// <item><description>It gets the Contains method of the string class.</description></item>
/// <item><description>It builds a list of expressions to check if any searchable property contains the query string.</description></item>
/// <item><description>It combines all these expressions with OR logic.</description></item>
/// <item><description>It creates a lambda expression representing the combined OR expression.</description></item>
/// <item><description>It applies this lambda expression as a filter to the queryable.</description></item>
/// </list>
/// </para>
/// <para>
/// Apply Sorting and Pagination:
/// <list type="bullet">
/// <item><description>If pagination is requested, it applies sorting based on the specified property and order.</description></item>
/// <item><description>It then applies pagination by skipping and taking the appropriate number of items.</description></item>
/// <item><description>If no pagination is requested, it just gets the filtered and sorted results.</description></item>
/// </list>
/// </para>
/// <para>
/// Get Total Count: It gets the total count of items matching the query.
/// </para>
/// <para>
/// Return Results: It returns the results in a BaseListResult object.
/// </para>
/// <para>
/// Exception Handling: If any exceptions occur, it handles them and returns an error result.
/// </para>
/// </remarks>
///

    public virtual async Task<BaseListResult<T>> Search(string query, bool paged = false, int page = 0, int pageSize = 10, bool cached = false, string? sortBy = null, bool reversed = false, DateTime startDate = default, DateTime endDate = default)
    {
        try
        {
            var result = await SearchAsQueryableAsync(query, paged, page, pageSize, cached, sortBy, reversed, startDate, endDate);
            
            var queryable = result.Data;

            // Get the total count of items matching the query
            int totalSearchResults = await queryable.CountAsync();

            // Return the results in a BaseListResult object
            return new BaseListResult<T>
            {
                Succeeded = true,
                Data = await result.Data.ToListAsync(),
                TotalCount = totalSearchResults,
                CurrentPage = page,
                PageSize = pageSize,
                SortBy = sortBy
            };
        }
        catch (Exception e)
        {
            // Handle any exceptions and return an error result
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

    public virtual async Task<BaseResult<IQueryable<T>>> SearchAsQueryableAsync(string query, bool paged = false,
        int page = 0, int pageSize = 10, bool cached = false, string? sortBy = null, bool reversed = false,
        DateTime startDate = default, DateTime endDate = default)
    {
        try
        {
            var Property = typeof(T).GetProperties().FirstOrDefault(x => x.Name == sortBy);
            if (Property == null)
            {
                Property = typeof(T).GetProperties().FirstOrDefault(x => x.Name == "Id");
            }

            var searchProperties = typeof(T).GetProperties()
                .Where(x => x.GetCustomAttributes(typeof(Searchable), true).Any()).ToList();
            IQueryable<T> queryable = context.Set<T>();
            if (!string.IsNullOrEmpty(query))
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                var likeExpressions = searchProperties.Select(p =>
                {
                    var propertyAccess = Expression.Property(parameter, p.Name);
                    Expression likeExpression = null;

                    if (p.PropertyType == typeof(string))
                    {
                        likeExpression = Expression.Call(propertyAccess, containsMethod, Expression.Constant(query));
                    }
                    else if (p.PropertyType == typeof(int) || p.PropertyType == typeof(double) ||
                             p.PropertyType == typeof(float))
                    {
                        var convertedQuery = Convert.ChangeType(query, p.PropertyType);
                        likeExpression = Expression.Equal(propertyAccess, Expression.Constant(convertedQuery));
                    }
                    else if (p.PropertyType == typeof(bool))
                    {
                        if (bool.TryParse(query, out var boolQuery))
                        {
                            likeExpression = Expression.Equal(propertyAccess, Expression.Constant(boolQuery));
                        }
                    }
                    else if (p.PropertyType.IsEnum)
                    {
                        var enumValue = Enum.Parse(p.PropertyType, query);
                        likeExpression = Expression.Equal(propertyAccess, Expression.Constant(enumValue));
                    }

                    return likeExpression;
                }).Where(expr => expr != null).ToList();

                if (likeExpressions.Any())
                {
                    var orExpression =
                        likeExpressions.Aggregate<Expression>((accumulate, next) => Expression.OrElse(accumulate, next));
                    var lambda = Expression.Lambda<Func<T, bool>>(orExpression, parameter);
                    queryable = queryable.Where(lambda);
                }
            }

            // Add date filtering
            if (startDate != default || endDate != default)
            {
                //check is the property is a date and is anotation with the DigitalFilterableDate attribute
                var dateProperties = typeof(T).GetProperties().Where(p => p.PropertyType == typeof(DateTime) &&
                                                                         p.GetCustomAttributes(typeof(DigitalFilterableDate),
                                                                             true).Any()).ToList();
                if (dateProperties.Any())
                {
                    var parameter = Expression.Parameter(typeof(T), "x");
                    Expression dateExpression = null;

                    if (startDate != default)
                    {
                        var startDateExpressions = dateProperties.Select(p =>
                        {
                            var propertyAccess = Expression.Property(parameter, p.Name);
                            var startDateExpression =
                                Expression.GreaterThanOrEqual(propertyAccess, Expression.Constant(startDate));
                            return startDateExpression;
                        });
                        dateExpression =
                            startDateExpressions.Aggregate<Expression>((accumulate, next) =>
                                Expression.OrElse(accumulate, next));
                    }

                    if (endDate != default)
                    {
                        var endDateExpressions = dateProperties.Select(p =>
                        {
                            var propertyAccess = Expression.Property(parameter, p.Name);
                            var endDateExpression =
                                Expression.LessThanOrEqual(propertyAccess, Expression.Constant(endDate));
                            return endDateExpression;
                        });
                        var endDateExpression =
                            endDateExpressions.Aggregate<Expression>((accumulate, next) =>
                                Expression.OrElse(accumulate, next));
                        dateExpression = dateExpression == null
                            ? endDateExpression
                            : Expression.AndAlso(dateExpression, endDateExpression);
                    }

                    var dateLambda = Expression.Lambda<Func<T, bool>>(dateExpression, parameter);
                    queryable = queryable.Where(dateLambda);
                }
            }

            if (paged)
            {
                queryable = reversed
                    ? queryable.OrderByDescending(x => Property.GetValue(x))
                    : queryable.OrderBy(x => Property.GetValue(x));
                queryable = queryable
                    .Skip(page * pageSize)
                    .Take(pageSize);
            }

            return new BaseResult<IQueryable<T>> { Succeeded = true, Data = queryable };
        }
        catch (Exception e)
        {
            return new BaseResult<IQueryable<T>>
                { Succeeded = false, Errors = new List<string> { e.Message }, Message = e.Message };
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
    where T : class, IHasKey<Tkey>, new()
    where Tkey : IEquatable<Tkey>
    where TNew : class
    where TContext : DbContext
{
    public abstract Task<BaseResult<T>> Create(TNew dto);
}

public class BaseService<T, TKey>(DbContext context) : BaseService<T, TKey, DbContext>(context)
    where T : class, IHasKey<TKey>, new()
    where TKey : IEquatable<TKey>;
    
public class BaseService<T>(DbContext context) : BaseService<T, int, DbContext>(context)
    where T : class, IHasKey<int>, new();