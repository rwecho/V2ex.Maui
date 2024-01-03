using System;
using System.Linq;

namespace V2ex.Api;

public static class UrlUtilities
{
    public const string BASE_URL = "https://www.v2ex.com";

    public static string CompleteUrl(string url)
    {
        if(url.StartsWith("https://") || url.StartsWith("http://"))
        {
            return url;
        }

        if (url.StartsWith(BASE_URL))
        {
            return url;
        }

        return BASE_URL + url;
    }

    public static string ParseId(string url)
    {
        return new UriBuilder(CompleteUrl(url)).Path.Split("/").Last();
    }
}