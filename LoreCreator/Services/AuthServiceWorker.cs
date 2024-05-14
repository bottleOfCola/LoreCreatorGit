using LoreCreator.ViewModels;
using SharedForLoreCreator.Models;
using SharedForLoreCreator.Repositories;
using System.Net;

namespace LoreCreator.Services;

public class AuthServiceWorker : IDisposable, IRepositoryAsync<SharedUser>
{
    private const string AUTH_SERVICE_ADRESS = "https://localhost:7121/Auth/";
    private HttpClient _httpClient = new();

    public async Task<bool> AddAsync(RegisterViewModel registerViewModel)
    {
        using HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"{AUTH_SERVICE_ADRESS}AddUser", registerViewModel);

        return response.StatusCode == HttpStatusCode.Accepted;
    }
    public async Task<bool> DeleteAsync(int id)
    {
        using HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"{AUTH_SERVICE_ADRESS}DeleteUser", new { id });
        return response.StatusCode == HttpStatusCode.Accepted;
    }

    public async Task<bool> AnyAsync()
    {
        return (await _httpClient.GetAsync($"{AUTH_SERVICE_ADRESS}AnyUser")).StatusCode == HttpStatusCode.Found;
    }

    public async Task<List<SharedUser>> GetAllAsync()
    {
        using HttpResponseMessage response = await _httpClient.GetAsync($"{AUTH_SERVICE_ADRESS}GetAllUsers");
        if (response.StatusCode == HttpStatusCode.NotFound) return new List<SharedUser>();
        return await response.Content.ReadFromJsonAsync<List<SharedUser>>();
    }

    public async Task<SharedUser?> GetByIdAsync(int id)
    {
        using HttpResponseMessage response = await _httpClient.GetAsync($"{AUTH_SERVICE_ADRESS}GetUser?id={id}");

        if (response.StatusCode == HttpStatusCode.NotFound) return null;
        return await response.Content.ReadFromJsonAsync<SharedUser>();
    }

    public async Task<SharedUser?> GetByEmailAndPasswordAsync(LoginViewModel loginViewModel)
    {
        using HttpResponseMessage response = await _httpClient.GetAsync($"{AUTH_SERVICE_ADRESS}GetUserByEmailAndPassword?email={loginViewModel.Email}&password={loginViewModel.Password}");

        if (response.StatusCode == HttpStatusCode.NotFound) return null;
        return await response.Content.ReadFromJsonAsync<SharedUser>();
    }

    public async Task<bool> IsUserHavePermissionAsync(int userId, PermissionTypes permission) =>
        (await _httpClient.GetAsync($"{AUTH_SERVICE_ADRESS}IsHavePermission?userId={userId}&permission={(int)permission}")).StatusCode == HttpStatusCode.Found;

    public async Task<bool> IsExistAsync(int id)
        => (await _httpClient.GetAsync($"{AUTH_SERVICE_ADRESS}isExistUser?id={id}")).StatusCode != HttpStatusCode.NotFound;

    public async Task<List<PermissionTypes>> GetUserPermissions(int id)
    {
        using HttpResponseMessage response = await _httpClient.GetAsync($"{AUTH_SERVICE_ADRESS}GetUserPermissions?userId={id}");
        if (response.StatusCode == HttpStatusCode.NotFound) return new List<PermissionTypes>();
        return await response.Content.ReadFromJsonAsync<List<PermissionTypes>>();
    }

    public async Task<bool> IsRoleExistAsync(int id)
    {
        using HttpResponseMessage response = await _httpClient.GetAsync($"{AUTH_SERVICE_ADRESS}IsRoleExist?roleId={id}");
        return response.StatusCode == HttpStatusCode.Found;
    }

    public async Task<SharedRole?> GetRoleAsync(int id)
    {
        using HttpResponseMessage response = await _httpClient.GetAsync($"{AUTH_SERVICE_ADRESS}GetRole?roleId={id}");
        if (response.StatusCode == HttpStatusCode.NotFound) return null;
        return await response.Content.ReadFromJsonAsync<SharedRole>();
    }


    public void Dispose()
    {
        _httpClient.Dispose();
    }
}
