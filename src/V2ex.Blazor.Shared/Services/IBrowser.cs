namespace V2ex.Blazor.Services;

public  interface IBrowser
{
    Task OpenAsync(Uri uri);
}