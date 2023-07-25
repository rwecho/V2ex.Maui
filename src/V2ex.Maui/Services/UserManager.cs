using System.Text.Json;
using V2ex.Api;
using Volo.Abp.DependencyInjection;

namespace V2ex.Maui.Services;

public class UserManager : ISingletonDependency
{
    private const string UserKey = "user.json";
    private UserInfo? _currentUser;
    private ReaderWriterLockSlim ReaderWriterLockSlim { get; } = new ReaderWriterLockSlim();
    public bool IsAuthorized => this.CurrentUser != null;

    public event EventHandler<UserInfo?>? UserChanged;

    public UserInfo? CurrentUser
    {
        get
        {
            return _currentUser;
        }
        private set
        {
            _currentUser = value;

            this.UserChanged?.Invoke(this, _currentUser);
        }
    }

    public UserManager()
    {
        this.CurrentUser = this.GetCurrentUser();
    }

    public void Login(UserInfo user)
    {
        try
        {
            ReaderWriterLockSlim.EnterWriteLock();
            this.CurrentUser = user;
            Preferences.Set(UserKey, JsonSerializer.Serialize(user));
        }
        finally
        {
            ReaderWriterLockSlim.ExitWriteLock();
        }
    }

    public void Logout()
    {
        try
        {
            ReaderWriterLockSlim.EnterWriteLock();
            this.CurrentUser = null;
            Preferences.Remove(UserKey);
        }
        finally
        {
            ReaderWriterLockSlim.ExitWriteLock();
        }
    }

    private UserInfo? GetCurrentUser()
    {
        string? userJson;
        try
        {
            ReaderWriterLockSlim.EnterReadLock();
            userJson = Preferences.Get(UserKey, null);
        }
        finally
        {
            ReaderWriterLockSlim.ExitReadLock();
        }

        if (string.IsNullOrEmpty(userJson))
        {
            return null;
        }
        return JsonSerializer.Deserialize<UserInfo>(userJson);
    }
}