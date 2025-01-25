namespace ZambeziDigital.Base.Implementation.Models;

public class LoginRequest : ILoginRequestDto
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public string? TwoFactorCode { get; set; } = string.Empty!;
    public string? TwoFactorRecoveryCode { get; set; } = string.Empty!;
    public bool RememberMe { get; set; }
}