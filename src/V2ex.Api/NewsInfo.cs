using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace V2ex.Api;

[HasXPath]
[DebuggerDisplay("{DebuggerDisplay}")]
public class NewsInfo
{
    private string DebuggerDisplay => $"Items:{this.Items.Count}";

    [XPath("//div[contains(@class, 'cell') and contains(@class, 'item')]", ReturnType.InnerHtml)]
    public List<NewsInfo.Item> Items { get; init; } = new();

    [XPath("//div[@id='Rightbar']//a[@href='/my/nodes']/ancestor::div[@class='box']", ReturnType.OuterHtml)]
    [SkipNodeNotFound]
    public UserInfo? CurrentUser { get; init; }

    [HasXPath]
    [DebuggerDisplay("{Title}-{UserName}")]
    public class Item
    {
        [XPath("//span[contains(@class, 'item_title')]/a")]
        [SkipNodeNotFound]
        public string Title { get; init; } = null!;

        [XPath("//span[contains(@class, 'item_title')]/a", "href")]
        [SkipNodeNotFound]
        public string Link { get; init; } = null!;

        [XPath("//td/a/img", "src")]
        public string Avatar { get; init; } = null!;

        [XPath("//td/a", "href")]
        public string AvatarLink { get; init; } = null!;

        [XPath("//span[contains(@class, 'topic_info')]/strong[1]/a")]
        public string UserName { get; init; } = null!;

        [XPath("//span[contains(@class, 'topic_info')]/strong[1]/a", "href")]
        public string UserLink { get; init; } = null!;

        [XPath("//span[contains(@class, 'topic_info')]/span[1]", "title")]
        [SkipNodeNotFound]
        public DateTime LastReplied { get; init; }

        [XPath("//span[contains(@class, 'topic_info')]/span[1]")]
        [SkipNodeNotFound]
        public string? LastRepliedText { get; init; }

        [XPath("//span[contains(@class, 'topic_info')]/strong[2]/a")]
        [SkipNodeNotFound]
        public string? LastRepliedBy { get; init; }

        [XPath("//span[contains(@class, 'topic_info')]/a[@class='node']")]
        public string NodeName { get; init; } = null!;

        [XPath("//span[contains(@class, 'topic_info')]/a[@class='node']", "href")]
        public string NodeLink { get; init; } = null!;

        [XPath("//a[starts-with(@class, 'count_')]")]
        [SkipNodeNotFound]
        public int Replies { get; init; }

        public string Id
        {
            get
            {
                return new UriBuilder(UrlUtilities.CompleteUrl(Link)).Path.Split("/").Last();
            }
        }

        public string NodeId
        {
            get
            {
                return new UriBuilder(UrlUtilities.CompleteUrl(NodeLink)).Path.Split("/").Last();
            }
        }
    }
}