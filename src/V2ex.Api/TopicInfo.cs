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

    [XPath("//div[@id='Main']//div[@class='header']/small/span", "title")]
    public DateTime Created{ get; set; }

    [XPath("//div[@id='Main']//div[@class='header']/small/span")]
    public string CreatedText { get; set; } = null!;

    [XPath("//div[@id='Main']//div[@class='topic_buttons']/div[contains(@class, 'topic_stats')]")]
    [SkipNodeNotFound]
    public string? TopicStats { get; set; }

    [XPath("//div[@id='Main']//div[@class='topic_content']", ReturnType.InnerHtml)]
    [SkipNodeNotFound]
    public string? Content { get; set; }

    [XPath("//div[@id='Main']//div[@class='header']/a[2]")]
    public string NodeName { get; set; } = null!;

    [XPath("//div[@id='Main']//div[@class='header']/a[2]", "href")]
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

    [XPath("//div[@id='Main']//div[@class='cell' and contains(@id, 'r_')]", "id")]
    [SkipNodeNotFound]
    public List<string> ReplyIds { get; set; } = new();

    public string NodeId
    {
        get
        {
            return new UriBuilder(UrlUtils.CompleteUrl(NodeLink)).Path.Split("/").Last();
        }
    }

    public string Id
    {
        get
        {
            //todo get id of topic;
            //return new UriBuilder(UrlUtils.CompleteUrl(Link)).Path.Split("/").Last();
            return "";
        }
    }

    [HasXPath]
    [DebuggerDisplay("{Content}")]
    public class ReplyInfo
    {
        [XPath("//td/div[@class='reply_content']")]
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
 