using System.Text.Json.Serialization;

namespace V2ex.Maui.Services;

public class MemberInfo
{
    public string Status { get; set; } = null!;

    private string Id { get; set; } = null!;

    [JsonPropertyName("username")]
    private string UserName { get; set; } = null!;

    private string? Website { get; set; }
    private string? Twitter { get; set; }
    private string? Psn { get; set; }
    private string? Github { get; set; }
    private string? Btc { get; set; }
    private string? Location { get; set; }
    private string? Tagline { get; set; }
    private string? Bio { get; set; }
    private string Avatar { get; set; } = null!;
    private long Created { get; set; }
}