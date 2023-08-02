using CommunityToolkit.Mvvm.Input;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using V2ex.Api;
using V2ex.Maui.Pages.ViewModels;
using V2ex.Maui.Services;

namespace V2ex.Maui.Pages.Components;

public static class HtmlDocumentExtensions
{
    public static View RenderNodes(this HtmlDocument htmlDocument)
    {
        return RenderChildrenNodes(htmlDocument.DocumentNode);
    }

    private static View RenderChildrenNodes(HtmlNode htmlNode)
    {
        //todo: not find a layout to render a multiple labels with wrap
        var container = new StackLayout();

        foreach (var childNode in htmlNode.ChildNodes)
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

    private static View? RenderDocumentNode(HtmlNode documentNode)
    {
        return documentNode.NodeType switch
        {
            HtmlNodeType.Document =>
                RenderChildrenNodes(documentNode),
            HtmlNodeType.Text =>
                RenderText(documentNode),
            HtmlNodeType.Element when documentNode.Name == "img" =>
                RenderImage(documentNode),
            HtmlNodeType.Element when documentNode.Name == "a" =>
                RenderLink(documentNode),
            HtmlNodeType.Element when documentNode.Name == "br" =>
                null,
            HtmlNodeType.Element when documentNode.Name == "p" =>
                RenderChildrenNodes(documentNode),
            HtmlNodeType.Element when documentNode.Name == "div" =>
                RenderChildrenNodes(documentNode),
            _ => RenderInnerText(documentNode)
        };
    }


    private static View? RenderInnerText(HtmlNode htmlNode)
    {
        var text = htmlNode.InnerText.Trim();

        if (text == "@" || string.IsNullOrEmpty(text))
        {
            return null;
        }

        return new Label
        {
            Text = htmlNode.InnerText,
            VerticalOptions = LayoutOptions.Center,
        };
    }

    private static View? RenderLink(HtmlNode htmlNode)
    {
        var view = RenderChildrenNodes(htmlNode);
        if (view == null)
        {
            return null;
        }

        var tapCommand = new RelayCommand(async () =>
        {
            var href = htmlNode.GetAttributeValue("href", string.Empty);
            await LaunchHref(href);
        });

        view.GestureRecognizers.Add(new TapGestureRecognizer
        {
            Command = tapCommand
        });
        return view;
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

    private static Image RenderImage(HtmlNode htmlNode)
    {
        return new Image
        {
            Source = htmlNode.GetAttributeValue("src", string.Empty),
            Aspect = Aspect.AspectFit,
        };
    }

    private static View? RenderText(HtmlNode htmlNode)
    {
        return RenderInnerText(htmlNode);
    }
}
