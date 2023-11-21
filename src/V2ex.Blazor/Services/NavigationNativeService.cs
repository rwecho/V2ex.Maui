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

    public Task Intercept(LocationChangingContext context)
    {
        context.PreventNavigation();

        // create a content page and navigate to it

       
        return Navigation.PushAsync(new ReturnPage(context.TargetLocation), true);
    }
}