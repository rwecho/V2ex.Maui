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
        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException(response.ReasonPhrase);
        }
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }

    public static async Task<T> GetEncapsulatedData<T>(this HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException(response.ReasonPhrase);
        }
        var content = await response.Content.ReadAsStringAsync();
        var document = new HtmlDocument();
        document.LoadHtml(content);

        var result = document.DocumentNode.GetEncapsulatedData<T>();
        return result;
    }

    public static async Task<T> GetEncapsulatedData<T>(this HttpResponseMessage response, Action<HtmlNode> handleNode)
    {
        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException(response.ReasonPhrase);
        }
        var content = await response.Content.ReadAsStringAsync();
        var document = new HtmlDocument();
        document.LoadHtml(content);
        handleNode(document.DocumentNode);
        var result = document.DocumentNode.GetEncapsulatedData<T>();
        return result;
    }

    public static async Task<T> GetEncapsulatedData<T, TError>(this HttpResponseMessage response, Action<TError> handleError)
    {
        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException(response.ReasonPhrase);
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
