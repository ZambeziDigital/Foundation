using System.Text.Json.Serialization;

namespace ZambeziDigital.Base.Models;
/// <summary>
/// BaseModel is a generic base class that provides common properties and functionality for entities.
/// </summary>
/// <typeparam name="TKey">The type of the entity's key.</typeparam>
public class BaseModel<TKey> : IHasKey<TKey>, IBaseModel<TKey>
    where TKey : IEquatable<TKey>
{
    /// <summary>
    /// This is the primary key of the entity. You do not pass it, it is generated automatically.
    /// </summary>
    public TKey Id { get; set; }
    /// <summary>
    /// This is the name of the entity. It is required.
    /// </summary>
    public virtual string Name { get; set; }
    /// <summary>
    /// This is the date and time the entity was created. It is generated automatically if not passed
    /// It is set to the date of posting this entity.
    /// </summary>
    // [JsonIgnore]
    public virtual DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    /// <summary>
    ///  This is the date and time the entity was last updated. It is generated automatically if not passed
    /// </summary>
    public virtual DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    [JsonIgnore]
    public bool IsDeleted  { get; set; } = false;
    //of the id is int set it to 0, if it is of type string set it to Guid
    /// <summary>
    ///  This is the default constructor for the BaseModel class.
    /// </summary>
    public BaseModel()
    {
        if (typeof(TKey) == typeof(int))
        {
            Id = (TKey)Convert.ChangeType(0, typeof(TKey));
        }
        else if (typeof(TKey) == typeof(string))
        {
            Id = (TKey)Convert.ChangeType(Guid.NewGuid().ToString(), typeof(TKey));
        }
    }
    public override string ToString()
    {
        return base.ToString();
    }
    [JsonIgnore]
    public virtual string SearchString => $"{Name} {CreatedAt} {UpdatedAt}";
}

// public class BaseModel : IHasKey<int>
// {
//     public int Id { get; set; }
//     public string Name { get; set; }
//     public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
//     public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

//     public override string ToString()
//     {
//         return base.ToString();
//     }
// }
