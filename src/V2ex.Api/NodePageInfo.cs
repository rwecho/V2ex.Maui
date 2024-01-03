using HtmlAgilityPack;
using System.Collections.Generic;
using System.Diagnostics;

namespace V2ex.Api;

[HasXPath]
[DebuggerDisplay("{CurrentPage}/{MaximumPage}")]
public class NodePageInfo
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

    [XPath("//div[@id='Wrapper']//div[@class='box']/div[contains(@class,'cell')]", ReturnType.OuterHtml)]
    [SkipNodeNotFound]
    public List<ItemInfo> Items { get; set; } = new();

    [HasXPath]
    [DebuggerDisplay("{UserName,nq} {TopicTitle,nq}")]
    public class ItemInfo
    {
        [XPath("//td/a/img", "src")]
        [SkipNodeNotFound]
        public string Avatar { get; init; } = null!;

        [XPath("//td/span/strong")]
        public string UserName { get; init; } = null!;

        public string UserLink
        {
            get
            {
                return $"/member/{UserName}";
            }
        }

        [XPath("//span[@class='item_title']/a")]
        public string TopicTitle { get; init; } = null!;

        [XPath("//span[@class='item_title']/a", "href")]
        public string TopicLink { get; init; } = null!;

        [XPath("//td/a[contains(@class, 'count_')]")]
        [SkipNodeNotFound]
        public int Replies { get; init; }
    }
}