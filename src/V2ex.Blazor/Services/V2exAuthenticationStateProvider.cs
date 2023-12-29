using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using V2ex.Api;

namespace V2ex.Blazor.Services;

public class V2exAuthenticationStateProvider(CookieContainerService cookieContainerService,
    ILogger<V2exAuthenticationStateProvider> logger) : AuthenticationStateProvider, IAuthenticationStateProvider
{
    private CookieContainerService CookieContainerService { get; } = cookieContainerService;
    private ILogger<V2exAuthenticationStateProvider> Logger { get; } = logger;
    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var userInfo = CookieContainerService.User;
        var identity = new ClaimsIdentity();
        if (userInfo != null)
        {
            try
            {
                var notifications = userInfo.Notifications?.ToString();
                var unreadMessageCount = 0;
                if (!string.IsNullOrEmpty(notifications))
                {
                    var splits = notifications.Split(" ");
                    if (splits.Length > 0 && int.TryParse(splits[1], out var count))
                    {
                        unreadMessageCount = count;
                    }
                }

                identity = new ClaimsIdentity(new[]
                {
                    new Claim("sub", userInfo.Name),
                    new Claim("avatar", userInfo.Avatar),
                    new Claim("following", userInfo.Following.ToString()),
                    new Claim("nodes", userInfo.Nodes.ToString()),
                    new Claim("topics", userInfo.Topics.ToString()),
                    new Claim("notifications", unreadMessageCount.ToString()),
                    new Claim("moneyGold", userInfo.MoneyGold?.Trim().ToString()??"0"),
                    new Claim("moneySilver", userInfo.MoneySilver?.Trim().ToString()??"0"),
                    new Claim("moneyBronze", userInfo.MoneyBronze?.Trim().ToString()??"0"),
                }, "v2ex");
            }
            catch (Exception)
            {
                Logger.LogError("Failed to create ClaimsIdentity");
            }
        }
        return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity)));
    }

    public Task LoginAsync(UserInfo userInfo)
    {
        this.CookieContainerService.Login(userInfo);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        return Task.CompletedTask;
    }
    public Task LogoutAsync()
    {
        this.CookieContainerService.Logout();
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        return Task.CompletedTask;
    }
}
