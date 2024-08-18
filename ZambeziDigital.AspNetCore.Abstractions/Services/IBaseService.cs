namespace ZambeziDigital.AspNetCore.Abstractions.Services;


public interface IDbBaseService<T, TKey> : IBaseService<T, TKey> where T : class, IHasKey<TKey>, new()
    where TKey : IEquatable<TKey>;




public interface IDbBaseService<T, TNew, TKey> : IBaseService<T, TNew, TKey>, IDbBaseService<T, TKey>
    where T : class, IHasKey<TKey>, new()
    where TKey : IEquatable<TKey>
    where TNew : class;
