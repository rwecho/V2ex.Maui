using HtmlAgilityPack;

namespace V2ex.Maui.Services;

public class NodeTopicsInfo
{
    public int Total { get; init; }
    public string FavoriteLink { get; init; } = null!;
    public List<ItemInfo> Items { get; init; } = new ();
    internal static NodeTopicsInfo Parse(string html)
    {
        var document = new HtmlDocument();
        document.LoadHtml(html);

        var total = document.DocumentNode.SelectSingleNode("span.topic-count strong");
        var favoriteLink = document.DocumentNode.SelectSingleNode("a[href*=favorite/] ").GetAttributeValue("href", null);
        var items = document.DocumentNode.SelectNodes("div.box div.cell:has(table)")
            .Select(ItemInfo.Parse)
            .ToList();

        return new NodeTopicsInfo
        {
            Total = int.Parse(total.InnerText),
            FavoriteLink = favoriteLink,
            Items = items,
        };
    }

    public class ItemInfo
    {
        public string Avatar{ get; init; } = null!;
        public string Title { get; init; } = null!;
        public string UserName { get; init; } = null!;
        public string ClickedAndContentLength { get; init; } = null!;
        public int CommentNumber { get; init; }
        public string TopicLink { get; init; } = null!;
        internal static ItemInfo Parse(HtmlNode node)
        {
            var avatar = node.SelectSingleNode("img.avatar").GetAttributeValue("src", null);
            var title = node.SelectSingleNode("span.item_title").InnerText;
            var userName = node.SelectSingleNode("span.small.fade strong").InnerText;
            var clickedAndContentLength = node.SelectSingleNode("span.small.fade").InnerText;
            var commentNumber = int.Parse(node.SelectSingleNode("a[class^=count_]").InnerText);
            var topicLink = node.SelectSingleNode("span.item_title a").GetAttributeValue("href", null);

            return new ItemInfo
            {
                Avatar = avatar,
                Title = title,
                UserName = userName,
                ClickedAndContentLength = clickedAndContentLength,
                CommentNumber = commentNumber,
                TopicLink = topicLink,
            };
        }
    }
}
 