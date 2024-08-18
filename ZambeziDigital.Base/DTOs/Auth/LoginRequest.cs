namespace ZambeziDigital.Base.DTOs.Auth;



public interface ILoginRequestDto
{
    public string Email { get; set; }
    public string Password { get; set; }
}

