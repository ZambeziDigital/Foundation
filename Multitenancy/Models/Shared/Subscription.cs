global using ZambeziDigital.Multitenancy.Models.Base;

namespace ZambeziDigital.Multitenancy.Models.Shared;

public interface ISubscription : IBaseModel<int>
{
    public TimeSpan Period { get; set; }
    public double Price { get; set; }
    public Duration Duration { get; set; }
    //Navigation Properties
    public List<ITenant>? Tenants { get; set; } 
}

public enum Duration
{
    Weekly, Monthly, Yearly
}