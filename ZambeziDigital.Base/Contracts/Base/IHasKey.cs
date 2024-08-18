namespace ZambeziDigital.Base.Contracts.Base;

public interface IHasKey<TKey> where TKey : IEquatable<TKey>
{
    [Key] TKey Id { get; set; }
}
