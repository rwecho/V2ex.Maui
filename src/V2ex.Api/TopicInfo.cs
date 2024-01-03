using HtmlAgilityPack;
using System.Collections.Generic;
using System.Diagnostics;

namespace V2ex.Api;

[HasXPath]
[DebuggerDisplay("{Title}")]
public class TopicInfo
{
    [XPath("//div[@id='Wrapper']//div[@class='header']/h1")]
    public string Title { get; set; } = null!;
    [XPath("//div[@id='Wrapper']//div[@class='header']/small/a")]
    public string UserName { get; set; } = null!;

    [XPath("//div[@id='Wrapper']//div[@class='header']/small/a", "href")]
    public string UserLink { get; set; } = null!;

    [XPath("//div[@id='Wrapper']//div[@class='header']/div[@class='fr']//img", "src")]
    public string Avatar { get; set; } = null!;

    [XPath("//div[@id='Wrapper']//div[@class='header']/small/a/following-sibling::text()")]
    public string CreatedText { get; set; } = null!;

    [XPath("//div[@id='Wrapper']//a[contains(@href, 'favorite/topic')]/preceding-sibling::span")]
    [SkipNodeNotFound]
    public string? TopicStats { get; set; }

    [XPath("//div[@id='Wrapper']//a[contains(@href, 'favorite/topic')]/parent::div/following-sibling::small")]
    [SkipNodeNotFound]
    public string? ViaPhone { get; set; }

    [XPath("//div[@id='Wrapper']//a[contains(@href, 'favorite/topic')]/text()")]
    [SkipNodeNotFound]
    public string? Liked { get; set; }
    
    public bool IsLiked
    {
        get
        {
            return Liked == "取消收藏";
        }
    }

    public bool IsThanked
    {
        get
        {
            return Thanked == "感谢已发送";
        }
    }

    [XPath("//div[@id='Wrapper']//div[@id='topic_thank']/a/text()")]
    [SkipNodeNotFound]
    public string? Thanked { get; set; }

    public bool IsIgnored
    {
        get
        {
            return Ignored == "取消忽略";
        }
    }

    [XPath("//div[@id='Wrapper']//a[contains(@onclick, '/ignore/topic')]/text()")]
    [SkipNodeNotFound]
    public string? Ignored { get; set; }

    [XPath("//div[@id='Wrapper']//div[@class='cell']/div[@class='topic_content']", ReturnType.InnerHtml)]
    [SkipNodeNotFound]
    public string? Content { get; set; }

    [XPath("//div[@id='Wrapper']//div[@class='subtle']", ReturnType.InnerHtml)]
    [SkipNodeNotFound]
    public List<SupplementInfo> Supplements { get; set; } = [];

    [XPath("//div[@id='Wrapper']//div[@class='header']/a[2]")]
    public string NodeName { get; set; } = null!;

    [XPath("//div[@id='Wrapper']//div[@class='header']/a[2]", "href")]
    public string NodeLink { get; set; } = null!;

    [XPath("//div[@id='Wrapper']/div/div[@class='box'][3]/div[@class='cell'][1]/span")]
    [SkipNodeNotFound]
    public string? ReplyStats { get; set; }

    [XPath("//div[@id='Wrapper']//a[@class='tag']")]
    [SkipNodeNotFound]
    public List<string> Tags { get; set; } = [];

    public int CurrentPage
    {
        get
        {
            if (string.IsNullOrEmpty(CurrentPageInfo))
            {
                return 0;
            }
            return int.Parse(CurrentPageInfo.Replace("第", "").Replace("页", "").Trim());
        }
    }

    public int MaximumPage
    {
        get
        {
            if (string.IsNullOrEmpty(MaximumPageInfo))
            {
                return 0;
            }
            return int.Parse(MaximumPageInfo.Replace("共", "").Replace("页", "").Trim());
        }
    }

    [XPath("(//div[@id='Wrapper']//span[@class='page_current'])[1]/preceding-sibling::div[2]")]
    [SkipNodeNotFound]
    public string? CurrentPageInfo { get; init; }

    [XPath("(//div[@id='Wrapper']//span[@class='page_current'])[1]/preceding-sibling::div[1]")]
    [SkipNodeNotFound]
    public string? MaximumPageInfo { get; init; }

    [XPath("//div[@id='Wrapper']//div[@class='cell' and contains(@id, 'r_')]", ReturnType.OuterHtml)]
    [SkipNodeNotFound]
    public List<ReplyInfo> Replies { get; set; } = [];

    [XPath("//div[@id='Wrapper']//input[@id='once']", "value")]
    [SkipNodeNotFound]
    public string? Once { get; set; }

    public string NodeId => UrlUtilities.ParseId(NodeLink);

    [HasXPath]
    [DebuggerDisplay("{Content}")]
    public class SupplementInfo
    {
        [XPath("//span[@class='fade']")]
        [SkipNodeNotFound]
        public string? CreatedText { get; set; }

        [XPath("//div[@class='topic_content']", ReturnType.InnerHtml)]
        public string? Content { get; set; }
    }


    [HasXPath]
    [DebuggerDisplay("{Content}")]
    public class ReplyInfo
    {
        [XPath("div[@class='cell' and contains(@id, 'r_')]", "id")]
        public string Id { get; init; } = null!;
        [XPath("//td/div[@class='reply_content']", ReturnType.InnerHtml)]
        public string Content { get; init; } = null!;
        [XPath("//td/strong/a")]
        public string UserName { get; init; } = null!;

        [XPath("//td/strong/a", "href")]
        public string UserLink { get; init; } = null!;

        [XPath("//img[@class='avatar']", "src")]
        public string Avatar { get; init; } = null!;

        [XPath("//td/strong/following-sibling::span[1]")]
        [SkipNodeNotFound]
        public string? ReplyTimeText { get; init; }

        [XPath("//td/div[@class='badges']")]
        [SkipNodeNotFound]
        public string? Badges { get; init; }

        [XPath("//td//span[@class='no']")]
        public int Floor { get; init; }

        [XPath("//td/strong/following-sibling::span[1]/following-sibling::span")]
        [SkipNodeNotFound]
        public string? Thanks { get; init; }

        [XPath("//td//div[contains(@class, 'thanked')]/text()")]
        [SkipNodeNotFound]
        public string? Thanked { get; init; }
    }
}
