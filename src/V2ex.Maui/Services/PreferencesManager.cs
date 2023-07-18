using System.Text.Json;

namespace V2ex.Maui.Services;

public abstract class PreferencesManager<T>
    where T: notnull
{
    private T? _current;
    private bool _isInitialized = true;
    private string Key { get; }
    private ReaderWriterLockSlim ReaderWriterLockSlim { get; } = new ReaderWriterLockSlim();
    public PreferencesManager(string key)
    {
        this.Key = key;
    }
    public T? Current
    {
        get
        {
            if (_current == null && _isInitialized)
            {
                _current = this.GetCurrentValue();
                _isInitialized = false;
            }

            return _current;
        }
        private set
        {
            _current = value;
        }
    }

    private T? GetCurrentValue()
    {

        string? json;
        try
        {
            ReaderWriterLockSlim.EnterReadLock();
            json = Preferences.Get(Key, null);
        }
        finally
        {
            ReaderWriterLockSlim.ExitReadLock();
        }

        if (string.IsNullOrEmpty(json))
        {
            return default;
        }
        return JsonSerializer.Deserialize<T>(json);
    }

    public void Set(T value)
    {
        try
        {
            ReaderWriterLockSlim.EnterWriteLock();
            this.Current = value;
            Preferences.Set(Key, JsonSerializer.Serialize(value));
        }
        finally
        {
            ReaderWriterLockSlim.ExitWriteLock();
        }
    }
}
