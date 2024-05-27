global using System.ComponentModel.DataAnnotations;

namespace ZambeziDigital.BasicAccess.Contracts;

public interface IBaseModel<TKey> : IHasKey<TKey> where TKey : IEquatable<TKey>
{
    [Key] TKey Id { get; set; }
    string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    string ToString();
}

