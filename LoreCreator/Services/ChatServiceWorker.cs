using LoreCreator.ViewModels;
using System.Net;

namespace LoreCreator.Services;

public class ChatServiceWorker
{
    private const string CHAT_SERVICE_ADRESS = "https://localhost:7250/Chat/";
    private HttpClient _httpClient = new();

    public async Task<bool> AddChat(string nameOfTable, int id)
    {
        using HttpResponseMessage response = await _httpClient.GetAsync($"{CHAT_SERVICE_ADRESS}AddChat?name={nameOfTable + id}");
        return response.StatusCode == HttpStatusCode.OK;
    }

    public async Task<bool> DeleteChat(string nameOfTable, int id)
    {
        using HttpResponseMessage response = await _httpClient.DeleteAsync($"{CHAT_SERVICE_ADRESS}DeleteChat?name={nameOfTable+id}");
        return response.StatusCode == HttpStatusCode.OK;
    }
}