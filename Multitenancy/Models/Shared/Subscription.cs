namespace ZambeziDigital.Multitenancy.Models.Shared;

public class Subscription : BaseModel
{
    public TimeSpan Period { get; set; }
    public decimal Price { get; set; }
    public Duration Duration { get; set; }
    
    
    //Navigation Properties
    public List<Tenant>? Tenants { get; set; } = new();
}

public enum Duration
{
    Weekly, Monthly, Yearly
}