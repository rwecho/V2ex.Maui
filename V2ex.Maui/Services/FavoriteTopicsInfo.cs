using HtmlAgilityPack;

namespace V2ex.Maui.Services;

public class FavoriteTopicsInfo
{
    public int Total { get; set; }
    public List<ItemInfo> Items { get; set; } = new ();
    internal static FavoriteTopicsInfo? Parse(string html)
    {
        var document = new HtmlDocument();
        document.LoadHtml(html);
        var wrap = document.DocumentNode.SelectSingleNode("div#Wrapper");

        var total = wrap.SelectSingleNode("div.fr.f12 strong.gray")?.InnerText;
        var items = wrap.SelectNodes("div.cell.item")
            .Select(ItemInfo.Parse)
            .ToList();

        return new FavoriteTopicsInfo
        {
            Total = int.Parse(total ?? "0"),
            Items = items,
        };
    }

    public class ItemInfo
    {
        public string UserLink { get; init; } = null!;
        public string Avatar { get; init; } = null!;
        public string Title { get; init; } = null!;
        public string Link { get; init; } = null!;
        public int CommentNumber { get; init; }
        public string Tag { get; init; } = null!;
        public string TagLink { get; init; } = null!;
        public string Time { get; init; } = null!;
        internal static ItemInfo Parse(HtmlNode node)
        {
            var userLink = node.SelectSingleNode("td>a[href^=/member]")?.InnerText;
            var avatar = node.SelectSingleNode("img.avatar")?.Attributes["src"]?.Value;
            var title = node.SelectSingleNode("span.item_title")?.InnerText;
            var link = node.SelectSingleNode("span.item_title a")?.Attributes["href"]?.Value;
            var commentNumber = node.SelectSingleNode("a[class^=count_]")?.InnerText;
            var tag = node.SelectSingleNode("a.node")?.InnerText;
            var tagLink = node.SelectSingleNode("a.node")?.Attributes["href"]?.Value;
            var time = node.SelectSingleNode("span.small.fade")?.GetAttributeValue("ownText", null);

            return new ItemInfo
            {
                UserLink = userLink ?? string.Empty,
                Avatar = avatar ?? string.Empty,
                Title = title ?? string.Empty,
                Link = link ?? string.Empty,
                CommentNumber = int.Parse(commentNumber ?? "0"),
                Tag = tag ?? string.Empty,
                TagLink = tagLink ?? string.Empty,
                Time = time ?? string.Empty,
            };
        }
    }
}
 