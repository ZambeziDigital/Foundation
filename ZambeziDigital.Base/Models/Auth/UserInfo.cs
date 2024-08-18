namespace ZambeziDigital.Base.Models.Auth;

public interface IUserInfo 
{
    public  string UserId { get; set; }
    public  string Email { get; set; }
    public   string Name { get; set; }
    public  List<string> Roles { get; set; }
}