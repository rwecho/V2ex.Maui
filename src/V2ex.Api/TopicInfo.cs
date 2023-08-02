using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace V2ex.Api;

[HasXPath]
[DebuggerDisplay("{Title}")]
public class TopicInfo
{
    [XPath("//div[@id='Main']//div[@class='header']/h1")]
    [SkipNodeNotFound]
    public string Title { get; set; } = null!;
    [XPath("//div[@id='Main']//div[@class='header']/small/a")]
    [SkipNodeNotFound]
    public string UserName { get; set; } = null!;

    [XPath("//div[@id='Main']//div[@class='header']/small/a", "href")]
    [SkipNodeNotFound]
    public string UserLink { get; set; } = null!;   

    [XPath("//div[@id='Main']//div[@class='header']/div[@class='fr']//img", "src")]
    [SkipNodeNotFound]
    public string Avatar { get; set; } = null!;

    [XPath("//div[@id='Main']//div[@class='header']/small/span", "title", NodeReturnType = ReturnType.InnerHtml)]
    [SkipNodeNotFound]
    public DateTime Created{ get; set; }

    [XPath("//div[@id='Main']//div[@class='header']/small/span")]
    [SkipNodeNotFound]
    public string CreatedText { get; set; } = null!;

    [XPath("//div[@id='Main']//div[@class='topic_buttons']/div[contains(@class, 'topic_stats')]")]
    [SkipNodeNotFound]
    public string? TopicStats { get; set; }

    [XPath("//div[@id='Main']//div[@class='cell']/div[@class='topic_content']", ReturnType.InnerHtml)]
    [SkipNodeNotFound]
    public string? Content { get; set; }

    [XPath("//div[@id='Main']//div[@class='subtle']", ReturnType.InnerHtml)]
    [SkipNodeNotFound]
    public List<SupplementInfo> Supplements { get; set; } = new();

    [XPath("//div[@id='Main']//div[@class='header']/a[2]")]
    [SkipNodeNotFound]
    public string NodeName { get; set; } = null!;

    [XPath("//div[@id='Main']//div[@class='header']/a[2]", "href")]
    [SkipNodeNotFound]
    public string NodeLink { get; set; } = null!;

    [XPath("//div[@id='Main']//a[@class='tag']/../../span")]
    [SkipNodeNotFound]
    public string? ReplyStats { get; set; }

    [XPath("//div[@id='Main']//a[@class='tag']")]
    [SkipNodeNotFound]
    public List<string> Tags { get; set; } = new();

    [XPath("//div[@id='Main']//a[@class='page_current'][1]")]
    [SkipNodeNotFound]
    public int CurrentPage { get; set; }

    [XPath("//div[@id='Main']//a[@class='normal_page'][last()]")]
    [SkipNodeNotFound]
    public int MaximumPage { get; set; }

    [XPath("//div[@id='Main']//div[@class='cell' and contains(@id, 'r_')]", ReturnType.OuterHtml)]
    [SkipNodeNotFound]
    public List<ReplyInfo> Replies { get; set; } = new();

    public string NodeId
    {
        get
        {
            return new UriBuilder(UrlUtilities.CompleteUrl(NodeLink)).Path.Split("/").Last();
        }
    }

    [HasXPath]
    [DebuggerDisplay("{Content}")]
    public class SupplementInfo
    {
        [XPath("//span[@class='fade']/span", "title")]
        [SkipNodeNotFound]
        public DateTime Created { get; set; }
        [XPath("//span[@class='fade']/span")]
        [SkipNodeNotFound]
        public string CreatedText { get; set; } = null!;

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
        [SkipNodeNotFound]
        public string UserName { get; init; } = null!;
        [XPath("//td/strong/a", "href")]
        public string UserLink { get; init; } = null!;
        [XPath("//img[@class='avatar']", "src")]
        public string Avatar { get; init; } = null!;
        [XPath("//td/span[@class='ago']", "title")]
        public DateTime ReplyTime { get; init; }

        [XPath("//td/span[@class='ago']")]
        public string ReplyTimeText { get; init; } = null!;

        [XPath("//td/badges")]
        [SkipNodeNotFound]
        public string? Badges { get; init; } 

        [XPath("//td//span[@class='no']")]
        public int Floor { get; init; }

        [XPath("//td/span[contains(@class, 'small fade')]")]
        [SkipNodeNotFound]
        public string? AlreadyThanked { get; init; }
    }
}
 