using HtmlAgilityPack;

namespace V2ex.Maui.Services;

public class MemberPageInfo
{
    public string UserName { get; init; } = null!;
    public string Avatar { get; init; } = null!;
    public string Description { get; init; } = null!;
    public string Online { get; init; } = null!;
    public string FollowOnClick { get; init; } = null!;
    public string BlockOnClick { get; init; } = null!;
    public List<TopicItemInfo> TopicItems { get; init; } = null!;
    public List<ReplyDockerItemInfo> DockItems { get; init; } = null!;
    public List<ReplyContentItemInfo> ReplyContentItems { get; init; } = null!;
    public static MemberPageInfo Parse(string html)
    {
        var document = new HtmlDocument();
        document.LoadHtml(html);
        var wrap = document.DocumentNode.SelectSingleNode("div#Wrapper");
        var username = wrap.SelectSingleNode("h1");
        var avatar = wrap.SelectSingleNode("img.avatar");
        var description = wrap.SelectSingleNode("td[valign=top] > span.gray");
        var online = wrap.SelectSingleNode("strong.online");
        var follow = wrap.SelectSingleNode("div.fr input").GetAttributeValue("onclick", null);
        var block = wrap.SelectSingleNode("div.fr input[value*=lock]").GetAttributeValue("onclick", null);
        var topicItems = wrap.SelectNodes("div.box:has(div.cell_tabs) > div.cell.item")
            .Select(TopicItemInfo.Parse)
            .ToList();
        var dockItems = wrap.SelectNodes("div.box:last-child > div.dock_area")
            .Select(ReplyDockerItemInfo.Parse)
            .ToList();
        var replyContentItems = wrap.SelectNodes("div.box:last-child div.reply_content")
            .Select(ReplyContentItemInfo.Parse)
            .ToList();

        return new MemberPageInfo
        {
            UserName = username.InnerText,
            Avatar = avatar.GetAttributeValue("src", null),
            Description = description.InnerText,
            Online = online.InnerText,
            FollowOnClick = follow,
            BlockOnClick = block,
            TopicItems = topicItems,
            DockItems = dockItems,
            ReplyContentItems = replyContentItems,
        };
    }

    public class TopicItemInfo
    {
        public string UserName { get; init; } = null!;
        public string Tag { get; init; } = null!;
        public string TagLink { get; init; } = null!;
        public string title { get; init; } = null!;
        public string Link { get; init; } = null!;
        public string Time { get; init; } = null!;
        public int ReplyNumber { get; init; }
        internal static TopicItemInfo Parse(HtmlNode node)
        {
            var username = node.SelectSingleNode("strong > a[href^=/member/]:first-child");
            var tag = node.SelectSingleNode("a.node");
            var tagLink = tag.GetAttributeValue("href", null);
            var title = node.SelectSingleNode("span.item_title");
            var link = node.SelectSingleNode("span.item_title a").GetAttributeValue("href", null);
            var time = node.SelectSingleNode("span.small.fade:last-child");
            var replyNumber = node.SelectSingleNode("a[class^=count_]").InnerText;
            return new TopicItemInfo
            {
                UserName = username.InnerText,
                Tag = tag.InnerText,
                TagLink = tagLink,
                title = title.InnerText,
                Link = link,
                Time = time.InnerText,
                ReplyNumber = int.Parse(replyNumber),
            };
        }
    }
    public class ReplyDockerItemInfo
    {
        public string Title { get; init; } = null!;
        public string Link { get; init; } = null!;
        public string Time { get; init; } = null!;

        internal static ReplyDockerItemInfo Parse(HtmlNode node)
        {
            var title = node.SelectSingleNode("span.gray");
            var link = node.SelectSingleNode("span.gray > a").GetAttributeValue("href", null);
            var time = node.SelectSingleNode("span.fade");
            return new ReplyDockerItemInfo
            {
                Title = title.InnerText,
                Link = link,
                Time = time.InnerText,
            };
        }
    }
    public class ReplyContentItemInfo
    {
        public string Content { get; init; } = null!;
        internal static ReplyContentItemInfo Parse(HtmlNode node)
        {
            return new ReplyContentItemInfo
            {
                Content = node.InnerHtml,
            };
        }
    }
}
 