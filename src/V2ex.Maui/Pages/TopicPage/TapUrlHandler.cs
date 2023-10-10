namespace V2ex.Maui.Pages;

public class TapUrlHandler
{
    public TapUrlHandler(string url)
    {
        const string userPrefix = "file:///member/";
        if (url.StartsWith(userPrefix))
        {
            this.Target = TapTarget.User;
            this.UserName = url.Replace(userPrefix, "");
        }
    }

    public TapTarget Target { get; }
    public string? UserName { get; }
}
