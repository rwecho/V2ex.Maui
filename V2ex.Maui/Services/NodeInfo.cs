using System.Text.Json.Serialization;

namespace V2ex.Maui.Services;

public class NodeInfo
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Url { get; set; } = null!;
    public string Title { get; set; } = null!;
    public int Topics{ get; set; }
    public int Stars{ get; set; }
    public string Head{ get; set; } = null!;
    public long Created{ get; set; }

    [JsonPropertyName("avatar_large")]
    public string Avatar { get; set; } = null!;
}
