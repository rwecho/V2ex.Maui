namespace V2ex.Api;

public static class UrlUtils
{
    public const string BASE_URL = "https://www.v2ex.com";

    public static string CompleteUrl(string url)
    {
        if (url.StartsWith(BASE_URL))
        {
            return url;
        }

        return BASE_URL + url;
    }
}