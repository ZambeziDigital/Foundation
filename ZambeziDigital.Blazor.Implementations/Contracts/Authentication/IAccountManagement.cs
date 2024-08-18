
using ZambeziDigital.Base.Implementation.Models;
using ZambeziDigital.Blazor.Implementations.Models.Authentication;

namespace ZambeziDigital.Blazor.Implementations.Contracts.Authentication;

public interface IAccountManagement
{
    /// <summary>
    /// Login service.
    /// </summary>
    /// <param name="email">User's email.</param>
    /// <param name="password">User's password.</param>
    /// <returns>The result of the request serialized to <see cref="FormResult"/>.</returns>
    public Task<FormResult> LoginAsync(string email, string password);

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
    /// <returns>The result of the request serialized to <see cref="FormResult"/>.</returns>
    public Task<FormResult> RegisterAsync(BaseApplicationUserAddRequest baseApplicationUser);

    public Task<bool> CheckAuthenticatedAsync();
}
