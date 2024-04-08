namespace V2ex.Blazor.Services;

public interface IFileSaver
{
    Task<string> SaveAs(string filename, byte[] data);
}
