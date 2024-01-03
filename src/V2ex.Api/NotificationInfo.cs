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

    [XPath("//div[@id='Wrapper']//div[@class='inner']//td/strong")]
    [SkipNodeNotFound]
    public string? PageInfo { get; init; }

    public int CurrentPage
    {
        get
        {
            if (string.IsNullOrEmpty(PageInfo) || !PageInfo.Contains("/"))
            {
                return 0;
            }
            var parts = PageInfo.Split('/');
            return int.Parse(parts[0]);
        }
    }

    public int MaximumPage
    {
        get
        {
            if (string.IsNullOrEmpty(PageInfo) || !PageInfo.Contains("/"))
            {
                return 0;
            }
            var parts = PageInfo.Split('/');
            return int.Parse(parts[1]);
        }
    }

    [XPath("//div[contains(@id, 'n_')]", ReturnType.OuterHtml)]
    [SkipNodeNotFound]
    public List<NotificationItemInfo> Items { get; set; } = [];

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
    }
}