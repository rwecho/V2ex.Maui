namespace V2ex.Blazor.Services;

public class AppConstants
{
#if DEBUG
    public const bool IsDebug = true;

#else
    public const bool IsDebug = false;
#endif

    public const string BlazorHybridHost = "0.0.0.0";
}
