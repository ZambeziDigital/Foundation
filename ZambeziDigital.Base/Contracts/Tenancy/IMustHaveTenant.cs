namespace ZambeziDigital.Base.Contracts.Tenancy;

public interface IMustHaveTenant
{
    public int? TenantId { get; set; }
}