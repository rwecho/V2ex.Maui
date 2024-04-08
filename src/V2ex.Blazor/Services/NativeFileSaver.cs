using CommunityToolkit.Maui.Storage;

namespace V2ex.Blazor.Services;

public class NativeFileSaver : Services.IFileSaver
{
    public async Task<string> SaveAs(string filename, byte[] data)
    {
        using var stream = new MemoryStream(data);

        var fileSaverResult = await FileSaver.Default.SaveAsync(filename, stream);
        fileSaverResult.EnsureSuccess();

        return fileSaverResult.FilePath;
    }
}
