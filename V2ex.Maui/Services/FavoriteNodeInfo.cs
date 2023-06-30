using HtmlAgilityPack;

namespace V2ex.Maui.Services;

public class FavoriteNodeInfo
{
    public List<ItemInfo> Items { get; set; } = new();
    internal static FavoriteNodeInfo Parse(string html)
    {
        var document = new HtmlDocument();
        document.LoadHtml(html);
        var wrap = document.DocumentNode.SelectSingleNode("div#my-nodes");

        var items = wrap.SelectNodes("a.fav-node")
            .Select(ItemInfo.Parse)
            .ToList();

        return new FavoriteNodeInfo
        {
            Items = items,
        };
    }

    public class ItemInfo
    {
        public string Image { get; init; } = null!;
        public string Name { get; init; } = null!;
        public string TopicNumber { get; init; } = null!;
        public string Link { get; init; } = null!;
        internal static ItemInfo Parse(HtmlNode node)
        {
            var image = node.SelectSingleNode("img")?.InnerText;
            var name = node.SelectSingleNode("span.fav-node-name")?.GetAttributeValue("ownerText", null);
            var link = node.GetAttributeValue("href", null);
            var topicNumber = node.SelectSingleNode("span.fade.f12")?.InnerText;

            return new ItemInfo
            {
               Image = image,
               Name = name,
               Link = link,
               TopicNumber = topicNumber,
            };
        }
    }
}
 