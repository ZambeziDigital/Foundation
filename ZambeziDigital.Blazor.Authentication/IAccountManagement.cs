using ZambeziDigital.Base.DTOs.Auth;
using ZambeziDigital.Base.Models;

namespace ZambeziDigital.Blazor.Authentication;

public interface IAccountManagement<TUser>  
    where TUser : class, IApplicationUserAddRequest
{
    /// <summary>
    /// Login service.
    /// </summary>
    /// <param name="email">User's email.</param>
    /// <param name="password">User's password.</param>
    /// <returns>The result of the request serialized to <see cref="TResult"/>.</returns>
    public Task<BaseResult> LoginAsync(string email, string password);

    /// <summary>
    /// Log out the logged in user.
    /// </summary>
    /// <returns>The asynchronous task.</returns>
    public Task LogoutAsync();

    /// <summary>
    /// Registration service.
    /// </summary>
    /// <param name="email">User's email.</param>
    /// <param name="password">User's password.</param>
    /// <returns>The result of the request serialized to <see cref="TResult"/>.</returns>
    public Task<BaseResult> RegisterAsync(TUser applicationUser);

    public Task<bool> CheckAuthenticatedAsync();
}
