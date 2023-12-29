using V2ex.Api;

namespace V2ex.Blazor.Services;

public interface IAuthenticationStateProvider
{
    Task LoginAsync(UserInfo userInfo);

    Task LogoutAsync();
}