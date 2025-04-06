using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;
using frontend.Services;

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

    protected async Task<ServiceResult<T>> GetAsync<T>(string endpoint)
    {
        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();
            T? data = await response.Content.ReadFromJsonAsync<T>();
            return new ServiceResult<T>(data);
        }
        catch (HttpRequestException ex)
        {
            return new ServiceResult<T>(default, ex.Message);
        }
        catch (Exception ex)
        {
            return new ServiceResult<T>(default, ex.Message);
        }
    }

    protected async Task<ServiceResult<TResponse>> PostAsync<TRequest, TResponse>(
        string endpoint,
        TRequest data
    )
    {
        try
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(endpoint, data);
            response.EnsureSuccessStatusCode();
            TResponse? responseData = await response.Content.ReadFromJsonAsync<TResponse>();
            return new ServiceResult<TResponse> { Data = responseData };
        }
        catch (HttpRequestException ex)
        {
            return new ServiceResult<TResponse>(default, ex.Message);
        }
        catch (Exception ex)
        {
            return new ServiceResult<TResponse>(default, ex.Message);
        }
    }

    protected async Task<ServiceResult> PostAsync<TRequest>(string endpoint, TRequest data)
    {
        try
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(endpoint, data);
            response.EnsureSuccessStatusCode();
            return new ServiceResult();
        }
        catch (HttpRequestException ex)
        {
            return new ServiceResult(ex.Message);
        }
        catch (Exception ex)
        {
            return new ServiceResult(ex.Message);
        }
    }

    protected async Task<ServiceResult<TResponse>> PutAsync<TRequest, TResponse>(
        string endpoint,
        TRequest data
    )
    {
        try
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync(endpoint, data);
            response.EnsureSuccessStatusCode();
            TResponse? responseData = await response.Content.ReadFromJsonAsync<TResponse>();
            return new ServiceResult<TResponse> { Data = responseData };
        }
        catch (HttpRequestException ex)
        {
            return new ServiceResult<TResponse>(default, ex.Message);
        }
        catch (Exception ex)
        {
            return new ServiceResult<TResponse>(default, ex.Message);
        }
    }

    protected async Task<ServiceResult> PutAsync<TRequest>(string endpoint, TRequest data)
    {
        try
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync(endpoint, data);
            response.EnsureSuccessStatusCode();
            return new ServiceResult();
        }
        catch (HttpRequestException ex)
        {
            return new ServiceResult(ex.Message);
        }
        catch (Exception ex)
        {
            return new ServiceResult(ex.Message);
        }
    }

    protected async Task<ServiceResult> DeleteAsync(string endpoint)
    {
        try
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync(endpoint);
            response.EnsureSuccessStatusCode();
            return new ServiceResult();
        }
        catch (HttpRequestException ex)
        {
            return new ServiceResult(ex.Message);
        }
        catch (Exception ex)
        {
            return new ServiceResult(ex.Message);
        }
    }
}
