using ZambeziDigital.Base.Contracts.Base;
using ZambeziDigital.Base.Contracts.Tenancy;
using ZambeziDigital.Base.Models;

namespace ZambeziDigital.Base.Implementation.Models;

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