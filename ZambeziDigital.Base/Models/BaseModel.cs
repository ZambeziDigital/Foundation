using System.Text.Json.Serialization;

namespace ZambeziDigital.Base.Models;
public class BaseModel<TKey> : IHasKey<TKey>, IBaseModel<TKey>
    where TKey : IEquatable<TKey>
{
    public TKey Id { get; set; }
    public string Name { get; set; }
    // [JsonIgnore]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    // [JsonIgnore]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    //of the id is int set it to 0, if it is of type string set it to Guid
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
    public virtual string SearchString { get; }
}

// public class BaseModel : IHasKey<int>
// {
//     public int Id { get; set; }
//     public string Name { get; set; }
//     public DateTime CreatedAt { get; set; } = DateTime.Now;
//     public DateTime UpdatedAt { get; set; } = DateTime.Now;

//     public override string ToString()
//     {
//         return base.ToString();
//     }
// }
