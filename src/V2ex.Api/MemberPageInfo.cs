using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace V2ex.Api;

[HasXPath]
[DebuggerDisplay("{DebuggerDisplay}")]
public class MemberPageInfo
{
    private string DebuggerDisplay => $"UserName:{this.UserName}";

    [XPath("//div[@id='Main']//td/h1")]
    public string UserName { get; init; } = null!;
    [XPath("//div[@id='Main']//td/span[@class='bigger'][1]")]
    [SkipNodeNotFound]
    public string? Tagline { get; init; }

    [XPath("//div[@id='Main']//td/img", "src")]
    public string Avatar { get; init; } = null!;
    [XPath("//div[@id='Main']//span/a[@href='/top/dau']")]
    [SkipNodeNotFound]
    public string? Bank { get; init; }
    [XPath("//div[@id='Main']//td/span[@class='bigger']/following-sibling::span[@class='gray']/text()[1]")]
    [SkipNodeNotFound]
    public string? CreatedText { get; init; }
    public string? FollowOnClick { get; init; } = null!;
    public string? BlockOnClick { get; init; } = null!;
    [XPath("//span[@class='item_title']/ancestor::table")]
    [SkipNodeNotFound]
    public List<TopicInfo> Topics { get; init; } = new();

    [XPath("//div[@class='dock_area']")]
    [SkipNodeNotFound]
    public List<ReplyInfo> Replies { get; init; } = new();

    [XPath("//div[@class='inner']/div[@class='reply_content']", ReturnType.InnerHtml)]
    [SkipNodeNotFound]
    public List<string> ReplyContents { get; init; } = new();

    [HasXPath]
    [DebuggerDisplay("{Title}-{UserName}")]
    public class TopicInfo
    {
        [XPath("//span[@class='topic_info']/strong[1]/a")]
        public string UserName { get; init; } = null!;
        [XPath("//span[@class='topic_info']/strong[2]/a")]
        [SkipNodeNotFound]
        public string? LatestReplyUserName { get; init; }
        [XPath("//span[@class='topic_info']/a[@class='node']")]
        public string NodeName { get; init; } = null!;
        [XPath("//span[@class='topic_info']/a[@class='node']", "href")]
        public string NodeLink { get; init; } = null!;

        [XPath("//span[@class='topic_info']/span")]
        [SkipNodeNotFound]
        public string? CreatedText { get; set; }

        [XPath("//span[@class='topic_info']/span", "title")]
        [SkipNodeNotFound]
        public DateTime Created { get; set; }
        [XPath("//span[@class='item_title']/a")]
        public string Title { get; init; } = null!;
        [XPath("//span[@class='item_title']/a", "href")]
        public string Link { get; init; } = null!;
        [XPath("//td/a[@class='count_livid']")]
        [SkipNodeNotFound]
        public int ReplyNumber { get; init; }
    }

    [HasXPath]
    [DebuggerDisplay("{OpUserName}-{TopicTitle}")]
    public class ReplyInfo
    {
        [XPath("//td/span[@class='gray']/a[1]")]
        public string OpUserName { get; init; } = null!;
        [XPath("//td/span[@class='gray']/a[2]")]
        public string NodeName { get; init; } = null!;
        [XPath("//td/span[@class='gray']/a[3]")]
        public string TopicTitle { get; init; } = null!;
        [XPath("//td/span[@class='gray']/a[3]", "href")]
        public string TopicLink { get; init; } = null!;

        [XPath("//td/div[@class='fr']/span[@class='fade']", "title")]
        public DateTime ReplyTime { get; init; }

        [XPath("//td/div[@class='fr']/span[@class='fade']")]
        public string ReplyTimeText { get; init; } = null!;
    }
}