using System.Diagnostics;
using System.Text.Json.Serialization;

namespace V2ex.Api;

[DebuggerDisplay("{ParentNodeName,nq}/{Title,nq}")]
public class NodeInfo
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Title { get; set; } = null!;
    [JsonPropertyName("title_alternative")]
    public string TitleAlternative { get; set; } = null!;
    public int Topics{ get; set; }
    public int Stars{ get; set; }
    public string? Header { get; set; }
    public string? Footer { get; set; }
    [JsonPropertyName("parent_node_name")]
    public string? ParentNodeName { get; set; }

    public string[]? Aliases { get; set; }
    [JsonPropertyName("avatar_mini")]
    public string? AvatarMini { get; set; }

    [JsonPropertyName("avatar_large")]
    public string? AvatarLarge { get; set; }


    [JsonPropertyName("avatar_normal")]
    public string? AvatarNormal { get; set; }
}


