namespace ZambeziDigital.Base.DTOs.Auth;

public interface IForgotPasswordRequest
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
}

public interface IResetPasswordRequest
{
    public string Email { get; set; }
    public string ResetCode { get; set; }
    public string NewPassword { get; set; }
    public string ConfirmPassword { get; set; }
}