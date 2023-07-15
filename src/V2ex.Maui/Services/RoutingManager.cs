using Volo.Abp.DependencyInjection;

namespace V2ex.Maui.Services;

public class RoutingManager : ISingletonDependency
{
    private readonly Dictionary<string, bool> _routes = new();

    public void Register(string routeKey, Type pageType, bool authorize = false)
    {
        Routing.RegisterRoute(routeKey, pageType);
        _routes.Add(routeKey, authorize);
    }

    public bool IsAuthorized(string routeKey)
    {
        if(_routes.TryGetValue(routeKey, out var value))
        {
            return value;
        }
        return false;
    }
}
