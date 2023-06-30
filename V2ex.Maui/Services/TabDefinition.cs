namespace V2ex.Maui.Services;

public record TabDefinition(string Name, string Description)
{
    public static IReadOnlyList<TabDefinition> GetTabDefinitions()
    {
        return new[]
        {
            new TabDefinition("all", "全部"),
            new TabDefinition("tech", "技术"),
            new TabDefinition("creative", "创意"),
            new TabDefinition("play", "好玩"),
            new TabDefinition("apple", "Apple"),
            new TabDefinition("jobs", "酷工作"),
            new TabDefinition("deals", "交易"),
            new TabDefinition("city", "城市"),
            new TabDefinition("qna", "问与答"),
            new TabDefinition("hot", "最热"),
            new TabDefinition("r2", "R2"),
            new TabDefinition("nodes", "节点"),
            new TabDefinition("members", "关注"),
        };
    }
}
