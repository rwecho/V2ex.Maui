using Volo.Abp.DependencyInjection;

namespace V2ex.Maui.Services;

public class ResourcesService: ITransientDependency
{
    public async Task<string> GetMarkdownContainer()
    {
        using var stream = await FileSystem.OpenAppPackageFileAsync("MarkdownContainer.html");
        using var reader = new StreamReader(stream);
        return await reader.ReadToEndAsync();
    }
}
