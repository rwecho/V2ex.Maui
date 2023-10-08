using System.Windows.Input;

namespace V2ex.Maui.Components;

public partial class BadgeView : ContentView
{
    public BadgeView()
    {
        InitializeComponent();
    }
    
    public ICommand TapCommand
    {
        get { return (ICommand)GetValue(TapCommandProperty); }
        set { SetValue(TapCommandProperty, value); }
    }

    public static readonly BindableProperty TapCommandProperty =
        BindableProperty.Create("TapCommand", typeof(ICommand), typeof(BadgeView));

    public string Label
    {
        get { return (string)GetValue(LabelProperty); }
        set { SetValue(LabelProperty, value); }
    }

    public static readonly BindableProperty LabelProperty =
        BindableProperty.Create("Label", typeof(string), typeof(BadgeView));

}