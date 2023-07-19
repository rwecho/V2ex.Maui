
using Microsoft.Extensions.Localization;

namespace V2ex.Maui.Services;

public class AppPreferences
{
    public string? LatestTabName { get; set; }
}

public class L : IMarkupExtension<string>
{
    public string? Text { get; set; }

    public string ProvideValue(IServiceProvider serviceProvider)
    {
        if (string.IsNullOrEmpty(Text))
        {
            return string.Empty;
        }
        var localizer = InstanceActivator.Create<IStringLocalizer<MauiResource>>();
        return localizer[Text];
    }

    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
    {
        return (this as IMarkupExtension<string>).ProvideValue(serviceProvider);
    }
}