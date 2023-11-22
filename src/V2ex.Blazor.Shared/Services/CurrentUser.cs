using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace V2ex.Blazor.Services;

public class CurrentUser(AuthenticationStateProvider stateProvider)
{
    private Task<AuthenticationState> GetAuthenticationStateTask { get; } = Task.Run(stateProvider.GetAuthenticationStateAsync);

    public string UserName => this.FindClaimValue("sub");
    public string Avatar => this.FindClaimValue("avatar");
    public int Following => int.TryParse(this.FindClaimValue("following"), out var v) ? v : 0;
    public int Nodes => int.TryParse(this.FindClaimValue("nodes"), out var v) ? v : 0;
    public int Topics => int.TryParse(this.FindClaimValue("topics"), out var v) ? v : 0;
    public int Notifications => int.TryParse(this.FindClaimValue("notifications"), out var v) ? v : 0;
    public int MoneyGold => int.TryParse(this.FindClaimValue("moneyGold"), out var v) ? v : 0;
    public int MoneySilver => int.TryParse(this.FindClaimValue("moneySilver"), out var v) ? v : 0;
    public int MoneyBronze => int.TryParse(this.FindClaimValue("moneyBronze"), out var v) ? v : 0;
    public bool IsAuthenticated => !string.IsNullOrEmpty(UserName);

    private AuthenticationState AuthenticationState
    {
        get
        {
            return this.GetAuthenticationStateTask.Result;
        }
    }

    public Claim? FindClaim(string claimType)
    {
        return AuthenticationState.User.Claims.FirstOrDefault(c => c.Type == claimType);
    }

    public string FindClaimValue(string claimType)
    {
        return this.FindClaim(claimType)?.Value ?? string.Empty;
    }
}
