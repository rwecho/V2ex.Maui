using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using V2ex.Api;

namespace V2ex.Blazor.Services;

public class V2exAuthenticationStateProvider(IPreferences preferences) : AuthenticationStateProvider
{
    private const string UserKey = "user.json";
    private readonly object locker = new();
    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        UserInfo? userInfo;
        lock (locker)
        {
            userInfo = preferences.Get<UserInfo?>(UserKey, null);
        }
        var identity = new ClaimsIdentity();
        if (userInfo != null)
        {
            identity = new ClaimsIdentity(new[]
            {
                new Claim("sub", userInfo.Name),
                new Claim("avatar", userInfo.Avatar),
                new Claim("following", userInfo.Following.ToString()),
                new Claim("nodes", userInfo.Nodes.ToString()),
                new Claim("topics", userInfo.Topics.ToString()),
                new Claim("notifications", userInfo.Notifications?.ToString()??""),
                new Claim("moneyGold", userInfo.MoneyGold?.ToString()??""),
                new Claim("moneySilver", userInfo.MoneySilver?.ToString()??""),
                new Claim("moneyBronze", userInfo.MoneyBronze?.ToString()??""),
            }, "v2ex");
        }
        return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity)));
    }

    public Task LoginAsync(UserInfo userInfo)
    {
        preferences.Set(UserKey, userInfo);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        return Task.CompletedTask;
    }

    public Task LogoutAsync()
    {
        preferences.Set<UserInfo?>(UserKey, null);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        return Task.CompletedTask;
    }
}