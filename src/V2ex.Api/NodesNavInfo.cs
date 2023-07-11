using HtmlAgilityPack;
using System.Collections.Generic;

namespace V2ex.Api;

[HasXPath]
public class NodesNavInfo
{
    [XPath("//td/span[@class='fade']/../..")]
    public List<ItemInfo> Items { get; set; } = new();

    [HasXPath]
    public class ItemInfo
    {
        [XPath("//td/span[@class='fade']")]
        public string Category { get; set; } = null!;

        [XPath("//td/a")]
        public List<NodeItemInfo> Nodes { get; set; } = new();

        [HasXPath]
        public class NodeItemInfo
        {
            [XPath(".")]
            public string Name { get; set; } = null!;

            [XPath(".", "href")]
            public string Link { get; set; } = null!;
        }
    }
}