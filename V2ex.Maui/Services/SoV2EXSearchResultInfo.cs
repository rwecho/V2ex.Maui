using System.Text.Json.Serialization;

namespace V2ex.Maui.Services;

public class SoV2EXSearchResultInfo
{
    public int Total { get; set; }

    public List<Hit> Hits { get; set; } = null!;

    public class Hit
    {
        [JsonPropertyName("_source")]
        public SourceNode Source { get; set; } = null!;
        public class SourceNode
        {
            public string Id { get; set; } = null!;
            public string Title { get; set; } = null!;
            public string Content { get; set; } = null!;
            [JsonPropertyName("node")]
            public string NodeId { get; set; } = null!;
            public long Replies { get; set; }
            [JsonPropertyName("created")]
            public DateTime Created { get; set; }
            [JsonPropertyName("member")]
            public string Creator = null!;
        }
    }
}
