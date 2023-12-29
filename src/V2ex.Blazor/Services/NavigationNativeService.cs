using Microsoft.AspNetCore.Components.Routing;
using V2ex.Blazor.Pages;

namespace V2ex.Blazor.Services;

public class NavigationInterceptorService : INavigationInterceptorService
{
    public NavigationInterceptorService()
    {
    }

    private INavigation Navigation
    {
        get
        {
            return Application.Current!.MainPage!.Navigation;
        }
    }

    public Task Intercept(string sourceLocation, LocationChangingContext context)
    {
        var sourcePath = GetLocalPath(sourceLocation);
        var targetPath = GetLocalPath(context.TargetLocation);
        if (sourcePath == targetPath)
        {
            return Task.CompletedTask;
        }

        if (targetPath == "/" || targetPath == "/tab")
        {
            return Navigation.PopToRootAsync(true);
        }

        context.PreventNavigation();
        return Navigation.PushAsync(new ReturnPage(context.TargetLocation), false);
    }

    private static string GetLocalPath(string url)
    {
        Uri uri;
        if (url.StartsWith("http://") || url.StartsWith("https://"))
        {
            uri = new Uri(url);
        }
        else
        {
            uri = new Uri("https://0.0.0.0"+ url);
        }
        return uri.LocalPath;
    }
}
