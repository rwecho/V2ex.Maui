using Microsoft.AspNetCore.Components.Routing;

namespace V2ex.Blazor.Services;

public class AppConstants
{
#if DEBUG
    public const bool IsDebug = true;

#else
    public const bool IsDebug = false;
#endif
}

public interface INavigationInterceptorService
{
    Task Intercept(LocationChangingContext context);
}
