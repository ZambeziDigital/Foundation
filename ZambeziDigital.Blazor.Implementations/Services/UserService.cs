using System.Net;
using System.Net.Http.Json;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using ZambeziDigital.Base.DTOs.Auth;
using ZambeziDigital.Base.Implementation.DTOs;
using ZambeziDigital.Base.Implementation.Models;
using ZambeziDigital.Base.Services.Contracts;
using ZambeziDigital.Base.Services.Contracts.Auth;
using ZambeziDigital.Base.Implementation.Models;
using ZambeziDigital.Base.Models;

namespace ZambeziDigital.Blazor.Implementations.Services;
public class BaseUserService(IHttpClientFactory httpClientFactory, NavigationManager navigationManager) :
    AuthenticationStateProvider, IUserService<BaseApplicationUser, BaseApplicationUserInfo, LoginRequest>, IBaseService<BaseApplicationUser, string>
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("Auth");
    private bool _authenticated = false;
    private readonly ClaimsPrincipal Unauthenticated =
        new(new ClaimsIdentity());
    // private readonly string PHONE_NUMBER_REGEX = @"^260(9[5-7]\d{7}|7[6-7]\d{7})$";
    private readonly string PHONE_NUMBER_REGEX = @"^260[97][5-7]\d{7}$";
    private BaseApplicationUserInfo? _basicInfo;


    public BaseApplicationUser? CurrentlyLoggedInUser { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public List<BaseApplicationUser> Users { get; set; } = new();

    public async Task<BaseApplicationUser> Create(BaseApplicationUserAddRequest dto)
    {
        var request = await _httpClient.PostAsJsonAsync($"api/User", dto);
        if (!request.IsSuccessStatusCode)
        {
            var result = await request.Content.ReadFromJsonAsync<BaseResult>();
            throw new Exception(result?.Errors?[0] ?? request.ReasonPhrase);
        }
        var user = await request.Content.ReadFromJsonAsync<BaseApplicationUser>();
        Users.Add(user ?? throw new Exception("Response from adding user object did not return a user object"));
        return user;
    }

    public Task<BaseResult> Delete(string id)
    {
        var request = _httpClient.DeleteAsync($"api/User/{id}");
        if (!request.Result.IsSuccessStatusCode)
        {
            var result = request.Result.Content.ReadFromJsonAsync<BaseResult>();
            throw new Exception(result.Result?.Errors?[0] ?? request.Result.ReasonPhrase);
        }
        return request.Result.Content.ReadFromJsonAsync<BaseResult>();

    }

    public async Task<BaseResult> Delete(List<string> id)
    {
        var request = await _httpClient.PostAsJsonAsync($"api/User/Delete", id);
        if (!request.IsSuccessStatusCode)
        {
            var result = await request.Content.ReadFromJsonAsync<BaseResult>();
            throw new Exception(result?.Errors?[0] ?? request.ReasonPhrase);
        }
        return await request.Content.ReadFromJsonAsync<BaseResult>();
    }

    public async Task<BaseResult> Delete(List<SelectableModel<BaseApplicationUser>> selectableModels)
    {
        return await Delete(selectableModels.Where(x => x.Selected).Select(x => x.Object.Id).ToList());
    }

    public async Task<BaseResult<BaseApplicationUser>> Login(LoginRequest loginDto)
    {
        var request = await _httpClient.PostAsJsonAsync($"api/User/Login", loginDto);
        if (request.StatusCode == HttpStatusCode.BadRequest)
            // return await request.Content.ReadFromJsonAsync<BaseResult<BaseApplicationUser>>() ?? 
            throw new Exception("Error processing login request response");
        if (request.StatusCode == HttpStatusCode.NotFound)
            // return await request.Content.ReadFromJsonAsync<BaseResult<BaseApplicationUser>>() ?? 
            throw new ArgumentException("The Email or Password is wrong");
        if (!request.IsSuccessStatusCode) throw new Exception(request.ReasonPhrase);

        var result = await request.Content.ReadFromJsonAsync<BaseResult<BaseApplicationUser>>();
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        CurrentUser = result.Succeeded ? result.Data : null;
        return new BaseResult<BaseApplicationUser>()
        {
            Succeeded = result.Succeeded,
            Data = result.Data,
            Errors = result.Errors
        };
    }

    public async Task<BaseResult> Logout(string? page)
    {
        var result = await _httpClient.PostAsync("api/User/Logout", null);
        var response = await result.Content.ReadFromJsonAsync<BaseResult>();
        if (!response.Succeeded) throw new ApplicationException("Failed to log out");
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        if (page is null)
            navigationManager.NavigateTo("/Account/Login");
        else
            navigationManager.NavigateTo(page);
        return new();
    }

    public async Task<BaseResult> AssignRole(string userId, string role)
    {
        var request = await _httpClient.GetAsync($"api/User/AssignRole/{userId}/{role}");
        if (!request.IsSuccessStatusCode)
        {
            var result = request.Content.ReadFromJsonAsync<BaseResult>();
            throw new Exception(result.Result?.Errors?[0] ?? request.ReasonPhrase);
        }
        var r = await request.Content.ReadFromJsonAsync<BaseResult<BaseApplicationUser>>();
        return new BaseResult()
        {
            Succeeded = r.Succeeded,
            Errors = r.Errors,
            Data = r.Data
        };


    }

    public BaseApplicationUser? CurrentUser { get; set; }


    public async Task<BaseResult> AddToRoleAsync(string Id, string role)
    {
        throw new NotImplementedException();
    }


    public BaseApplicationUserInfo? BasicInfo
    {
        get => _basicInfo;
        set => _basicInfo = value;
    }

    public async Task<string> GetUserIdAsync(BaseApplicationUser user)
    {
        var request = await _httpClient.PostAsJsonAsync($"api/User/GetUserId", user);
        if (!request.IsSuccessStatusCode)
        {
            var result = await request.Content.ReadFromJsonAsync<BaseResult>();
            throw new Exception(result?.Errors?[0] ?? request.ReasonPhrase);
        }
        else return await request.Content.ReadAsStringAsync();
    }





    // public UserInfo? BasicInfo { get; set; }

    public Task Delete(BaseApplicationUser entity)
    {
        throw new NotImplementedException();
    }

    public async Task<List<BaseApplicationUser>> Get()
    {
        if (Users is not null && Users.Count > 0) return Users;
        var request = await _httpClient.GetAsync($"api/User");
        if (!request.IsSuccessStatusCode) throw new Exception(request.ReasonPhrase);
        var objects = await request.Content.ReadFromJsonAsync<List<BaseApplicationUser>>();
        Users = objects ?? throw new Exception("No objects were found");
        return Users;
    }
    public async Task<List<BaseApplicationUser>> GetOrganizers()
    {
        // if (Users is not null && Users.Count > 0) return Users;
        var request = await _httpClient.GetAsync($"api/User/Organizers");
        if (!request.IsSuccessStatusCode) throw new Exception(request.ReasonPhrase);
        var objects = await request.Content.ReadFromJsonAsync<List<BaseApplicationUser>>();
        // Users = objects ?? throw new Exception("No objects were found");
        return objects ?? throw new Exception("No objects were found");
    }

    public Task<BaseResult<BaseApplicationUser>> Get(string id, bool cached = false)
    {
        var user = Users.FirstOrDefault(x => x.Id.Equals(id));
        if (user is null || !cached)
        {
            var request = _httpClient.GetAsync($"api/User/{id}");
            if (!request.Result.IsSuccessStatusCode) throw new Exception(request.Result.ReasonPhrase);
            user = request.Result.Content.ReadFromJsonAsync<BaseApplicationUser>().Result;
        }
        return Task.FromResult(new BaseResult<BaseApplicationUser>()
        {
            Succeeded = user is not null,
            Data = user,
            Errors = user is null ? new List<string>() { $"User with Id {id} not found" } : new List<string>()
        });
    }

    public async Task<BaseResult<BaseApplicationUser>> Update(BaseApplicationUser t)
    {
        var request = await _httpClient.PutAsJsonAsync($"api/User", t);
        if (!request.IsSuccessStatusCode) throw new Exception(request.ReasonPhrase);
        var user = await request.Content.ReadFromJsonAsync<BaseApplicationUser>();
        return new BaseResult<BaseApplicationUser>()
        {
            Succeeded = true,
            Data = user
        };
    }

    async Task<BaseResult> IBaseService<BaseApplicationUser, string>.Delete(string id)
    {
        var request = await _httpClient.DeleteAsync($"api/User/{id}");
        if (!request.IsSuccessStatusCode) throw new Exception(request.ReasonPhrase);
        return await request.Content.ReadFromJsonAsync<BaseResult>();
    }

    public async Task<BaseResult<BaseApplicationUser?>> FindByEmailAsync(string email)
    {
        var request = await _httpClient.GetAsync($"api/User/FindByEmailAsync/{email}");
        if (!request.IsSuccessStatusCode)
        {
            return null;
            var result = await request.Content.ReadFromJsonAsync<BaseResult>();
            throw new Exception(result?.Errors?[0] ?? request.ReasonPhrase);
        };
        var user = await request.Content.ReadFromJsonAsync<BaseApplicationUser>();
        return new BaseResult<BaseApplicationUser?>()
        {
            Succeeded = user is not null,
            Data = user,
            Errors = user is null ? new List<string>() { "User not found" } : new List<string>()
        };
    }

    public async Task<BaseResult<BaseApplicationUser>> RequestPasswordReset(IForgotPasswordRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<BaseResult<BaseApplicationUser>> ResetPasswordRequest(IResetPasswordRequest request)
    {
        throw new NotImplementedException();
    }




    public async Task<BaseResult> RequestPasswordReset(ForgotPasswordRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<BaseResult> ResetPasswordRequest(ResetPasswordRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<BaseResult> RequestPasswordReset(string email)
    {
        var request = await _httpClient.GetAsync($"api/User/ForgotPassword/{email}");
        if (!request.IsSuccessStatusCode)
        {
            var result = await request.Content.ReadFromJsonAsync<BaseResult>();
            throw new Exception(result?.Errors?[0] ?? request.ReasonPhrase);
        }
        return await request.Content.ReadFromJsonAsync<BaseResult>();
    }

    public async Task<BaseResult> RequestResetPassword(ForgotPasswordRequest requestDto)
    {
        throw new NotImplementedException();
    }


    // public async Task<BaseResult<BaseApplicationUser>> Login(ILoginRequestDto loginDto)
    // {
    //     // var request = await _httpClient.PostAsJsonAsync($"login?useCookies=true&useSessionCookies=true", loginDto);
    //     var request = await _httpClient.PostAsJsonAsync($"api/User/Login", loginDto);
    //     if (request.StatusCode == HttpStatusCode.BadRequest)
    //         return await request.Content.ReadFromJsonAsync<BaseResult<BaseApplicationUser>>() ?? throw new Exception("Error processing login request response");
    //     if (request.StatusCode == HttpStatusCode.NotFound)
    //         return await request.Content.ReadFromJsonAsync<BaseResult<BaseApplicationUser>>() ?? throw new ArgumentException("The Email or Password is wrong");
    //     if (!request.IsSuccessStatusCode) throw new Exception(request.ReasonPhrase);
    //     
    //     var result = await request.Content.ReadFromJsonAsync<BaseResult<BaseApplicationUser>>();
    //     NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    //     CurrentUser = result.Succeeded ? result.Object : null;
    //     return result;
    // }





    public async Task<BaseApplicationUser?> Put(BaseApplicationUser entity)
    {
        try
        {
            //check if the phone number is valid
            if (!Regex.IsMatch(entity.PhoneNumber, PHONE_NUMBER_REGEX))
            {
                throw new Exception("The phone number is invalid, Phone number should be in the format 260XXXXXXXXX");
            }
            var result = await _httpClient.PutAsJsonAsync($"api/User", entity);
            var response = await result.Content.ReadFromJsonAsync<BaseResult<BaseApplicationUser>>();
            // if (!response.Succeeded) throw new Exception(response.Errors?[0] ?? "Error processing user update request response");
            var user = response.Data;
            var _user = Users.First(x => x.Id.Equals(user.Id)); //= user;
            _user = user;
            return user;

            return await result.Content.ReadFromJsonAsync<BaseApplicationUser>() ?? throw new Exception("Error processing user update request response");


        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        _authenticated = false;

        // default to not authenticated
        var user = Unauthenticated;

        try
        {
            // the user info endpoint is secured, so if the user isn't logged in this will fail
            // var userResponse = await _httpClient.GetAsync("Manage/Info");
            var userResponse = await _httpClient.GetAsync("api/User/Manage/Info");

            // throw if user info wasn't retrieved
            userResponse.EnsureSuccessStatusCode();

            // user is authenticated,so let's build their authenticated identity
            var userJson = await userResponse.Content.ReadAsStringAsync();
            var userInfo = await userResponse.Content.ReadFromJsonAsync<BaseApplicationUserInfo>();
            // BasicInfo = userInfo;
            if (userInfo != null)
            {
                // in our system name and email are the same
                var claims = new List<Claim>();
                claims.Add(new(ClaimTypes.Name, userInfo.Email));
                claims.Add(new(ClaimTypes.Email, userInfo.Email));
                claims.Add(new(ClaimTypes.NameIdentifier, userInfo.UserId));
                claims.Add(new(ClaimTypes.Role, "Admin"));




                // set the principal
                var id = new ClaimsIdentity(claims, nameof(BaseApplicationUser));
                user = new ClaimsPrincipal(id);
                _authenticated = true;
            }
        }
        catch (AuthenticationException ex)
        {
            await Logout("Not Authorized");
        }
        catch
        {
            // if we got here, the user isn't authenticated
            // so we'll return the unauthenticated user
            _authenticated = false;
        }

        // return the state
        return new AuthenticationState(user);
    }
    /// <summary>
    ///  This property should and is not supported in this class nor any other class in the client project
    /// </summary>
    /// <exception cref="NotSupportedException"></exception>

    // public List<ApplicationUser> Objects { get; set; } = new();
    public async Task<List<BaseApplicationUser>> Get(bool forceRefresh = false)
    {
        var request = await _httpClient.GetAsync("api/User");
        if (!request.IsSuccessStatusCode) throw new Exception(request.ReasonPhrase);
        var objects = await request.Content.ReadFromJsonAsync<List<BaseApplicationUser>>();
        Objects = objects ?? throw new Exception("No objects were found");
        return Objects;
    }

    public async Task<BaseApplicationUser> Get(int id)
    {
        var request = await _httpClient.GetAsync($"api/User/{id}");
        if (!request.IsSuccessStatusCode) throw new Exception(request.ReasonPhrase);
        var user = await request.Content.ReadFromJsonAsync<BaseApplicationUser>();
        return user ?? throw new Exception("No objects were found");
    }

    public Task<BaseListResult<BaseApplicationUser>> Get(bool paged = false, int page = 0, int pageSize = 10, bool cached = false)
    {
        throw new NotImplementedException();
    }

    public Task<BaseListResult<BaseApplicationUser>> Search(string query, bool paged = false, int page = 0, int pageSize = 10, bool cached = false)
    {
        throw new NotImplementedException();
    }

    public Task<BaseListResult<BaseApplicationUser>> Get(bool paged = false, int page = 0, int pageSize = 10, bool cached = false, string? sortBy = null,
        bool reversed = false)
    {
        throw new NotImplementedException();
    }

    public Task<BaseListResult<BaseApplicationUser>> Search(string query, bool paged = false, int page = 0, int pageSize = 10, bool cached = false,
        string? sortBy = null, bool reversed = false)
    {
        throw new NotImplementedException();
    }

    public async Task<BaseResult<BaseApplicationUser>> Create(BaseApplicationUser dto)
    {
        var request = await _httpClient.PostAsJsonAsync("api/User", dto);
        if (!request.IsSuccessStatusCode) throw new Exception(request.ReasonPhrase);
        var user = await request.Content.ReadFromJsonAsync<BaseApplicationUser>();
        return new BaseResult<BaseApplicationUser>()
        {
            Succeeded = true,
            Data = user
        };
    }

    public List<BaseApplicationUser> Objects { get; set; }

    public async Task<BaseResult> Delete(int id)
    {
        var request = await _httpClient.DeleteAsync($"api/User/{id}");
        if (!request.IsSuccessStatusCode) throw new Exception(request.ReasonPhrase);
        return await request.Content.ReadFromJsonAsync<BaseResult>();
    }
}