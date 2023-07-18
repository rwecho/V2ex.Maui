using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace V2ex.Api;

[HasXPath]
[DebuggerDisplay("{CurrentPage}/{MaximumPage}")]
public class FollowingInfo
{
    [XPath("(//div[@id='Main']//a[@class='page_current'])[1]")]
    [SkipNodeNotFound]
    public int CurrentPage { get; set; }

    [XPath("(//div[@id='Main']//a[@class='page_normal'])[last()]")]
    [SkipNodeNotFound]
    public int MaximumPage { get; set; }

    [XPath("//div[@id='Main']//div[@class='cell item']")]
    [SkipNodeNotFound]
    public List<ItemInfo> Items { get; set; } = new();

    [HasXPath]
    [DebuggerDisplay("{UserName,nq} {TopicTitle,nq}")]
    public class ItemInfo
    {
        [XPath("//td/a/img", "src")]
        [SkipNodeNotFound]
        public string Avatar { get; init; } = null!;

        [XPath("//span[@class='topic_info']/strong[1]/a")]
        public string UserName { get; init; } = null!;

        [XPath("//span[@class='topic_info']/strong[1]/a", "href")]
        public string UserLink { get; init; } = null!;

        [XPath("//span[@class='item_title']/a")]
        public string TopicTitle { get; init; } = null!;

        [XPath("//span[@class='item_title']/a", "href")]
        public string TopicLink { get; init; } = null!;

        [XPath("//td/a[contains(@class, 'count_')]")]
        [SkipNodeNotFound]
        public int Replies { get; init; }

        [XPath("//span[@class='topic_info']/a[@class='node']")]
        public string NodeName { get; init; } = null!;

        [XPath("//span[@class='topic_info']/a[@class='node']", "href")]
        public string NodeLink { get; init; } = null!;

        [XPath("//span[@class='topic_info']/span", "title")]
        [SkipNodeNotFound]
        public DateTime Created { get; set; }

        [XPath("//span[@class='topic_info']/span")]
        [SkipNodeNotFound]
        public string? CreatedText { get; set; }

        [XPath("//span[@class='topic_info']/strong[2]/a")]
        [SkipNodeNotFound]
        public string? LastReplyUserName { get; set; }

        [XPath("//span[@class='topic_info']/strong[2]/a", "href")]
        [SkipNodeNotFound]
        public string? LastReplyUserLink { get; set; }

        public string Id
        {
            get
            {
                return new UriBuilder(UrlUtils.CompleteUrl(TopicLink)).Path.Split("/").Last();
            }
        }

        public string NodeId
        {
            get
            {
                return new UriBuilder(UrlUtils.CompleteUrl(NodeLink)).Path.Split("/").Last();
            }
        }
    }
}