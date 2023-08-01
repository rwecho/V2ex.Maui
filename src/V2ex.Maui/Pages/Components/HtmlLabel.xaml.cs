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

        htmlLabel.Content = RenderNode(htmlDoc.DocumentNode);
    }

    private static View? RenderNode(HtmlNode documentNode)
    {
        //todo: not find a layout to render a multiple labels with wrap
        // so use a flex layout to render a multiple labels and every label is a paragraph
        var container = new FlexLayout
        {
            Direction = Microsoft.Maui.Layouts.FlexDirection.Column
        };

        foreach (var childNode in documentNode.ChildNodes)
        {
            var view = RenderDocumentNode(childNode);
            if (view is null)
            {
                continue;
            }
            container.Children.Add(view);
        }

        return container;
    }

    private static IView? RenderDocumentNode(HtmlNode documentNode)
    {
        return documentNode.NodeType switch
        {
            HtmlNodeType.Document =>
                RenderNode(documentNode),
            HtmlNodeType.Text =>
                RenderText(documentNode),
            HtmlNodeType.Element when documentNode.Name == "img" =>
                RenderImage(documentNode),
            HtmlNodeType.Element when documentNode.Name == "a" =>
                RenderLink(documentNode),
            HtmlNodeType.Element when documentNode.Name == "br" =>
                null,
            HtmlNodeType.Element when documentNode.Name == "p" =>
                RenderParagraph(documentNode),
            _ => RenderInnerText(documentNode)
        };
    }

    private static View? RenderInnerText(HtmlNode documentNode)
    {
        var text = documentNode.InnerText.Trim();

        if(text == "@")
        {
            return null;
        }

        return new Label
        {
            Text = documentNode.InnerText,
        };
    }

    private static View? RenderParagraph(HtmlNode documentNode)
    {
        return RenderInnerText(documentNode);
    }


    private static View? RenderLink(HtmlNode documentNode)
    {
        return new Label
        {
            Text = $"@{documentNode.InnerText}",
            TextDecorations = TextDecorations.Underline,
            GestureRecognizers =
                    {
                        new TapGestureRecognizer
                        {
                            Command = new RelayCommand(async () =>
                            {
                                var href = documentNode.GetAttributeValue("href", string.Empty);
                                if (string.IsNullOrEmpty(href))
                                {
                                    return;
                                }
                                await LaunchHref(href);
                            })
                        }
                    }
        };
    }

    private static async Task LaunchHref(string href)
    {
        var navigationManager = InstanceActivator.Create<NavigationManager>();
        var memberRouter = new Regex("/member/(.+)");
        if (memberRouter.IsMatch(href))
        {
            var match = memberRouter.Match(href);
            var username = match.Groups[1].Value;

            await navigationManager.GoToAsync(nameof(MemberPage), true, new Dictionary<string, object>
            {
                { MemberPageViewModel.QueryUserNameKey, username }
            });
            return;
        }

        if (href.StartsWith("http"))
        {
            await Launcher.OpenAsync(href);
        }
        else
        {
            await Launcher.OpenAsync(UrlUtilities.CompleteUrl(href));
        }
    }

    private static Image RenderImage(HtmlNode documentNode)
    {
        return new Image
        {
            Source = documentNode.GetAttributeValue("src", string.Empty)
        };
    }

    private static View? RenderText(HtmlNode documentNode)
    {
        return RenderInnerText(documentNode);
    }

    public string? Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }
}