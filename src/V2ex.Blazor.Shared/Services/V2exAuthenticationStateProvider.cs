using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace V2ex.Blazor.Services;

public class V2exAuthenticationStateProvider : AuthenticationStateProvider
{

    public V2exAuthenticationStateProvider()
    {

    }

    private bool _isAuthenticated;
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var identity = new ClaimsIdentity();

        //if (_isAuthenticated)
        {
            identity = new ClaimsIdentity(new[]
            {
                // fake asp.net core user identity
                new Claim("sub", "test"),
                new Claim(ClaimTypes.Name, "test"),
                new Claim(ClaimTypes.Role, "Administrator"),

            }, "Fake authentication type" );
        }

        var state = new AuthenticationState(new ClaimsPrincipal(identity));

        var user = state.User;




        return state;
    }


    public async Task LoginAsync(string username, string password)
    {
        _isAuthenticated = true;
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());

        Trace.WriteLine($"LoginAsync: {username} {password}");
    }

    public async Task LogoutAsync()
    {
        _isAuthenticated = false;
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}