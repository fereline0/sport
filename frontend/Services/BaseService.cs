using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public abstract class BaseService
{
    private readonly HttpClient _httpClient;

    public BaseService(
        IHttpClientFactory httpClientFactory,
        string httpClientName = "DefaultAPIClient"
    )
    {
        _httpClient = httpClientFactory.CreateClient(httpClientName);
    }

    protected async Task<T> GetAsync<T>(string endpoint)
    {
        var response = await _httpClient.GetAsync(endpoint);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<T>();
    }

    protected async Task PostAsync<T>(string endpoint, T data)
    {
        var response = await _httpClient.PostAsJsonAsync(endpoint, data);
        response.EnsureSuccessStatusCode();
    }

    protected async Task PutAsync<T>(string endpoint, T data)
    {
        var response = await _httpClient.PutAsJsonAsync(endpoint, data);
        response.EnsureSuccessStatusCode();
    }

    protected async Task DeleteAsync(string endpoint)
    {
        var response = await _httpClient.DeleteAsync(endpoint);
        response.EnsureSuccessStatusCode();
    }
}
