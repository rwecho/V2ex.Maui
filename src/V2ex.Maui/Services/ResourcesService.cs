using Volo.Abp.DependencyInjection;

namespace V2ex.Maui.Services;

public class ResourcesService: ITransientDependency
{
    public async Task<string> GetHtmlContainerAsync()
    {
        using var stream = await FileSystem.OpenAppPackageFileAsync("HtmlContainer.html");
        using var reader = new StreamReader(stream);
        return await reader.ReadToEndAsync();
    }
}
