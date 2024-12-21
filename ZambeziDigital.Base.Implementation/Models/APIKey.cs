using ZambeziDigital.Base.Contracts.Base;
using ZambeziDigital.Base.Enums;
using ZambeziDigital.Base.Models;
using ZambeziDigital.Base.Services.Contracts;
using ZambeziDigital.Functions.Helpers;

namespace ZambeziDigital.Base.Implementation.Models;

public class APIKey : BaseModel<int>, IAPIKey, ISearchable
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
