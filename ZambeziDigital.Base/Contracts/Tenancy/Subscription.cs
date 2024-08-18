using ZambeziDigital.Base.Contracts.Base;

namespace ZambeziDigital.Base.Contracts.Tenancy;

public interface ISubscription<TTenant> : IBaseModel<int>
{
    public TimeSpan Period { get; set; }
    public double Price { get; set; }
    public Duration Duration { get; set; }
    //Navigation Properties
    public List<TTenant>? Tenants { get; set; } 
}

public enum Duration
{
    Weekly, Monthly, Yearly
}