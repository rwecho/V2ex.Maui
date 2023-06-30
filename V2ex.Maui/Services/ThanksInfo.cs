using HtmlAgilityPack;

namespace V2ex.Maui.Services;

public class ThanksInfo
{
    public string Link { get; init; } = null!;
    internal static ThanksInfo Parse(string html)
    {
        var document = new HtmlDocument();
        document.LoadHtml(html);

        var link = document.DocumentNode.SelectSingleNode("a[href=/balance]").InnerText;

        return new ThanksInfo
        {
            Link = link,
        };
    }
}
