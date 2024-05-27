global using ZambeziDigital.BasicAccess.Models;
namespace ZambeziDigital.Authentication.Services.Client;
public class UserService(IHttpClientFactory httpClientFactory, NavigationManager navigationManager) :  AuthenticationStateProvider, IUserService
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("Auth");
    private bool _authenticated = false;
    private readonly ClaimsPrincipal Unauthenticated = 
        new(new ClaimsIdentity());
    // private readonly string PHONE_NUMBER_REGEX = @"^260(9[5-7]\d{7}|7[6-7]\d{7})$";
    private readonly string PHONE_NUMBER_REGEX = @"^260[97][5-7]\d{7}$";
   

    public ApplicationUser? CurrentlyLoggedInUser { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public List<ApplicationUser> Users { get; set; } = new();

    public async Task<ApplicationUser> Create(ApplicationUserAddRequest dto)
    {
        var request = await _httpClient.PostAsJsonAsync($"api/User", dto);
        if (!request.IsSuccessStatusCode)
        {
            var result = await request.Content.ReadFromJsonAsync<BasicResult>();
            throw new Exception(result?.Errors?[0] ?? request.ReasonPhrase);
        }
        var user = await request.Content.ReadFromJsonAsync<ApplicationUser>();
        Users.Add(user ?? throw new Exception("Response from adding user object did not return a user object"));
        return user;
    }

    public Task Delete(string id)
    {
        var request = _httpClient.DeleteAsync($"api/User/{id}");
        if (!request.Result.IsSuccessStatusCode)
        {
            var result = request.Result.Content.ReadFromJsonAsync<BasicResult>();
            throw new Exception(result.Result?.Errors?[0] ?? request.Result.ReasonPhrase);
        }
        return request.Result.Content.ReadFromJsonAsync<BasicResult>();
        
    }

    public ApplicationUser? CurrentUser { get; set; }
    public async Task<bool> AddToRoleAsync(string Id, string role)
    {
        var request = await _httpClient.GetAsync($"api/User/AddToRoleAsync/{Id}/{role}");
        if (!request.IsSuccessStatusCode)
        {
            var result = await request.Content.ReadFromJsonAsync<BasicResult>();
            throw new Exception(result?.Errors?[0] ?? request.ReasonPhrase);
        }
        else return true;
        
    }

    public async Task<string> GetUserIdAsync(ApplicationUser user)
    {
        var request = await _httpClient.PostAsJsonAsync($"api/User/GetUserId", user);
        if (!request.IsSuccessStatusCode)
        {
            var result = await request.Content.ReadFromJsonAsync<BasicResult>();
            throw new Exception(result?.Errors?[0] ?? request.ReasonPhrase);
        }
        else return await request.Content.ReadAsStringAsync();
    }

    async Task<BasicResult> IUserService.AddToRoleAsync(string Id, string role)
    {
        throw new NotImplementedException();
    }

    public UserInfo? BasicInfo { get; set; }

    public Task Delete(ApplicationUser entity)
    {
        throw new NotImplementedException();
    }

    public async Task<List<ApplicationUser>> Get()
    {
        if (Users is not null && Users.Count > 0) return Users;
        var request = await _httpClient.GetAsync($"api/User");
        if (!request.IsSuccessStatusCode) throw new Exception(request.ReasonPhrase);
        var objects = await request.Content.ReadFromJsonAsync<List<ApplicationUser>>();
        Users = objects ?? throw new Exception("No objects were found");
        return Users;
    }
   public async Task<List<ApplicationUser>> GetOrganizers()
    {
        // if (Users is not null && Users.Count > 0) return Users;
        var request = await _httpClient.GetAsync($"api/User/Organizers");
        if (!request.IsSuccessStatusCode) throw new Exception(request.ReasonPhrase);
        var objects = await request.Content.ReadFromJsonAsync<List<ApplicationUser>>();
        // Users = objects ?? throw new Exception("No objects were found");
        return objects ?? throw new Exception("No objects were found");
    }

    public Task<ApplicationUser> Get(string id)
    {
        throw new NotImplementedException();
    }

    public async Task<ApplicationUser> Update(ApplicationUser t)
    {
        throw new NotImplementedException();
    }

    public async Task<ApplicationUser?> FindByEmailAsync(string email)
    {
        var request = await _httpClient.GetAsync($"api/User/FindByEmailAsync/{email}");
        if (!request.IsSuccessStatusCode)
        {
            return null;
            var result = await request.Content.ReadFromJsonAsync<BasicResult>();
            throw new Exception(result?.Errors?[0] ?? request.ReasonPhrase);
        };
        var user = await request.Content.ReadFromJsonAsync<ApplicationUser>();
        return user;
    }

    public async Task<BasicResult> RequestPasswordReset(ForgotPasswordRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<BasicResult> ResetPasswordRequest(ResetPasswordRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<BasicResult> RequestPasswordReset(string email)
    {
        var request = await _httpClient.GetAsync($"api/User/ForgotPassword/{email}");
        if (!request.IsSuccessStatusCode)
        {
            var result = await request.Content.ReadFromJsonAsync<BasicResult>();
            throw new Exception(result?.Errors?[0] ?? request.ReasonPhrase);
        }
        return await request.Content.ReadFromJsonAsync<BasicResult>();
    }

    public async Task<BasicResult> RequestResetPassword(ForgotPasswordRequest requestDto)
    {
        throw new NotImplementedException();
    }


    public async Task<BasicResult<ApplicationUser>> Login(LoginRequestDto loginDto)
    {
        // var request = await _httpClient.PostAsJsonAsync($"login?useCookies=true&useSessionCookies=true", loginDto);
        var request = await _httpClient.PostAsJsonAsync($"api/User/Login", loginDto);
        if (request.StatusCode == HttpStatusCode.BadRequest)
            return await request.Content.ReadFromJsonAsync<BasicResult<ApplicationUser>>() ?? throw new Exception("Error processing login request response");
        if (request.StatusCode == HttpStatusCode.NotFound)
            return await request.Content.ReadFromJsonAsync<BasicResult<ApplicationUser>>() ?? throw new ArgumentException("The Email or Password is wrong");
        if (!request.IsSuccessStatusCode) throw new Exception(request.ReasonPhrase);
        
        var result = await request.Content.ReadFromJsonAsync<BasicResult<ApplicationUser>>();
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        CurrentUser = result.Succeeded ? result.Object : null;
        return result;
    }

    public async Task<BasicResult> Logout(string? page = null)
    {
        var result = await _httpClient.PostAsync("api/User/Logout", null);
        var response = await result.Content.ReadFromJsonAsync<BasicResult>();
        if (!response.Succeeded) throw new ApplicationException("Failed to log out");
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        if(page is null)
            navigationManager.NavigateTo("/Account/Login");
        else 
            navigationManager.NavigateTo(page);
        return response;
    }

    public async Task<BasicResult> AssignRole(string userId, string role)
    {
        var request = await _httpClient.GetAsync($"api/User/AssignRole/{userId}/{role}");
        if (!request.IsSuccessStatusCode)
        {
            var result = request.Content.ReadFromJsonAsync<BasicResult>();
            throw new Exception(result.Result?.Errors?[0] ?? request.ReasonPhrase);
        }
        return await request.Content.ReadFromJsonAsync<BasicResult>();
    }


    public async Task<ApplicationUser?> Put(ApplicationUser entity)
    {
        try
        {
            //check if the phone number is valid
            if (!Regex.IsMatch(entity.PhoneNumber, PHONE_NUMBER_REGEX))
            {
                throw new Exception("The phone number is invalid, Phone number should be in the format 260XXXXXXXXX");
            }
            var result = await _httpClient.PutAsJsonAsync($"api/User", entity);
            var response = await result.Content.ReadFromJsonAsync<BasicResult<ApplicationUser>>();
            // if (!response.Succeeded) throw new Exception(response.Errors?[0] ?? "Error processing user update request response");
            var user = response.Object;
            var _user = Users.First(x => x.Id.Equals(user.Id)); //= user;
            _user = user;
            return user;
            
            return await result.Content.ReadFromJsonAsync<ApplicationUser>() ?? throw new Exception("Error processing user update request response");
            
            
        }
        catch(Exception ex)
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
            var userInfo = await userResponse.Content.ReadFromJsonAsync<UserInfo>();
            BasicInfo = userInfo;
            if (userInfo != null)
            {
                // in our system name and email are the same
                var claims = new List<Claim>();
                claims.Add(new(ClaimTypes.Name, userInfo.Email));
                claims.Add(new(ClaimTypes.Email, userInfo.Email));
                claims.Add(new(ClaimTypes.NameIdentifier, userInfo.UserId));
                claims.Add(new(ClaimTypes.Role, "Admin"));




                // set the principal
                var id = new ClaimsIdentity(claims, nameof(UserService));
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
    public DbContext context { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }

    public List<ApplicationUser> Objects { get; set; } = new();
    public async Task<List<ApplicationUser>> Get(bool forceRefresh = false)
    {
        var request = await _httpClient.GetAsync("api/User");
        if (!request.IsSuccessStatusCode) throw new Exception(request.ReasonPhrase);
        var objects = await request.Content.ReadFromJsonAsync<List<ApplicationUser>>();
        Objects = objects ?? throw new Exception("No objects were found");
        return Objects;
    }

    public async Task<ApplicationUser> Get(int id)
    {
        var request = await _httpClient.GetAsync($"api/User/{id}");
        if (!request.IsSuccessStatusCode) throw new Exception(request.ReasonPhrase);
        var user = await request.Content.ReadFromJsonAsync<ApplicationUser>();
        return user ?? throw new Exception("No objects were found");
    }

    public async Task<ApplicationUser> Create(ApplicationUser dto)
    {
        var request = await _httpClient.PostAsJsonAsync("api/User", dto);
        if (!request.IsSuccessStatusCode) throw new Exception(request.ReasonPhrase);
        var user = await request.Content.ReadFromJsonAsync<ApplicationUser>();
        return user ?? throw new Exception("No objects were found");
    }

    public async Task<BasicResult> Delete(int id)
    {
        var request = await _httpClient.DeleteAsync($"api/User/{id}");
        if (!request.IsSuccessStatusCode) throw new Exception(request.ReasonPhrase);
        return await request.Content.ReadFromJsonAsync<BasicResult>();
    }
}