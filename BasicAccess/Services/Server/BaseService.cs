using MySqlConnector;

namespace ZambeziDigital.BasicAccess.Services.Server;
public class BaseService<T, Tkey, TContext>(IServiceScopeFactory serviceScopeFactory, TContext context)
    : IDbBaseService<T, Tkey, TContext>
    where T : class, IHasKey<Tkey>, new()
    where Tkey : IEquatable<Tkey>
    where TContext : DbContext
{
    protected readonly TContext context = context;//=
       // serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<TContext>();
    
    public virtual async Task<T> Create(T entity)
    {
        try
        { 
            var result = await context.Set<T>().AddAsync(entity);
            await context.SaveChangesAsync();
            return result.Entity;
        }
        catch (MySqlException sqlException)
        {
            if(sqlException.Message.Contains("Unknown database"))
                throw new Exception("Tenant invalid, Please log out, clear cookies and log in again.");
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public virtual async Task<T> Update(T entity)
    {
        try
        { 
            var result = context.Set<T>().Update(entity);
            await context.SaveChangesAsync();
            return result.Entity;
        }
        catch (MySqlException sqlException)
        {
            if(sqlException.Message.Contains("Unknown database"))
                throw new Exception("Tenant invalid, Please log out, clear cookies and log in again.");
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

    }

    public virtual async Task<BasicResult> Delete(Tkey id)
    {
        try
        { 
            var entity = await context.Set<T>().FindAsync(id);
            if (entity == null)
            {
                return new BasicResult { Succeeded = false, Errors = ["Entity not found"] };
            }
            context.Set<T>().Remove(entity);
            await context.SaveChangesAsync();
            return new BasicResult { Succeeded = true, Errors = ["Entity deleted"] };
        }
        catch (MySqlException sqlException)
        {
            if(sqlException.Message.Contains("Unknown database"))
                throw new Exception("Tenant invalid, Please log out, clear cookies and log in again.");
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
       
    }

    public virtual async Task<T> Get(Tkey id)
    {
        
        try
        {
            return await context.Set<T>().FindAsync(id);

        }
        catch (MySqlException sqlException)
        {
            if(sqlException.Message.Contains("Unknown database"))
                throw new Exception("Tenant invalid, Please log out, clear cookies and log in again.");
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public virtual async Task<List<T>> Get(bool force = false)
    {
        try
        {
            if (force || Objects == null || !Objects.Any())
            {
                Objects = await context.Set<T>().ToListAsync();
            }
            return Objects;
        }
        catch (MySqlException sqlException)
        {
            if(sqlException.Message.Contains("Unknown database"))
                throw new Exception("Tenant invalid, Please log out, clear cookies and log in again.");
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
       
    }

    public virtual List<T> Objects { get; set; }
}

// public class BaseServiceARD<TARD, TARDI, Tkey, TContext>(IServiceScopeFactory serviceScopeFactory, TContext context)
//     : BaseService<TARD, Tkey, TContext>(serviceScopeFactory, context)
//     where TARDI : ARDItem, IHasKey<Tkey>, new()
//     where TARD : ARDocument<TARDI>, IHasKey<Tkey>, new()
//     where Tkey : IEquatable<Tkey>
//     where TContext : DbContext
// {
//     public override async Task<List<TARD>> Get(bool force = false)
//     {
//         try
//         {
//             return context.Set<TARD>().Include(x => x.Items).ToList();
//         }
//         catch (MySqlException sqlException)
//         {
//             if(sqlException.Message.Contains("Unknown database"))
//                 throw new Exception("Tenant invalid, Please log out, clear cookies and log in again.");
//             throw;
//         }
//         catch (Exception e)
//         {
//             Console.WriteLine(e);
//             throw;
//         }
//     }
//
//     public override async Task<TARD> Get(Tkey id)
//     {
//         try
//         {
//             return await context.Set<TARD>().Include(x => x.Items).FirstOrDefaultAsync(x => x.Id.Equals(id));
//         }
//         catch (MySqlException sqlException)
//         {
//             if(sqlException.Message.Contains("Unknown database"))
//                 throw new Exception("Tenant invalid, Please log out, clear cookies and log in again.");
//             throw;
//         }
//         catch (Exception e)
//         {
//             Console.WriteLine(e);
//             throw;
//         }
//     }
// }
//


public abstract class BaseService<T, TNew, Tkey, TContext>(IServiceScopeFactory serviceScopeFactory, TContext context) : BaseService<T, Tkey, TContext>(serviceScopeFactory, context), 
    IDbBaseService<T, TNew, Tkey, TContext> 
    where T : class, IHasKey<Tkey>, new() 
    where Tkey : IEquatable<Tkey> 
    where TNew : class
    where TContext : DbContext
{
    public abstract Task<T> Create(TNew dto);
}
