using HtmlAgilityPack;

namespace V2ex.Maui.Services;

public class NewsInfo
{
    public string? Unread { get; init; }

    public List<NewsInfo.Item>? Items { get; init; }

    public string? TwoStepString { get; init; }

    public static NewsInfo Parse(string html)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        var unread = doc.DocumentNode
           .SelectSingleNode("input.super.special.button")
           ?.GetAttributeValue("value", null);
        var twoStepString = doc.DocumentNode.SelectSingleNode("form[action=/2fa]")?.InnerText;

        var items = doc.DocumentNode.SelectNodes("div.cell.item")
            .Select(Item.Parse)
            .ToList();

        return new NewsInfo
        {
            Unread = unread,
            TwoStepString = twoStepString,
            Items = items,
        };
    }

    public class Item
    {
        public string Title { get; init; } = null!;
        public string LinkPath { get; init; } = null!;
        public string Avatar { get; init; } = null!;
        public string AvatarLink { get; init; } = null!;
        public string UserName { get; init; } = null!;
        public string Time { get; init; } = null!;
        public string TagName { get; init; } = null!;
        public string TagLink { get; init; } = null!;
        public int Replies { get; init; }
        public string Id { get; init; } = null!;

        public static Item Parse(HtmlNode node)
        {
            var title = node.SelectSingleNode("span.item_title > a").InnerText;
            var linkPath = node.SelectSingleNode("span.item_title > a").GetAttributeValue("href", null);
            var avatar = node.SelectSingleNode("td > a > img").GetAttributeValue("src", null);
            var avatarLink = node.SelectSingleNode("td > a").GetAttributeValue("href", null);
            var userName = node.SelectSingleNode("span.small.fade > strong > a").InnerText;
            var time = node.SelectSingleNode("span.small.fade:last-child").GetAttributeValue("ownText", null);
            var tagName = node.SelectSingleNode("span.small.fade > a").InnerText;
            var tagLink = node.SelectSingleNode("span.small.fade > a").GetAttributeValue("href", null);
            var replies = node.SelectSingleNode("a[class^=count_]").InnerText;

            var id = new UriBuilder(linkPath).Path.Split("/").Last();

            return new Item
            {
                Title = title,
                LinkPath = linkPath,
                Avatar = avatar,
                AvatarLink = avatarLink,
                UserName = userName,
                Time = time,
                TagName = tagName,
                TagLink = tagLink,
                Replies = int.Parse(replies),
                Id = id,
            };
        }
    }
}
