using System.Net;
using System.Net.Http.Json;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using ZambeziDigital.Base.DTOs.Auth;
using ZambeziDigital.Base.Models;
using ZambeziDigital.Base.Models.Auth;
using ZambeziDigital.Base.Services.Contracts;
using ZambeziDigital.Base.Services.Contracts.Auth;

namespace ZambeziDigital.Blazor.Implementations.Generics.Services;
public class UserService<TUser, TUserAdd, TUserInfo, TForgotPasswordRequest, TResetPasswordRequest, TLoginRequestDto>
    (IHttpClientFactory httpClientFactory, NavigationManager navigationManager) :
    AuthenticationStateProvider,
    IUserService<TUser, TUserAdd, TUserInfo, TForgotPasswordRequest, TResetPasswordRequest, TLoginRequestDto>, IBaseService<TUser, string>
    where TUser : class, IApplicationUser, new()
    where TUserAdd : class, IApplicationUserAddRequest
    where TUserInfo : class, IUserInfo
    where TForgotPasswordRequest : class
    where TResetPasswordRequest : class
    where TLoginRequestDto : class
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("Auth");
    private bool _authenticated = false;
    private readonly ClaimsPrincipal Unauthenticated =
        new(new ClaimsIdentity());
    // private readonly string PHONE_NUMBER_REGEX = @"^260(9[5-7]\d{7}|7[6-7]\d{7})$";
    private readonly string PHONE_NUMBER_REGEX = @"^260[97][5-7]\d{7}$";
    private TUserInfo? _basicInfo;


    public TUser? CurrentlyLoggedInUser { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public List<TUser> Users { get; set; } = new();

    public async Task<BaseResult<TUser>> Create(TUserAdd dto)
    {
        var request = await _httpClient.PostAsJsonAsync($"api/User", dto);
        if (!request.IsSuccessStatusCode)
        {
            var result = await request.Content.ReadFromJsonAsync<BaseResult<TUser>>();
            throw new Exception(result?.Errors?[0] ?? request.ReasonPhrase);
        }
        var user = await request.Content.ReadFromJsonAsync<TUser>();
        Users.Add(user ?? throw new Exception("Response from adding user object did not return a user object"));
        return new BaseResult<TUser>()
        {
            Succeeded = true,
            Data = user
        };
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
        var re = await request.Content.ReadFromJsonAsync<BaseResult>();
        return re.Succeeded ? await request.Content.ReadFromJsonAsync<BaseResult>() : throw new Exception(re.Errors?[0] ?? "Error assigning role to user");
    }


    public TUser? CurrentUser { get; set; }


    public async Task<BaseResult> AddToRoleAsync(string Id, string role)
    {
        throw new NotImplementedException();
    }

    public TUserInfo? BasicInfo { get; set; }

    // public UserInfo? BasicInfo { get; set; }

    public Task Delete(TUser entity)
    {
        throw new NotImplementedException();
    }

    public async Task<List<TUser>> Get()
    {
        if (Users is not null && Users.Count > 0) return Users;
        var request = await _httpClient.GetAsync($"api/User");
        if (!request.IsSuccessStatusCode) throw new Exception(request.ReasonPhrase);
        var objects = await request.Content.ReadFromJsonAsync<BaseResult<List<TUser>>>();
        Users = objects.Data ?? throw new Exception("No objects were found");
        return Users;
    }
    public async Task<List<TUser>> GetOrganizers()
    {
        // if (Users is not null && Users.Count > 0) return Users;
        var request = await _httpClient.GetAsync($"api/User/Organizers");
        if (!request.IsSuccessStatusCode) throw new Exception(request.ReasonPhrase);
        var objects = await request.Content.ReadFromJsonAsync<List<TUser>>();
        // Users = objects ?? throw new Exception("No objects were found");
        return objects ?? throw new Exception("No objects were found");
    }

    public Task<BaseResult<TUser>> Get(string id, bool cached = false)
    {
        var user = Users.FirstOrDefault(x => x.Id.Equals(id));
        if (user is null || !cached)
        {
            var request = _httpClient.GetAsync($"api/User/{id}");
            if (!request.Result.IsSuccessStatusCode) throw new Exception(request.Result.ReasonPhrase);
            user = request.Result.Content.ReadFromJsonAsync<TUser>().Result;
        }
        return Task.FromResult(new BaseResult<TUser>()
        {
            Succeeded = user != null,
            Data = user,
            Errors = user == null ? new List<string>() { $"User with Id {id} not found" } : new List<string>()
        });
    }

    public async Task<BaseResult<TUser>> Update(TUser t)
    {
        var request = await _httpClient.PutAsJsonAsync($"api/User", t);
        if (!request.IsSuccessStatusCode) throw new Exception(request.ReasonPhrase);
        var user = await request.Content.ReadFromJsonAsync<TUser>();
        return new BaseResult<TUser>()
        {
            Succeeded = true,
            Data = user
        };
    }

    public async Task<string> Delete(TUserAdd id)
    {
        var request = await _httpClient.DeleteAsync($"api/User/{id}");
        if (!request.IsSuccessStatusCode) throw new Exception(request.ReasonPhrase);
        return await request.Content.ReadAsStringAsync();
    }


    public async Task<BaseResult<TUser?>> FindByEmailAsync(string email)
    {
        var request = await _httpClient.GetAsync($"api/User/FindByEmailAsync/{email}");
        if (!request.IsSuccessStatusCode)
        {
            return null;
            var result = await request.Content.ReadFromJsonAsync<BaseResult<TUser>>();
            throw new Exception(result?.Errors?[0] ?? request.ReasonPhrase);
        };
        var user = await request.Content.ReadFromJsonAsync<TUser>();
        return new BaseResult<TUser?>()
        {
            Succeeded = true,
            Data = user
        };
    }

    public async Task<BaseResult> RequestPasswordReset(TForgotPasswordRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<BaseResult> ResetPasswordRequest(TResetPasswordRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<BaseResult> Login(TLoginRequestDto loginDto)
    {
        // var request = await _httpClient.PostAsJsonAsync($"login?useCookies=true&useSessionCookies=true", loginDto);
        var request = await _httpClient.PostAsJsonAsync($"api/User/Login", loginDto);
        if (request.StatusCode == HttpStatusCode.BadRequest) throw new Exception("The Email or Password is wrong");
        // return await request.Content.ReadFromJsonAsync<TResult>() ?? throw new Exception("Error processing login request response");
        if (request.StatusCode == HttpStatusCode.NotFound) throw new Exception("The Email or Password is wrong");
        // return await request.Content.ReadFromJsonAsync<TResult>() ?? throw new ArgumentException("The Email or Password is wrong");
        if (!request.IsSuccessStatusCode) throw new Exception(request.ReasonPhrase);

        var result = await request.Content.ReadFromJsonAsync<BaseResult>();
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        // CurrentUser = result.Succeeded ? result : null;
        return result;
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

    public async Task<BaseResult> RequestResetPassword(TForgotPasswordRequest requestDto)
    {
        throw new NotImplementedException();
    }






    public async Task<BaseResult> Put(TUser entity)
    {
        try
        {
            //check if the phone number is valid
            if (!Regex.IsMatch(entity.PhoneNumber, PHONE_NUMBER_REGEX))
            {
                throw new Exception("The phone number is invalid, Phone number should be in the format 260XXXXXXXXX");
            }
            var result = await _httpClient.PutAsJsonAsync($"api/User", entity);
            var response = await result.Content.ReadFromJsonAsync<BaseResult>();
            // if (!response.Succeeded) throw new Exception(response.Errors?[0] ?? "Error processing user update request response");
            var user = response.Data;
            // var _user = Users.First(x => x.Id.Equals(user.Id)); //= user;
            // _user = user;
            return response;



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
            BasicInfo = await userResponse.Content.ReadFromJsonAsync<TUserInfo>();
            // BasicInfo = userInfo;
            if (BasicInfo != null)
            {
                // in our system name and email are the same
                var claims = new List<Claim>();
                claims.Add(new(ClaimTypes.Name, BasicInfo.Email));
                claims.Add(new(ClaimTypes.Email, BasicInfo.Email));
                claims.Add(new(ClaimTypes.NameIdentifier, BasicInfo.UserId));
                claims.Add(new(ClaimTypes.Role, BasicInfo.Roles.First()));

                // set the principal
                var id = new ClaimsIdentity(claims, nameof(TUser));
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
    public async Task<List<TUser>> Get(bool forceRefresh = false)
    {
        var request = await _httpClient.GetAsync("api/User");
        if (!request.IsSuccessStatusCode) throw new Exception(request.ReasonPhrase);
        var objects = await request.Content.ReadFromJsonAsync<List<TUser>>();
        Objects = objects ?? throw new Exception("No objects were found");
        return Objects;
    }

    public async Task<TUser> Get(int id)
    {
        var request = await _httpClient.GetAsync($"api/User/{id}");
        if (!request.IsSuccessStatusCode) throw new Exception(request.ReasonPhrase);
        var user = await request.Content.ReadFromJsonAsync<TUser>();
        return user ?? throw new Exception("No objects were found");
    }

    public Task<BaseResult<List<TUser>>> Get(bool paged = false, int page = 0, int pageSize = 10, bool cached = false)
    {
        throw new NotImplementedException();
    }

    public Task<BaseResult<List<TUser>>> Search(string query, bool paged = false, int page = 0, int pageSize = 10, bool cached = false)
    {
        throw new NotImplementedException();
    }

    public async Task<BaseResult<TUser>> Create(TUser dto)
    {
        var request = await _httpClient.PostAsJsonAsync("api/User", dto);
        if (!request.IsSuccessStatusCode) throw new Exception(request.ReasonPhrase);
        var user = await request.Content.ReadFromJsonAsync<TUser>();
        return new BaseResult<TUser>()
        {
            Succeeded = true,
            Data = user
        };
    }

    public List<TUser> Objects { get; set; }

    public async Task<BaseResult> Delete(int id)
    {
        var request = await _httpClient.DeleteAsync($"api/User/{id}");
        if (!request.IsSuccessStatusCode) throw new Exception(request.ReasonPhrase);
        return await request.Content.ReadFromJsonAsync<BaseResult>();
    }
}