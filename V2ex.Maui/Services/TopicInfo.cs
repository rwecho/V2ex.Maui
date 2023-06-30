using HtmlAgilityPack;

namespace V2ex.Maui.Services;

public class TopicInfo
{
    public HeaderInfo Header { get; init; } = null!;
    public ContentInfo Content { get; init; } = null!;
    public ProblemInfo Problem { get; init; } = null!;
    public List<ReplyInfo> Replies { get; init; } = null!;
    public string Once { get; init; } = null!;
    public string TopicLink { get; init; } = null!;
    public string ReportLink { get; init; } = null!;
    public string HasReport{ get; init; } = null!;
    public string Fade { get; init; } = null!;
    public string Sticky { get; init; } = null!;
    internal static TopicInfo Parse(string html)
    {
        var document = new HtmlDocument();
        document.LoadHtml(html);
        var topicInfo = new TopicInfo();
        var header = HeaderInfo.Parse(document.DocumentNode.SelectSingleNode("div#Wrapper"));
        var content = ContentInfo.Parse(document.DocumentNode.SelectSingleNode("div.content div.box"));
        var problem = ProblemInfo.Parse(document.DocumentNode.SelectSingleNode("div.problem"));
        var replies = document.DocumentNode.SelectNodes("div[id^=r_]")
            .Select(ReplyInfo.Parse)
            .ToList();
        var once = document.DocumentNode.SelectSingleNode("input[name=once]").GetAttributeValue("value", "");
        var topicLink = document.DocumentNode.SelectSingleNode("meta[property=og:url]").GetAttributeValue("content", "");
        var reportLink = document.DocumentNode.SelectSingleNode("a[onclick*=/report/topic/]").GetAttributeValue("onclick", "");
        var hasReport = document.DocumentNode.SelectSingleNode("div.content div.box div.inner span.fade").InnerText;
        var fade = document.DocumentNode.SelectSingleNode("a[onclick*=/fade/topic/]").GetAttributeValue("onclick", "");
        var sticky = document.DocumentNode.SelectSingleNode("a[onclick*=/sticky/topic/]").GetAttributeValue("onclick", "");

        return new TopicInfo
        {
            Header = header,
            Content = content,
            Problem = problem,
            Replies = replies,
            Once = once,
            TopicLink = topicLink,
            ReportLink = reportLink,
            HasReport = hasReport,
            Fade = fade,
            Sticky = sticky,
        };
    }

    public class ProblemInfo
    {
        public string Title { get; set; } = null!;
        public List<string?> Tips { get; set; } = null!;
        internal static ProblemInfo Parse(HtmlNode htmlNode)
        {
            var title = htmlNode.GetAttributeValue("ownText", null);
            var tips = htmlNode.SelectNodes("ul li").Select(o => o.ToString())
                .ToList();
            return new ProblemInfo
            {
                Title = title,
                Tips = tips,
            };
        }
    }

    public class ReplyInfo
    {
        public string Content { get; init; } = null!;
        public string UserName { get; init; } = null!;
        public string Avatar { get; init; } = null!;
        public string Time { get; init; } = null!;
        public string Love { get; init; } = null!;
        public string Floor { get; init; } = null!;
        public string AlreadyThanked { get; init; } = null!;
        public string Id { get; init; } = null!;
        internal static ReplyInfo Parse(HtmlNode htmlNode)
        {
            var document = new HtmlDocument();
            document.LoadHtml(htmlNode.InnerHtml);
            var content = document.DocumentNode.SelectSingleNode("div.reply_content").InnerHtml;
            var userName = document.DocumentNode.SelectSingleNode("strong a.dark[href^=/member]").InnerText;
            var avatar = document.DocumentNode.SelectSingleNode("img.avatar").GetAttributeValue("src", "");
            var time = document.DocumentNode.SelectSingleNode("span.fade.small:not(:contains(♥))").InnerText;
            var love = document.DocumentNode.SelectSingleNode("span.small.fade:has(img)").InnerText;
            var floor = document.DocumentNode.SelectSingleNode("span.no").InnerText;
            var alreadyThanked = document.DocumentNode.SelectSingleNode("div.thank_area.thanked").InnerText;
            var id = document.DocumentNode.SelectSingleNode("id").InnerText;

            return new ReplyInfo
            {
                Content = content,
                UserName = userName,
                Avatar = avatar,
                Time = time,
                Love = love,
                Floor = floor,
                AlreadyThanked = alreadyThanked,
                Id = id,
            };
        }
    }

    public class HeaderInfo
    {
        public string Avatar { get; init; } = null!;
        public string UserName { get; init; } = null!;
        public string Time { get; init; } = null!;
        public string Tag { get; init; } = null!;
        public string TagLink { get; init; } = null!;
        public string Comment { get; init; } = null!;
        public int Page { get; init; }
        public int CurrentPage { get; init; }

        public string Title { get; init; } = null!;
        public string FavoriteLink { get; init; } = null!;
        public string ThankedText { get; init; } = null!;
        public string CanSendThanksText { get; init; } = null!;
        public string AppendText { get; init; } = null!;
        internal static HeaderInfo Parse(HtmlNode htmlNode)
        {
            var avatar = htmlNode.SelectSingleNode("div.box img.avatar").GetAttributeValue("src", "");
            var userName = htmlNode.SelectSingleNode("div.box small.gray a").InnerText;
            var time = htmlNode.SelectSingleNode("div.box small.gray").GetAttributeValue("ownText", null);
            var tag = htmlNode.SelectSingleNode("div.box a[href^=/go]").InnerText;
            var tagLink = htmlNode.SelectSingleNode("div.box a[href^=/go]").GetAttributeValue("href", "");
            var comment = htmlNode.SelectSingleNode("div.cell span.gray:contains(回复)").InnerText;
            var page = htmlNode.SelectSingleNode("div.box a.page_normal:last-child").InnerText;
            var currentPage = htmlNode.SelectSingleNode("div.box span.page_current").InnerText;
            var title = htmlNode.SelectSingleNode("div.box h1").InnerText;
            var favoriteLink = htmlNode.SelectSingleNode("div.box a[href*=favorite/]").GetAttributeValue("href", "");
            var thankedText = htmlNode.SelectSingleNode("div.box div[id=topic_thank]").InnerText;
            var canSendThanksText = htmlNode.SelectSingleNode("div.box div.inner div#topic_thank").InnerText;
            var appendText = htmlNode.SelectSingleNode("div.box div.header a.op").InnerText;

            return new HeaderInfo
            {
                Avatar = avatar,
                UserName = userName,
                Time = time,
                Tag = tag,
                TagLink = tagLink,
                Comment = comment,
                Page = int.Parse(page),
                CurrentPage = int.Parse(currentPage),
                Title = title,
                FavoriteLink = favoriteLink,
                ThankedText = thankedText,
                CanSendThanksText = canSendThanksText,
                AppendText = appendText,
            };
        }
    }
    public class ContentInfo
    {
        public string Html { get; init; } = null!;
        public string? FormattedHtml { get; init; };
        internal static ContentInfo Parse(HtmlNode htmlNode)
        {
            var content = htmlNode.OuterHtml;
            var contentHtml = htmlNode;
            contentHtml.Element("header").Remove();
            contentHtml.Element("inner").Remove();
            string? formattedHtml;
            if (string.IsNullOrEmpty(contentHtml.InnerText)
                && contentHtml.SelectSingleNode(".embedded_video_wrapper") == null)
            {
                formattedHtml = null;
            }
            else
            {
                formattedHtml = contentHtml.InnerHtml;
            }

            return new ContentInfo
            {
                Html = content,
                FormattedHtml = formattedHtml,
            };
        }
    }
}
 