namespace ZambeziDigital.Authentication.DataTransferObjects;

public class ForgotPasswordRequest(string Email)
{
    public string Email { get; set; } = string.Empty!;
}

public class ResetPasswordRequest(string Email, string ResetCode, string NewPassword)
{
    public string Email { get; set; } = string.Empty!;
    public string ResetCode { get; set; } = string.Empty!;
    public string NewPassword { get; set; } = string.Empty!;
    public string ConfirmPassword { get; set; } = string.Empty!;
}