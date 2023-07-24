using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace V2ex.Api;

[HasXPath]
[DebuggerDisplay("{Total}/{MaximumPage}")]
public class NotificationInfo
{
    [XPath("//div[@id='Main']//div[@class='header']//strong")]
    [SkipNodeNotFound]
    public int Total { get; set; }

    [XPath("(//div[@id='Main']//a[@class='page_current'])[1]")]
    [SkipNodeNotFound]
    public int CurrentPage { get; set; }

    [XPath("(//div[@id='Main']//a[@class='page_normal'])[last()]")]
    [SkipNodeNotFound]
    public int MaximumPage { get; set; }

    [XPath("//div[contains(@id, 'n_')]", ReturnType.OuterHtml)]
    public List<NotificationItemInfo> Items { get; set; } = new();

    [XPath("//div[contains(@id, 'n_')]", "id")]
    public List<string> ItemIds { get; set; } = new();

    [HasXPath]
    [DebuggerDisplay("{DebuggerDisplay, nq}")]
    public class NotificationItemInfo
    {
        private string DebuggerDisplay
        {
            get
            {
                return $"{this.PreTitle} {this.TopicTitle} {this.PostTitle}";
            }
        }
        [XPath("//td//strong")]
        public string UserName { get; set; } = null!;

        [XPath("//td//strong/..", "href")]
        public string UserLink { get; set; } = null!;

        [XPath("//td/a/img", "src")]
        public string Avatar { get; set; } = null!;

        [XPath("//td//a[@class='topic-link']", "href")]
        public string TopicLink { get; set; } = null!;

        [XPath("//td//a[@class='topic-link']")]
        public string TopicTitle { get; set; } = null!;

        [XPath("//td//a[@class='topic-link']/preceding-sibling::text()")]
        [SkipNodeNotFound]
        public string? PreTitle { get; set; }

        [XPath("//td//a[@class='topic-link']/following-sibling::text()")]
        [SkipNodeNotFound]
        public string? PostTitle { get; set; }

        [XPath("//td//span[@class='snow']")]
        [SkipNodeNotFound]
        public string? Created { get; set; }

        [XPath("//td//div[@class='payload']", ReturnType.InnerHtml)]
        [SkipNodeNotFound]
        public string? Payload { get; set; }

        public string Id
        {
            get
            {
                return new UriBuilder(UrlUtils.CompleteUrl(TopicLink)).Path.Split("/").Last();
            }
        }
    }
}