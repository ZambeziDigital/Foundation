namespace ZambeziDigital.Base.DTOs.Auth;

public interface IApplicationUserAddRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string? OtherNames { get; set; }
    public UserState Active { get; set; }
}


public enum UserState
{
    Active = 2,
    InActive = 1,
    Suspended = 3,
    Deleted = 4
}
