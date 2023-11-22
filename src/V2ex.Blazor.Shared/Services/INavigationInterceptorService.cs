using Microsoft.AspNetCore.Components.Routing;

namespace V2ex.Blazor.Services;

public interface INavigationInterceptorService
{
    Task Intercept(LocationChangingContext context);
}
