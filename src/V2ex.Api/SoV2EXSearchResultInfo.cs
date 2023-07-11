using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace V2ex.Api;

[DebuggerDisplay("{Total}")]
public class SoV2EXSearchResultInfo
{
    public int Took { get; set; }
    public int Total { get; set; }

    public HitInfo[]? Hits { get; set; }

    [DebuggerDisplay("{Source}")]
    public class HitInfo
    {
        [JsonPropertyName("_source")]
        public SourceInfo Source { get; set; } = null!;

        [DebuggerDisplay("{Title}")]
        public class SourceInfo
        {
            public int Id { get; set; }
            public string Title { get; set; } = null!;
            public string Content { get; set; } = null!;
            [JsonPropertyName("node")]
            public int NodeId { get; set; }
            public long Replies { get; set; }
            [JsonPropertyName("created")]
            public DateTime Created { get; set; }
            [JsonPropertyName("member")]
            public string Creator { get; set; } = null!;
        }
    }
}
