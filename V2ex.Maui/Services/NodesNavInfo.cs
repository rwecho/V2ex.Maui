using HtmlAgilityPack;

namespace V2ex.Maui.Services;

public class NodesNavInfo: List<NodesNavInfo.ItemInfo>
{
    internal static NodesNavInfo Parse(string content)
    {
        var document = new HtmlDocument();
        document.LoadHtml(content);
        var wrap = document.DocumentNode.SelectNodes("div.box:last-child div > table");
        var nodes = wrap.Select(ItemInfo.Parse).ToList();
        var nodesNavInfo = new NodesNavInfo();
        nodesNavInfo.AddRange(nodes);
        return nodesNavInfo;
    }

    public class ItemInfo
    {
        public string Category { get; set; } = null!;

        public List<NodeItemInfo> Nodes { get; set; } = new();

        internal static ItemInfo Parse(HtmlNode node)
        {
            var category = node.SelectSingleNode("span.fade").InnerText;
            var nodes = node.SelectNodes("a").Select(NodeItemInfo.Parse).ToList();
            return new ItemInfo
            {
                Category = category,
                Nodes = nodes,
            };
        }

        public class NodeItemInfo
        {
            public string Name { get; set; } = null!;
            public string Link { get; set; } = null!;

            internal static NodeItemInfo Parse(HtmlNode node)
            {
                var a = node.SelectSingleNode("a");
                return new NodeItemInfo
                {
                    Name = a.InnerText,
                    Link = a.GetAttributeValue("href", ""),
                };
            }
        }
    }
}
 