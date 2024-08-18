using ZambeziDigital.Base.DTOs.Auth;

namespace ZambeziDigital.Base.Implementation.DTOs;

public class LoginDto : ILoginRequestDto
{
    public bool RememberMe { get; set; } = true;
    public string? Password { get; set; } = string.Empty;
    public string? Email { get; set; } = string.Empty;
    public bool? UseCookies { get; set; } = true;
    public bool? UseSessionCookies { get; set; } = true;
}
