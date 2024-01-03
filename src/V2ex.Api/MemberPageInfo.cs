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

    [XPath("//div[@id='Wrapper']//td/h1")]
    public string UserName { get; init; } = null!;

    [XPath("//div[@id='Wrapper']//td/img", "src")]
    public string Avatar { get; init; } = null!;
    [XPath("//div[@id='Wrapper']//td/h1/following-sibling::span[@class='gray']")]
    [SkipNodeNotFound]
    public string? CreatedText { get; init; }

    [XPath("//div[@id='Wrapper']//td/strong[@class='online']")]
    [SkipNodeNotFound]
    public string? IsOnline { get; set; }

    [XPath("//span[@class='item_title']/ancestor::table", ReturnType.InnerHtml)]
    [SkipNodeNotFound]
    public List<TopicInfo> Topics { get; init; } = [];

    [XPath("//div[@class='dock_area']", ReturnType.OuterHtml)]
    [SkipNodeNotFound]
    public List<ReplyInfo> Replies { get; init; } = [];

    [XPath("//div[@class='reply_content']", ReturnType.InnerHtml)]
    [SkipNodeNotFound]
    public List<string> ReplyContents { get; init; } = [];

    [HasXPath]
    [DebuggerDisplay("{Title}-{UserName}")]
    public class TopicInfo
    {
        [XPath("//td/span[1]/strong/a")]
        public string UserName { get; init; } = null!;

        [XPath("//td/span[3]/strong/a")]
        [SkipNodeNotFound]
        public string? LatestReplyBy { get; init; }

        [XPath("//td/span[1]/a[@class='node']")]
        public string NodeName { get; init; } = null!;
        [XPath("//td/span[1]/a[@class='node']", "href")]
        public string NodeLink { get; init; } = null!;

        [XPath("//td/span[3]")]
        [SkipNodeNotFound]
        public string? CreatedText { get; set; }

        [XPath("//span[@class='item_title']/a")]
        public string Title { get; init; } = null!;
        [XPath("//span[@class='item_title']/a", "href")]
        public string Link { get; init; } = null!;
        [XPath("//td/a[@class='count_livid']")]
        [SkipNodeNotFound]
        public int Replies { get; init; }
    }

    [HasXPath]
    [DebuggerDisplay("{OpUserName}-{TopicTitle}")]
    public class ReplyInfo
    {
        [XPath("//td/span[1]")]
        public string OpUserName { get; init; } = null!;

        [XPath("//td/span[1]/a")]
        public string TopicTitle { get; init; } = null!;
        [XPath("//td/span[1]/a", "href")]
        public string TopicLink { get; init; } = null!;

        [XPath("//td/span[2]")]
        public string ReplyTimeText { get; init; } = null!;
    }
}