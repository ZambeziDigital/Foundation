namespace ZambeziDigital.Multitenancy.Models.Base;

public interface IMustHaveTenant
{
    public int TenantId { get; set; }
}