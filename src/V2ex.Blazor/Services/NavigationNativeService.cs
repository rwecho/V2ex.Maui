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
            return App.Current!.MainPage!.Navigation;
        }
    }

    public Task Intercept(string sourceLocation, LocationChangingContext context)
    {
        if (sourceLocation == context.TargetLocation)
        {
            return Task.CompletedTask;
        }

        var path = context.IsNavigationIntercepted
            ? new Uri(context.TargetLocation).LocalPath
            : new Uri("http://0.0.0.0" + context.TargetLocation).LocalPath;
        if (path == "/" || path == "/tab")
        {
            return Navigation.PopToRootAsync(true);
        }

        context.PreventNavigation();
        return Navigation.PushAsync(new ReturnPage(context.TargetLocation), true);
    }
}
