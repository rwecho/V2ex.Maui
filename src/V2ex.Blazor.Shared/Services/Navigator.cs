using Microsoft.AspNetCore.Components;

namespace V2ex.Blazor.Services;

public interface INavigator
{
    void GoBack();

    void RaiseCurrentChanged();
}
public class Navigator : INavigator, IDisposable
{
    private readonly NavigationManager navigationManager;

    private readonly Stack<string> history = new();
    private string currentUri;

    public Navigator(NavigationManager navigationManager)
    {
        this.navigationManager = navigationManager;
        currentUri = this.navigationManager.BaseUri;
    }

    // if the user is backing, we don't want to push the current page to the history again
    private bool isBacking = false;
    public void GoBack()
    {
        if (history.Any())
        {
            isBacking = true;
            navigationManager.NavigateTo(history.Pop());
        }
        else
        {
            navigationManager.NavigateTo("/");
        }
    }

    public void Dispose()
    {
        history.Clear();
    }

    public void RaiseCurrentChanged()
    {
        var currentUri = navigationManager.Uri;

        if (isBacking || GetPathAndQuery(currentUri) == GetPathAndQuery(this.currentUri))
        {
            isBacking = false;
            return;
        }

        history.Push(this.currentUri);

        // if the user is navigating to the base uri, we don't want to keep the history
        if (currentUri == navigationManager.BaseUri)
        {
            history.Clear();
        }

        // keep the current uri
        this.currentUri = currentUri;
    }

    private static bool IsAbsoluteUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out _);
    }

    private static string GetPathAndQuery(string url)
    {
        return IsAbsoluteUrl(url) ? new UriBuilder(url).Uri.PathAndQuery : "/";
    }
}