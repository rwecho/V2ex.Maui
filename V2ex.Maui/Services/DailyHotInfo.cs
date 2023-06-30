using System.Text.Json.Serialization;

namespace V2ex.Maui.Services;

public class DailyHotInfo: List<DailyHotInfo.Item>
{
    public class Item
    {
        public string Id { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Url { get; set; } = null!;
        public string Content { get; set; } = null!;
        public int Replies { get; set; }
        public long Time { get; set; }
        public Member Member { get; set; } = null!;
        public Node Node { get; set; } = null!;
    }

    public class Member
    {
        public string Id { get; set; } = null!;

        [JsonPropertyName("username")]
        public string UserName { get; set; } = null!;

        [JsonPropertyName("avatar_large")]
        public string Avatar { get; set; } = null!;
    }

    public class Node
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Url { get; set; } = null!;
        public int Topics { get; set; }

        [JsonPropertyName("avatar_large")]
        public string Avatar { get; set; } = null!;
    }
}
