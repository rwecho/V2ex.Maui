using System.Collections.Generic;

namespace V2ex.Api;

public record TabDefinition(string Name, string Description, int Order, bool IsVisible)
{
    public static IReadOnlyList<TabDefinition> GetTabDefinitions()
    {
        return new[]
        {
            new TabDefinition("all", "全部", 0, true),
            new TabDefinition("tech", "技术", 0, true),
            new TabDefinition("creative", "创意", 0, true),
            new TabDefinition("play", "好玩", 0, true),
            new TabDefinition("apple", "Apple", 0, true),
            new TabDefinition("jobs", "酷工作", 0, true),
            new TabDefinition("deals", "交易", 0, true),
            new TabDefinition("city", "城市", 0, true),
            new TabDefinition("qna", "问与答", 0, true),
            new TabDefinition("hot", "最热", 0, true),
            new TabDefinition("r2", "R2", 0, true),
            new TabDefinition("nodes", "节点", 0, true),
            new TabDefinition("members", "关注", 0, true),
        };
    }
}
