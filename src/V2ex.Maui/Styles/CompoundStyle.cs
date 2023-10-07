namespace V2ex.Maui.Styles;

/// <summary>
/// Add compound style to component, combine styles from different resources
/// </summary>
[ContentProperty(nameof(Keys))]
public class CompoundExtension : IMarkupExtension<Style>
{
    public string Keys { get; set; } = null!;
    
    public Style ProvideValue(IServiceProvider serviceProvider)
    {
        Style? compoundStyle = null;

        if (string.IsNullOrEmpty(Keys))
        {
            ArgumentNullException.ThrowIfNull(nameof(Keys));
        }

        var keys = Keys!.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (string resourceKey in keys)
        {
            var style = new StaticResourceExtension { Key = resourceKey }.ProvideValue(serviceProvider) as Style;
            if (style == null)
            {
                continue;
            }

            if (compoundStyle == null)
            {
                compoundStyle = new Style(style.TargetType);
            }
            compoundStyle.Compound(style);
        }
        return compoundStyle ?? throw new ArgumentNullException(nameof(compoundStyle));
    }

    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
    {
        return (this as IMarkupExtension<Style>).ProvideValue(serviceProvider);
    }
}
