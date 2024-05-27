namespace ZambeziDigital.BasicAccess.Models;

public class BaseModel : IBaseModel<int>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public override string ToString()
    {
        return base.ToString();
    }
}
public class BaseModel<Tkey> : IBaseModel<Tkey> where Tkey : IEquatable<Tkey>
{
    public Tkey Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public BaseModel()
    {
        //if the TKey is a string then we will generate a new GUID
        if (typeof(Tkey) == typeof(string))
        {
            Id = (Tkey)(object)Guid.NewGuid().ToString();
        }
        //if the TKey is an int then we will set it to a default value
        else if (typeof(Tkey) == typeof(int))
        {
            Id = (Tkey)(object)0; // or any other default value you want
        }
    }
}