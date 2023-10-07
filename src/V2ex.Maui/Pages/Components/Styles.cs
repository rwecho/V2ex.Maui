namespace V2ex.Maui.Pages.Components;

public enum Size
{
    ExtraSmall,
    Small,
    Medium,
    Large,
    ExtraLarge
}

public static class Styles
{
    public static Size GetSize(BindableObject view)
    {
        return (Size)view.GetValue(SizeProperty);
    }

    public static void SetSize(BindableObject view, Size value)
    {
        view.SetValue(SizeProperty, value);
    }

    public static readonly BindableProperty SizeProperty =
        BindableProperty.CreateAttached("Size", typeof(Size), typeof(Styles), Size.Medium, propertyChanging: SizeChanging, propertyChanged: SizeChanged);

    private static void SizeChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is Button button)
        {
            button.GetValue(SizeProperty);
            SetButtonSize(button, (Size)newValue);
        }
    }

    private static void SetButtonSize(Button button, Size size)
    {
        button.FontSize = size switch
        {
            Size.ExtraSmall => 10,
            Size.Small => 12,
            Size.Medium => 14,
            Size.Large => 16,
            Size.ExtraLarge => 18,
            _ => 14
        };

        button.Padding = size == Size.ExtraSmall ? new Thickness(8) : new Thickness(12);
    }

    private static void SizeChanging(BindableObject bindable, object oldValue, object newValue)
    {
    }
}