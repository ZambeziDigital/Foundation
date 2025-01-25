using ZambeziDigital.Base.DTOs.Auth;
namespace ZambeziDigital.Base.Implementation.Models;

public class ForgotPasswordRequest : IForgotPasswordRequest
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
}

