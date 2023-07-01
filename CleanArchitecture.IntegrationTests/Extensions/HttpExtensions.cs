using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CleanArchitecture.Api.Models;

namespace CleanArchitecture.IntegrationTests.Extensions;

public static class HttpExtensions
{
    private static readonly JsonSerializerOptions s_jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private static T? Deserialize<T>(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return default;
        }

        return JsonSerializer.Deserialize<T>(json, s_jsonSerializerOptions);
    }

    private static string Serialize<T>(T data)
    {
        return JsonSerializer.Serialize(data, s_jsonSerializerOptions);
    }

    public static async Task<ResponseMessage<T>?> ReadAsJsonAsync<T>(this HttpContent httpContent)
    {
        var stringContent = await httpContent.ReadAsStringAsync();

        return Deserialize<ResponseMessage<T>>(stringContent);
    }

    public static Task<HttpResponseMessage> PatchAsJsonAsync<T>(this HttpClient httpClient, string url, T data)
    {
        var content = new StringContent(Serialize(data), Encoding.UTF8, "application/json");

        return httpClient.PatchAsync(url, content);
    }

    public static Task<HttpResponseMessage> PostAsJsonAsync<T>(this HttpClient httpClient, string url, T data)
    {
        var content = new StringContent(Serialize(data), Encoding.UTF8, "application/json");

        return httpClient.PostAsync(url, content);
    }

    public static Task<HttpResponseMessage> PutAsJsonAsync<T>(this HttpClient httpClient, string url, T data)
    {
        var content = new StringContent(Serialize(data), Encoding.UTF8, "application/json");

        return httpClient.PutAsync(url, content);
    }
}