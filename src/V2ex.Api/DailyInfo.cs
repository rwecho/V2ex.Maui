using HtmlAgilityPack;

namespace V2ex.Api;

public class DailyInfo
{
    public string UserLink { get; init; } = null!;
    public string Avatar { get; init; } = null!;
    public string Title { get; init; } = null!;
    public string ContinuousLoginDay { get; init; } = null!;
    public string CheckinUrl { get; init; } = null!;
    internal static DailyInfo Parse(string html)
    {
        var document = new HtmlDocument();
        document.LoadHtml(html);
        var userLink = document.DocumentNode.SelectSingleNode("[href^=/member]").GetAttributeValue("href", null);
        var avatar = document.DocumentNode.SelectSingleNode("img[src*=avatar/]").GetAttributeValue("src", null);
        var title = document.DocumentNode.SelectSingleNode("h1").InnerText;
        var continuousLoginDay = document.DocumentNode.SelectSingleNode("div.cell:contains(已连续)").InnerText;
        var checkinUrl = document.DocumentNode.SelectSingleNode("div.cell input[type=button]").GetAttributeValue("onclick", null);

        return new DailyInfo
        {
            UserLink = userLink,
            Avatar = avatar,
            Title = title,
            ContinuousLoginDay = continuousLoginDay,
            CheckinUrl = checkinUrl,
        };
    }
}

