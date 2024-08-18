using ZambeziDigital.Base.DTOs.Auth;

namespace ZambeziDigital.Base.Implementation.Models;

public class ForgotPasswordRequest : IForgotPasswordRequest
{
    public string Email { get; set; }
}