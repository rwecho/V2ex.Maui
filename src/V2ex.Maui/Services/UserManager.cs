using System.Text.Json;
using V2ex.Api;
using Volo.Abp.DependencyInjection;

namespace V2ex.Maui.Services;

public class UserManager : ISingletonDependency
{
    private const string UserKey = "user.json";
    private UserInfo? _currentUser;
    private ReaderWriterLockSlim ReaderWriterLockSlim { get; } = new ReaderWriterLockSlim();
    private bool _isInitialized = true;
    public bool IsAuthorized => this.CurrentUser != null;

    public UserInfo? CurrentUser
    {
        get
        {
            if (_currentUser == null && _isInitialized)
            {
                _currentUser = this.GetCurrentUser();
                _isInitialized = false;
            }
            return _currentUser;
        }
        private set
        {
            _currentUser = value;
        }
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