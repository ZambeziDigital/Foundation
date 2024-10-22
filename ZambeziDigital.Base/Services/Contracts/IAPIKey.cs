using ZambeziDigital.Base.Enums;

namespace ZambeziDigital.Base.Services.Contracts;

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