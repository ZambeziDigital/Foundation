namespace ZambeziDigital.Authentication.Models;

public class UserInfo
{
    public  string UserId { get; set; }
    public  string Email { get; set; }
    public   string Name { get; set; }
    public  List<string> Roles { get; set; }

    public UserInfo()
    {
        Roles = new(){"Admin"};
    }

    public UserInfo(ApplicationUser applicationUser)
    {
        UserId = applicationUser.Id;
        Email = applicationUser.Email;
        Name = applicationUser.Name;
        Roles = applicationUser.Roles.Select( r => r.Name).ToList();
    }
}