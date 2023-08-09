using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace V2ex.Api;

[HasXPath]
public class TagInfo
{
    [XPath("(//div[@id='Main']//a[@class='page_current'])[1]")]
    [SkipNodeNotFound]
    public int CurrentPage { get; set; }

    [XPath("(//div[@id='Main']//a[@class='page_normal'])[last()]")]
    [SkipNodeNotFound]
    public int MaximumPage { get; set; }

    [XPath("//div[@id='Main']//div[contains(@class,'cell')]", ReturnType.OuterHtml)]
    [SkipNodeNotFound]
    public List<ItemInfo> Items { get; set; } = new();
    public string Url { get; internal set; } = null!;

    [HasXPath]
    public class ItemInfo
    {
        [XPath("//td/a/img", "src")]
        [SkipNodeNotFound]
        public string? Avatar { get; init; }

        [XPath("//span[@class='topic_info']/strong[1]/a")]
        public string UserName { get; init; } = null!;

        [XPath("//span[@class='topic_info']/strong[1]/a", "href")]
        public string UserLink { get; init; } = null!;

        [XPath("//span[@class='item_title']/a")]
        public string TopicTitle { get; init; } = null!;

        [XPath("//span[@class='item_title']/a", "href")]
        public string TopicLink { get; init; } = null!;

        public int Replies { get; init; }

        [XPath("//span[@class='topic_info']/a[@class='node']")]
        public string NodeName { get; init; } = null!;

        [XPath("//span[@class='topic_info']/a[@class='node']", "href")]
        public string NodeLink { get; init; } = null!;

        [XPath("//span[@class='topic_info']/span", "title")]
        [SkipNodeNotFound]
        public DateTime Created { get; set; }

        [XPath("//span[@class='topic_info']/span")]
        public string CreatedText { get; set; } = null!;

        [XPath("//span[@class='topic_info']/strong[2]/a")]
        [SkipNodeNotFound]
        public string? LastReplyUserName { get; set; }
        [XPath("//span[@class='topic_info']/strong[2]/a", "href")]
        [SkipNodeNotFound]
        public string? LastReplyUserLink { get; set; }
    }
}
