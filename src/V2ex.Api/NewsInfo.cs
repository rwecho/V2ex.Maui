using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace V2ex.Api;

[HasXPath]
[DebuggerDisplay("{DebuggerDisplay}")]
public class NewsInfo
{
    private string DebuggerDisplay => $"Items:{this.Items.Count}";

    [XPath("//div[contains(@class, 'cell') and contains(@class, 'item')]", ReturnType.InnerHtml)]
    public List<Item> Items { get; init; } = [];

    [XPath("//body", ReturnType.OuterHtml)]
    [SkipNodeNotFound]
    public UserInfo? CurrentUser { get; init; }

    [HasXPath]
    [DebuggerDisplay("{Title}-{UserName}")]

    public class Item
    {
        private string? lastRepliedText;

        [XPath("//span[contains(@class, 'item_title')]/a")]
        public string Title { get; init; } = null!;

        [XPath("//span[contains(@class, 'item_title')]/a", "href")]
        [SkipNodeNotFound]
        public string? Link { get; init; }

        [XPath("//td/a/img", "src")]
        public string Avatar { get; init; } = null!;

        [XPath("//td/a", "href")]
        public string AvatarLink { get; init; } = null!;

        [XPath("//td/span[1]/strong/a")]
        public string UserName { get; init; } = null!;

        [XPath("//td/span[1]/strong/a", "href")]
        public string UserLink { get; init; } = null!;


        [XPath("//td/span[3]/strong/preceding-sibling::text()")]
        [SkipNodeNotFound]
        public string? LastRepliedText
        {
            get => lastRepliedText;
            init
            {
                //  1 小时 1 分钟前 &nbsp;•&nbsp; 最后回复来自
                if (value == null)
                { return; }

                var parts = value.Split("&nbsp;•&nbsp;");
                if (parts.Length == 2)
                {
                    lastRepliedText = parts[0].Trim();
                }
            }
        }

        public bool IsPinned
        {
            get
            {
                return this.LastRepliedText == "置顶";
            }
        }

        [XPath("//td/span[3]/strong/a")]
        [SkipNodeNotFound]
        public string? LastRepliedBy { get; init; }

        [XPath("//td/span[1]/a")]
        public string NodeName { get; init; } = null!;

        [XPath("//td/span[1]/a", "href")]
        public string NodeLink { get; init; } = null!;

        [XPath("//a[starts-with(@class, 'count_')]")]
        [SkipNodeNotFound]
        public int Replies { get; init; }

        public string Id => Link == null ? throw new ArgumentNullException(nameof(Link)) : UrlUtilities.ParseId(Link);

        public string NodeId => UrlUtilities.ParseId(NodeLink);
    }
}