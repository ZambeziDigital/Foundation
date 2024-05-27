namespace ZambeziDigital.BasicAccess.Contracts;

public interface IHasKey<TKey> where TKey : IEquatable<TKey>
{
    [Key] TKey Id { get; set; }
}
