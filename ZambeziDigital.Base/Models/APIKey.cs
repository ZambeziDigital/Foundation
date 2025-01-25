using ZambeziDigital.Base.Enums;
using ZambeziDigital.Functions.Helpers;

namespace ZambeziDigital.Base.Models;

public class APIKey : BaseModel<int>, IAPIKey
{
    [DigitalColumn]
    public string Key { get; set; }
    public int TenantId { get; set; }
    [DigitalColumn]
    public string Role { get; set; }
    public string? UserId { get; set; }
    [DigitalColumn]
    public string? UserName { get; set; }
    [DigitalColumn]
    public string? Email { get; set; }
    public string? Password { get; set; }
    [DigitalColumn]
    public string? Token { get; set; }
    [DigitalColumn]
    public string? Cookie { get; set; }
    [DigitalColumn]
    public string? Creator { get; set; }
    [DigitalColumn]
    public APIStatus Status { get; set; }
}
