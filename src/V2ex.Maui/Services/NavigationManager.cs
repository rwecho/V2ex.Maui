using V2ex.Maui.Pages;
using Volo.Abp.DependencyInjection;

namespace V2ex.Maui.Services;

public class NavigationManager : ITransientDependency
{
    public NavigationManager(UserManager userManager, RoutingManager routingManager)
    {
        this.UserManager = userManager;
        this.RoutingManager = routingManager;
    }

    private UserManager UserManager { get; }
    private RoutingManager RoutingManager { get; }

    public async Task GoToAsync(ShellNavigationState navigationState,
        bool animate = true,
        Dictionary<string, object>? parameters = null)
    {
        var route = navigationState.Location.OriginalString;
        if (!UserManager.IsAuthorized && this.RoutingManager.IsAuthorized(route))
        {
            await Shell.Current.GoToAsync(nameof(LoginPage), animate);
            return;
        }
        if (parameters == null)
        {
            await Shell.Current.GoToAsync(navigationState, animate);
        }
        else
        {
            await Shell.Current.GoToAsync(navigationState, animate, parameters);
        }
    }
}