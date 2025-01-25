namespace ZambeziDigital.Base.DTOs.Auth;



public interface ILoginRequestDto
{
    public string UserName { get; set; }
    public string Password { get; set; }
}

