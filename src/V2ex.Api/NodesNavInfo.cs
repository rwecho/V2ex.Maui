using HtmlAgilityPack;
using System.Collections.Generic;
using System.Diagnostics;

namespace V2ex.Api;

[HasXPath]
[DebuggerDisplay("{DebuggerDisplay}")]
public class NodesNavInfo
{
    [XPath("//td/span[@class='fade']/../..", ReturnType.OuterHtml)]
    public List<ItemInfo> Items { get; set; } = new();
    public string Url { get; internal set; } = null!;

    private string DebuggerDisplay
    {
        get
        {
            return $"Count = {this.Items.Count}";
        }
    }

    [HasXPath]
    [DebuggerDisplay("{Category}")]
    public class ItemInfo
    {
        [XPath("//td/span[@class='fade']")]
        public string Category { get; set; } = null!;

        [XPath("//td/a", ReturnType.OuterHtml)]
        [SkipNodeNotFound]
        public List<NodeItemInfo> Nodes { get; set; } = new();

        [HasXPath]
        [DebuggerDisplay("{Name}")]
        public class NodeItemInfo
        {
            [XPath("a")]
            [SkipNodeNotFound]
            public string Name { get; set; } = null!;

            [XPath("a", "href")]
            [SkipNodeNotFound]
            public string Link { get; set; } = null!;
        }
    }
}