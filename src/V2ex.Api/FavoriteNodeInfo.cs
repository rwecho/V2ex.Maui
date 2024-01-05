using HtmlAgilityPack;
using System.Collections.Generic;
using System.Diagnostics;

namespace V2ex.Api;


[HasXPath]
[DebuggerDisplay("{Items.Count,nq}")]
public class FavoriteNodeInfo
{
    [XPath("//a[contains(@class, 'av-node')]", ReturnType.OuterHtml)]
    [SkipNodeNotFound]
    public List<ItemInfo> Items { get; init; } = [];

    [HasXPath]
    [DebuggerDisplay("{Name,nq}")]
    public class ItemInfo
    {
        [XPath("//img", "src")]
        public string Image { get; set; } = null!;
        [XPath("//span[@class='fav-node-name']")]
        public string Name { get; init; } = null!;
        [XPath("//span[@class='f12 fade']")]
        public string Topics { get; init; } = null!;

        [XPath("//a", "href")]
        public string Link { get; init; } = null!;
    }
}
 