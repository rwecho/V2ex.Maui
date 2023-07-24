using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace V2ex.Api;


[HasXPath]
[DebuggerDisplay("{Items.Count,nq}")]
public class FavoriteNodeInfo
{
    [XPath("//a[contains(@class, 'av-node')]", ReturnType.OuterHtml)]
    public List<ItemInfo> Items { get; init; } = new();

    [HasXPath]
    [DebuggerDisplay("{Name,nq}")]
    public class ItemInfo
    {
        [XPath("/img", "src")]
        public string Image { get; init; } = null!;
        [XPath("/span[@class='fav-node-name']")]
        public string Name { get; init; } = null!;
        [XPath("/span[@class='f12 fade']")]
        public string Topics { get; init; } = null!;

        // todo
        public string Link { get; set; } = null!;

        public string Id
        {
            get
            {
                return new UriBuilder(UrlUtils.CompleteUrl(Link)).Path.Split("/").Last();
            }
        }
    }
}
 