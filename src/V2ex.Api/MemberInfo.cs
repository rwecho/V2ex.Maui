using System.Diagnostics;
using System.Text.Json.Serialization;

namespace V2ex.Api;

[DebuggerDisplay("{UserName}")]
public class MemberInfo
{
    public int Id { get; set; }

    [JsonPropertyName("username")]
    public string UserName { get; set; } = null!;
    public string? Website { get; set; }
    public string? Twitter { get; set; }
    public string? Psn { get; set; }
    public string? Github { get; set; }
    public string? Btc { get; set; }
    public string? Location { get; set; }
    public string? Tagline { get; set; }
    public string? Bio { get; set; }

    [JsonPropertyName("avatar_mini")]
    public string? AvatarMini { get; set; }
    [JsonPropertyName("avatar_normal")]
    public string? AvatarNormal { get; set; }
    [JsonPropertyName("avatar_large")]
    public string? AvatarLarge { get; set; }
    [JsonPropertyName("avatar_xlarge")]
    public string? AvatarXlarge { get; set; }
    [JsonPropertyName("avatar_xxlarge")]
    public string? AvatarXxlarge { get; set; }
    [JsonPropertyName("avatar_xxxlarge")]
    public string? AvatarXxxlarge { get; set; }
    public long Created { get; set; }

    [JsonPropertyName("last_modified")]
    public long LastModified { get; set; }
    public string? Status { get; set; }
}