using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace V2ex.Api;

[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class DailyHotInfo : List<DailyHotInfo.Item>
{
    public string Url { get; internal set; } = null!;

    private string DebuggerDisplay
    {
        get
        {
            return $"{this.Count}";
        }
    }

    [DebuggerDisplay("{Title,nq}")]
    public class Item
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Url { get; set; } = null!;
        public string Content { get; set; } = null!;
        [JsonPropertyName("content_rendered")]
        public string? ContentRendered { get; set; }
        public int Replies { get; set; }
        [JsonPropertyName("last_modified")]
        public long? LastModified { get; set; }
        public long Created { get; set; }
        [JsonPropertyName("last_touched")]
        public long? LastTouched { get; set; }
        [JsonPropertyName("last_reply_by")]
        public string? LastReplyBy { get; set; }
        public Member Member { get; set; } = null!;
        public Node Node { get; set; } = null!;
    }

    [DebuggerDisplay("{UserName,nq}")]
    public class Member
    {
        public int Id { get; set; } 

        [JsonPropertyName("username")]
        public string UserName { get; set; } = null!;
        public string? Url { get; set; } 
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
        public long? LastModified { get; set; }
    }

    [DebuggerDisplay("{ParentNodeName,nq}/{Title,nq}")]
    public class Node
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Title { get; set; } = null!;
        public int Stars { get; set; }
        public bool Root { get; set; }
        [JsonPropertyName("parent_node_name")]
        public string? ParentNodeName { get; set; }
        public string[]? Aliases { get; set; }
        [JsonPropertyName("title_alternative")]
        public string TitleAlternative { get; set; } = null!;
        public string? Url { get; set; }
        public int Topics { get; set; }
        public string? Footer { get; set; }
        public string? Header { get; set; }

        [JsonPropertyName("avatar_large")]
        public string? Avatar { get; set; }
    }
}
