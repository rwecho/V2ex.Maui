using V2ex.Api;

namespace V2ex.Maui.Services;

public class Utilities
{
    public static string ParseId(string link)
    {
        return new UriBuilder(UrlUtilities.CompleteUrl(link)).Path.Split("/").Last();
    }
}
