using HtmlAgilityPack;
using System.Collections.Generic;
using System.Diagnostics;

namespace V2ex.Api;

[HasXPath]
[DebuggerDisplay("{Once}")]
public class CreateTopicParameter
{
    [XPath("(//input[@name='once'])[1]", "value")]
    public string Once { get; init; } = null!;

    [XPath("//div[@class='cell'][5]/a[@class='node']", ReturnType.OuterHtml)]
    public List<HotNode> HotNodes { get; init; } = new();

    public string Url { get; internal set; } = null!;
    [HasXPath]
    public class HotNode
    {
        [XPath("/a", ReturnType.InnerText)]
        public string Title { get; init; } = null!;

        [XPath("/a", "href")]
        public string Link { get; init; } = null!;
    }
}
