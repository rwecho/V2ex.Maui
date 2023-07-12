using HtmlAgilityPack;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace V2ex.Api;

public static class HttpResponseMessageExtensions
{
    public static async Task<T?> ReadFromJson<T>(this HttpResponseMessage response)
    {
        if ((int)response.StatusCode >= 400 && (int)response.StatusCode <= 500)
        {
            throw new InvalidOperationException("The page is not found.");
        }
        // handle status code greater than 500
        if ((int)response.StatusCode >= 500)
        {
            throw new InvalidOperationException("The server is not available.");
        }
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }

    public static async Task<T> GetEncapsulatedData<T>(this HttpResponseMessage response)
    {
        if ((int)response.StatusCode >= 400 && (int)response.StatusCode <= 500)
        {
            throw new InvalidOperationException("The page is not found.");
        }
        // handle status code greater than 500
        if ((int)response.StatusCode >= 500)
        {
            throw new InvalidOperationException("The server is not available.");
        }
        var content = await response.Content.ReadAsStringAsync();
        var document = new HtmlDocument();
        document.LoadHtml(content);
        var result = document.DocumentNode.GetEncapsulatedData<T>();
        return result;
    }

    public static async Task<T> GetEncapsulatedData<T, TError>(this HttpResponseMessage response, Action<TError> handleError)
    {
        if ((int)response.StatusCode >= 400 && (int)response.StatusCode <= 500)
        {
            throw new InvalidOperationException("The page is not found.");
        }
        // handle status code greater than 500
        if ((int)response.StatusCode >= 500)
        {
            throw new InvalidOperationException("The server is not available.");
        }
        var content = await response.Content.ReadAsStringAsync();
        var document = new HtmlDocument();
        document.LoadHtml(content);

        var error = document.DocumentNode.GetEncapsulatedData<TError>();
        handleError(error);
        var result = document.DocumentNode.GetEncapsulatedData<T>();
        return result;
    }

}
