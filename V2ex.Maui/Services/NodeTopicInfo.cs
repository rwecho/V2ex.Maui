using HtmlAgilityPack;

namespace V2ex.Maui.Services;

public class NodeTopicInfo
{
    public int Total { get; init; }
    public string FavoriteLink { get; init; } = null!;
    public List<ItemInfo> Items { get; init; } = null!;
    internal static NodeTopicInfo Parse(string html)
    {
        var document = new HtmlDocument();
        document.LoadHtml(html);
        var wrapper = document.DocumentNode.SelectSingleNode("div#Wrapper");
        var total = wrapper.SelectSingleNode("span.topic-count strong").InnerText;
        var favoriteLink = wrapper.SelectSingleNode("a[href*=favorite/] ").GetAttributeValue("href", "");
        var items = wrapper.SelectNodes("div.box div.cell:has(table)")
            .Select(ItemInfo.Parse)
            .ToList();

        return new NodeTopicInfo
        {
            Total = int.Parse(total),
            FavoriteLink = favoriteLink,
            Items = items,
        };
    }

    public class ItemInfo
    {
        public string Avatar { get; init; } = null!;
        public string Title { get; init; } = null!;
        public string UserName { get; init; } = null!;
        public string ClickedAndContentLength { get; init; } = null!;
        public int CommentNumber { get; init; }
        public string TopicLink { get; init; } = null!;
        internal static ItemInfo Parse(HtmlNode node)
        {
            var avatar = node.SelectSingleNode("img.avatar").GetAttributeValue("src", "");
            var title = node.SelectSingleNode("span.item_title").InnerText;
            var userName = node.SelectSingleNode("span.small.fade strong").InnerText;
            var clickedAndContentLength = node.SelectSingleNode("span.small.fade").GetAttributeValue("ownText", null);
            var commentNumber = node.SelectSingleNode("a[class^=count_]").InnerText;
            var topicLink = node.SelectSingleNode("span.item_title a").GetAttributeValue("href", "");
            return new ItemInfo
            {
                Avatar = avatar,
                Title = title,
                UserName = userName,
                ClickedAndContentLength = clickedAndContentLength,
                CommentNumber = int.Parse(commentNumber),
                TopicLink = topicLink,
            };
        }
    }
}

