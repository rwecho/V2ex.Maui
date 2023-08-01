using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace V2ex.Api;

public static class HttpResponseMessageExtensions
{
    public static async Task<T?> ReadFromJson<T>(this HttpResponseMessage response)
    {
        CheckStatusCode(response);
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }

    public static async Task<T> GetEncapsulatedData<T>(this HttpResponseMessage response,
        ILogger? logger = null)
    {
        logger ??= NullLogger.Instance;
        CheckStatusCode(response);

        var datetime1 = DateTime.Now;
        logger.LogDebug("Reading content of request {Url} as string", response.RequestMessage.RequestUri);

        var content = await response.Content.ReadAsStringAsync();
        logger.LogDebug("Reading content as string completed, elapsed {Elapsed}", DateTime.Now - datetime1);
        var document = new HtmlDocument();
        document.LoadHtml(content);

        var datetime2 = DateTime.Now;
        var result = document.DocumentNode.GetEncapsulatedData<T>();
        logger.LogDebug("GetEncapsulatedData completed, elapsed {Elapsed}", DateTime.Now - datetime2);
        return result;
    }

    private static void CheckStatusCode(HttpResponseMessage response)
    {
        var statusCode = (int)response.StatusCode;
        if (statusCode > 300 && statusCode < 400)
        {
            throw new NotAuthorizedException();
        }

        if (statusCode > 400 && statusCode < 500)
        {
            throw new BadRequestException(response.ReasonPhrase);
        }

        if (statusCode > 500)
        {
            throw new ServerErrorException(response.ReasonPhrase);
        }
    }

    public static async Task<T> GetEncapsulatedData<T>(this HttpResponseMessage response,
        Action<HtmlNode> handleNode,
        ILogger? logger = null)
    {
        logger ??= NullLogger.Instance;
        CheckStatusCode(response);

        var datetime1 = DateTime.Now;
        logger.LogDebug("Reading content of request {Url} as string", response.RequestMessage.RequestUri);
        var content = await response.Content.ReadAsStringAsync();
        logger.LogDebug("Reading content as string completed, elapsed {Elapsed}", DateTime.Now - datetime1);
        var document = new HtmlDocument();
        document.LoadHtml(content);
        handleNode(document.DocumentNode);
        var datetime2 = DateTime.Now;
        var result = document.DocumentNode.GetEncapsulatedData<T>();
        logger.LogDebug("GetEncapsulatedData completed, elapsed {Elapsed}", DateTime.Now - datetime2);
        return result;
    }

    public static async Task<T> GetEncapsulatedData<T, TError>(this HttpResponseMessage response, 
        Action<TError> handleError,
        ILogger? logger = null)
    {
        CheckStatusCode(response);
        var datetime1 = DateTime.Now;
        logger.LogDebug("Reading content of request {Url} as string", response.RequestMessage.RequestUri);
        var content = await response.Content.ReadAsStringAsync();
        logger?.LogDebug("Reading content as string completed, elapsed {Elapsed}", DateTime.Now - datetime1);
        var document = new HtmlDocument();
        document.LoadHtml(content);

        var datetime2 = DateTime.Now;
        var error = document.DocumentNode.GetEncapsulatedData<TError>();
        handleError(error);
        var result = document.DocumentNode.GetEncapsulatedData<T>();
        logger?.LogDebug("GetEncapsulatedData completed, elapsed {Elapsed}", DateTime.Now - datetime2);
        return result;
    }
}
