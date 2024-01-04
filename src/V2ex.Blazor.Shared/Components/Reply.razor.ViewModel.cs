using Microsoft.AspNetCore.Components;
using System.Text.RegularExpressions;

namespace V2ex.Blazor.Components;
public record ReplyViewModel(string Id,
    MarkupString? Content,
    string UserName,
    string UserLink,
    string Avatar,
    MarkupString? ReplyTimeText,
    string? Badges,
    int Floor
)
{
    public string NormalizedId => Id.Replace("r_", "");
    public bool Thanked { get; set; }
    public int Thanks { get; set; }
    public List<ReplyViewModel> Replies { get; } = new();
    public ReplyViewModel? Parent { get; set; }
    public Action? StateChanged = default!;
    private List<Mention> _mentions = default!;
    public List<Mention> Mentions
    {
        get
        {
            if (_mentions == null)
            {
                _mentions = new List<Mention>();
                // parse content
                if (Content != null)
                {
                    var matches = Regex.Matches(Content.Value.Value, @"@<a href=.+?>(.+?)</a> (#(\d+))?");
                    foreach (Match match in matches)
                    {
                        int.TryParse(match.Groups[3].Value, out var floor);
                        _mentions.Add(new(match.Groups[1].Value, floor));
                    }
                }
            }
            return _mentions;
        }
    }
};
