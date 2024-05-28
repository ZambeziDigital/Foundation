namespace ZambeziDigital.BasicAccess.Services.Contracts;

public interface IBaseService<T, TKey> where T : class, IHasKey<TKey>, new() where TKey : IEquatable<TKey>
{
    Task<T> Create(T dto);
    List<T> Objects { get; set; }
    Task<List<T>> Get(bool forceRefresh = false);
    Task<T> Get(TKey id);
    Task<T> Update(T t);
    Task<BasicResult> Delete(TKey id);
}


public interface IBaseService<T, TNew, TKey> : IBaseService<T, TKey> where T : class, IHasKey<TKey>, new() where TKey : IEquatable<TKey> where TNew : class
{
    Task<T> Create(TNew dto);
}


public interface IDbBaseService<T, TKey, TContext> : IBaseService<T, TKey> where T : class, IHasKey<TKey>, new()
    where TKey : IEquatable<TKey>
    where TContext : DbContext
{
    // TContext context { get; set; }
    // protected readonly TContext context; //=

}




public interface IDbBaseService<T, TNew, TKey, TContext> : IBaseService<T, TKey>, IDbBaseService<T, TKey, TContext>
    where T : class, IHasKey<TKey>, new()
    where TKey : IEquatable<TKey>
    where TNew : class
    where TContext : DbContext;
