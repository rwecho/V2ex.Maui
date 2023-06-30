using HtmlAgilityPack;

namespace V2ex.Maui.Services;

public class NotificationInfo
{
    public int Total { get; set; }
    public List<ReplyInfo> Replies { get; set; } = new();
    internal static NotificationInfo? Parse(string html)
    {
        var document = new HtmlDocument();
        document.LoadHtml(html);
        var notificationNode = document.DocumentNode.SelectSingleNode("div#Main div.box");
        var replies = notificationNode.SelectNodes("div.cell[id^=n_]")
            .Select(ReplyInfo.Parse)
            .ToList();
        var total = notificationNode.SelectSingleNode("div.fr.f12 strong")?.InnerText;

        return new NotificationInfo
        {
            Total = int.Parse(total ?? "0"),
            Replies = replies,
        };
    }

    public class ReplyInfo
    {
        public string Name { get; init; } = null!;
        public string Avatar { get; init; } = null!;
        public string Title { get; init; } = null!;
        public string Link{ get; init; } = null!;
        public string Content { get; init; } = null!;
        public string Time { get; init; } = null!;
        public static ReplyInfo Parse(HtmlNode node)
        {
            var name = node.SelectSingleNode("a[href^=/member/] strong")?.InnerText;
            var avatar = node.SelectSingleNode("a[href^=/member/] img")?.Attributes["src"]?.Value;
            var title = node.SelectSingleNode("span.fade")?.InnerText;
            var link = node.SelectSingleNode("a[href^=/t/]")?.Attributes["href"]?.Value;
            var content = node.SelectSingleNode("div.payload")?.InnerHtml;
            var time = node.SelectSingleNode("span.snow")?.InnerText;
            return new ReplyInfo
            {
                Name = name,
                Avatar = avatar,
                Title = title,
                Link = link,
                Content = content,
                Time = time,
            };
        }
    }
}
 