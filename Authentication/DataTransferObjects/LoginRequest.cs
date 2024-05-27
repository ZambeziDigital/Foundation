namespace ZambeziDigital.Authentication.DataTransferObjects;

public record LoginRequest(
    string Email,
    string Password,
    string? TwoFactorCode = null,
    string? TwoFactorRecoveryCode = null);

public class LoginRequestDto
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string? TwoFactorCode { get; set; } = string.Empty!;
    public string? TwoFactorRecoveryCode { get; set; } = string.Empty!;
    public LoginRequestDto() { }

    public LoginRequestDto(
        string email,
        string password,
        string? twoFactorCode = null,
        string? twoFactorRecoveryCode = null)
    {
        Email = email;
        Password = password;
        TwoFactorCode = twoFactorCode;
        TwoFactorRecoveryCode= twoFactorRecoveryCode;
    }
    
    
}

