using ZambeziDigital.Base.Contracts.Base;
using ZambeziDigital.Base.Models;

namespace ZambeziDigital.Base.Implementation.Models;

public class APIKey : BaseModel<int>, IAPIKey, ISearchable
{
    public string Key { get; set; }
    public int TenantId { get; set; }
    public string Role { get; set; }
    public string? UserId { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? Token { get; set; }
    public string? Cookie { get; set; }
    public string? Creator { get; set; }
    public APIStatus Status { get; set; }
    // public Tenant? Tenant { get; set; }
}

public interface IAPIKey
{
    public string Key { get; set; }
    public int TenantId { get; set; }
    public string Role { get; set; }
    public string? UserId { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? Token { get; set; }
    public string? Cookie { get; set; }
    public string Creator { get; set; }
    public APIStatus Status { get; set; }
}
public enum APIStatus
{
    Active,
    Inactive,
    Suspended,
    Deleted
}
