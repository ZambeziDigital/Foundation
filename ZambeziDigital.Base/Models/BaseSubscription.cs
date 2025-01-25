using ZambeziDigital.Base.Contracts.Tenancy;

namespace ZambeziDigital.Base.Models;

public class BaseSubscription<TTenant> : BaseModel<int>, ISubscription<TTenant>
where TTenant : class, ITenant, IHasKey<int>, new()
{
    public int Id  { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public TimeSpan Period { get; set; }
    public double Price { get; set; }
    public Duration Duration { get; set; }
    public List<TTenant>? Tenants { get; set; }
}