using System;
using System.ComponentModel.DataAnnotations;

namespace ZambeziDigital.Base.Contracts.Base;

public interface IBaseModel<TKey> : IHasKey<TKey> where TKey : IEquatable<TKey>
{
    [Key] TKey Id { get; set; }
    string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

