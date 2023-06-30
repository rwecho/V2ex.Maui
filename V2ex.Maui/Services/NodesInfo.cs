namespace V2ex.Maui.Services;

// https://v2ex.com/api/nodes/s2.json
public class NodesInfo : List<NodesInfo.Node>
{
    public class Node
    {
        public string Text { get; set; } = null!;
        public int Topics { get; set; }
        public string Id { get; set; } = null!;
        public bool IsHot { get; set; }
    }
}
