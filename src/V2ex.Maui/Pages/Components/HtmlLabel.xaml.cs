using HtmlAgilityPack;

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
        if(bindable is not HtmlLabel htmlLabel)
        {
            return;
        }

        htmlLabel.Container.Children.Clear();

        var newHtml = (string)newValue;
        if (string.IsNullOrEmpty(newHtml))
        {
            return;
        }
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(newHtml);

        foreach (var childNode in htmlDoc.DocumentNode.ChildNodes)
        {
            htmlLabel.Container.Children.Add(RenderDocumentNode(childNode));
        }
    }

    private static IView RenderNode(HtmlNode documentNode)
    {
        var container = new StackLayout
        {
            Orientation = StackOrientation.Vertical
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

    private static Label RenderInnerText(HtmlNode documentNode)
    {
        return new Label
        {
            Text = documentNode.InnerText
        };
    }

    private static Label RenderParagraph(HtmlNode documentNode)
    {
        return new Label
        {
            Text = documentNode.InnerText
        };
    }

    private static Label RenderNewLine()
    {
        return new Label
        {
            Text = Environment.NewLine
        };
    }

    private static Label RenderLink(HtmlNode documentNode)
    {
        return new Label
        {
            Text = documentNode.InnerText,
            GestureRecognizers =
                    {
                        new TapGestureRecognizer
                        {
                            Command = new Command(() =>
                            {
                                var href = documentNode.GetAttributeValue("href", string.Empty);
                                if (string.IsNullOrEmpty(href))
                                {
                                    return;
                                }
                                Launcher.OpenAsync(href);
                            })
                        }
                    }
        };
    }

    private static Image RenderImage(HtmlNode documentNode)
    {
        return new Image
        {
            Source = documentNode.GetAttributeValue("src", string.Empty)
        };
    }

    private static Label RenderText(HtmlNode documentNode)
    {
        return new Label
        {
            Text = documentNode.InnerText
        };
    }

    public string? Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }
}