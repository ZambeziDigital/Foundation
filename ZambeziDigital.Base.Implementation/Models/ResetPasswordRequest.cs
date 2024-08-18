using ZambeziDigital.Base.DTOs.Auth;

namespace ZambeziDigital.Base.Implementation.Models;

public class ResetPasswordRequest : IResetPasswordRequest
{
    public string Email { get; set; }
    public string ResetCode { get; set; }
    public string NewPassword { get; set; }
    public string ConfirmPassword { get; set; }
}