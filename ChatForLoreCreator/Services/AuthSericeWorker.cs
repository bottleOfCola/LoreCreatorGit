using ChatForLoreCreator.DbStuff.Models;
using ChatForLoreCreator.DbStuff.Repositories;
using SharedForLoreCreator.Models;
using SharedForLoreCreator.Repositories;
using System.Net;

namespace ChatForLoreCreator.Services;

public class AuthSericeWorker : IDisposable, IRepositoryAsync<SharedUser>
{
    private const string AUTH_SERVICE_ADRESS = "https://localhost:7121/Auth/";
    private HttpClient _httpClient = new();

    public async Task<bool> IsServiceWork()
    {
        return true;
    }

    public async Task<bool> AnyAsync()
    {
        return (await _httpClient.GetAsync($"{AUTH_SERVICE_ADRESS}GetAllUsers")).StatusCode == HttpStatusCode.Found;
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

    public async Task<bool> IsExistAsync(int id)
    {
        var a = await _httpClient.GetAsync($"{AUTH_SERVICE_ADRESS}IsUserExist?id={id}");
        return a.StatusCode == HttpStatusCode.Found;
    }

    public async Task<bool> IsUserHavePermissionAsync(int userId, PermissionTypes permission)
    {
        var a = await _httpClient.GetAsync($"{AUTH_SERVICE_ADRESS}IsUserHavePermission?userId={userId}&permission={(int)permission}");
        if (a.StatusCode == HttpStatusCode.OK) return true;
        return false;
    }

    public void Dispose() => _httpClient.Dispose();
}
