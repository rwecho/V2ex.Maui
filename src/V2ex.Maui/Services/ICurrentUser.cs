namespace V2ex.Maui.Services;

public interface  ICurrentUser
{
    string? Name { get; }

    bool IsAuthorized();
}
