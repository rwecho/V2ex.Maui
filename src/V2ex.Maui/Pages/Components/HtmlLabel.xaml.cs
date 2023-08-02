using CommunityToolkit.Mvvm.Input;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using V2ex.Api;
using V2ex.Maui.Pages.ViewModels;
using V2ex.Maui.Services;

namespace V2ex.Maui.Pages.Components;

public partial class HtmlLabel : ContentView
{
    public HtmlLabel()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty TextProperty = BindableProperty.Create(
        nameof(Text),
        typeof(string),
        typeof(HtmlLabel), propertyChanged: TextChanged);

    private static void TextChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not HtmlLabel htmlLabel)
        {
            return;
        }
        var newHtml = (string)newValue;
        if (string.IsNullOrEmpty(newHtml))
        {
            return;
        }
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(newHtml);

        htmlLabel.Content = htmlDoc.RenderNodes();
    }

    public string? Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }
}