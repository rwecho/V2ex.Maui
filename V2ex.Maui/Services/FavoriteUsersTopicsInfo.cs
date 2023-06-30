using HtmlAgilityPack;

namespace V2ex.Maui.Services;

public class FavoriteUsersTopicsInfo
{
    public int  Total { get; set; }
    public List<ItemInfo> Items { get; set; } = new();
    internal static FavoriteUsersTopicsInfo? Parse(string html)
    {
        var document = new HtmlDocument();
        document.LoadHtml(html);

        var wrapper = document.DocumentNode.SelectSingleNode("div#Wrapper");
        var total = wrapper.SelectSingleNode("div.fr.f12 strong.gray").InnerText;
        var items = wrapper.SelectNodes("div.cell.item")
            .Select(ItemInfo.Parse);

        return new FavoriteUsersTopicsInfo
        {
            Total = int.Parse(total),
            Items = items.ToList(),
        };
    }

    public class ItemInfo
    {
        public string Avatar { get; init; } = null!;
        public string UserName { get; init; } = null!;
        public string Time { get; init; } = null!;
        public string Title { get; init; } = null!;
        public string Link { get; init; } = null!;
        public int ComentNumber { get; init; }
        public string TagName { get; init; } = null!;
        public string TagLink { get; init; } = null!;
        internal static ItemInfo Parse(HtmlNode node)
        {
            var avatar = node.SelectSingleNode("img.avatar").InnerText;
            var userName = node.SelectSingleNode("strong a[href^=/member/]").InnerText;
            var time = node.SelectSingleNode("span.small.fade").GetAttributeValue("ownText", null);
            var title = node.SelectSingleNode("span.item_title a[href^=/t/]").InnerText;
            var link = node.SelectSingleNode("span.item_title a[href^=/t/]").GetAttributeValue("href", null);
            var comentNumber = node.SelectSingleNode("a[class^=count_]").InnerText;
            var tagName = node.SelectSingleNode("a.node").InnerText;
            var tagLink = node.SelectSingleNode("a.node").GetAttributeValue("href", null);

            return new ItemInfo
            {
                Avatar = avatar,
                UserName = userName,
                Time = time,
                Title = title,
                Link = link,
                ComentNumber = int.Parse(comentNumber),
                TagName = tagName,
                TagLink = tagLink,
            };
        }
    }
}
 