using Microsoft.AspNetCore.Components;
using System.Text.RegularExpressions;
using V2ex.Api;
using V2ex.Blazor.Components;

namespace V2ex.Blazor.Pages;

public record TopicPageViewModel(
    string NodeId,
    string Title,
    string UserName,
    string UserLink,
    string Avatar,
    string CreatedText,
    MarkupString? Content,
    List<SupplementViewModel> Supplements,
    string NodeName,
    string NodeLink,
    List<string> Tags
    )
{
    private string? topicStats;
    private string? replyStats;

    public int CurrentPage { get; protected set; }
    public int MaximumPage { get; protected set; }

    public bool Liked { get;  set; }

    public string? TopicStats
    {
        get => topicStats; set
        {
            topicStats = value;

            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            var match1 = Regex.Match(value, @"(\d+) 次点击");
            if (match1.Success)
            {
                this.Clicks = int.Parse(match1.Groups[1].Value);
            }

            var match2 = Regex.Match(value, @"(\d+) 人收藏");
            if (match2.Success)
            {
                this.Likes = int.Parse(match2.Groups[1].Value);
            }
            var match3 = Regex.Match(value, @"(\d+) 人感谢");
            if (match3.Success)
            {
                this.Thanks = int.Parse(match3.Groups[1].Value);
            }
        }
    }

    public string? ReplyStats 
    { 
        get => replyStats; 
        set
        {
            replyStats = value;

            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            var match1 = Regex.Match(value, @"(\d+) 条回复");
            if (match1.Success)
            {
                this.RepliesCount = int.Parse(match1.Groups[1].Value);
            }

            if(DateTime.TryParse( value.Split(";").Last(), out var lastReplyTime))
            {
                this.LastReplyTime = lastReplyTime;
            }

        }
    }

    public int Clicks { get; set; }

    public int RepliesCount { get; set; }

    public DateTime? LastReplyTime { get; set; }

    public int Likes
    {
        get; set;
    }

    public int Thanks
    {
        get; set;
    }


    public bool Thanked { get;  set; }
    public bool Ignored { get;  set; }
    public string? Once { get; set; }
    public List<ReplyViewModel> Replies { get; } = [];


    public static TopicPageViewModel Create(TopicInfo topicInfo)
    {
        var supplements = topicInfo.Supplements.Select(o => new SupplementViewModel(
           o.CreatedText,
           o.Content == null ? null : new MarkupString(o.Content)
           )).ToList();
        var viewModel = new TopicPageViewModel(
        topicInfo.NodeId,
        topicInfo.Title,
        topicInfo.UserName,
        topicInfo.UserLink,
        topicInfo.Avatar,
        topicInfo.CreatedText,
        topicInfo.Content == null ? null : new MarkupString(topicInfo.Content),
        supplements,
        topicInfo.NodeName,
        topicInfo.NodeLink,
        topicInfo.Tags
        );

        viewModel.Update(topicInfo);
        return viewModel;
    }

    public void Update(TopicInfo topicInfo)
    {
        var replies = topicInfo.Replies.Select(o => new ReplyViewModel(o.Id,
            o.Content == null ? null : new MarkupString(o.Content),
            o.UserName,
            o.UserLink,
            o.Avatar,
            o.ReplyTimeText == null? null: new MarkupString(o.ReplyTimeText),
            o.Badges,
            o.Floor)
        {
            Thanked = o.Thanked != null,
            Thanks = int.TryParse(o.Thanks, out var thanks) ? thanks : 0
        }).ToList();

        CurrentPage = topicInfo.CurrentPage;
        MaximumPage = topicInfo.MaximumPage;

        foreach (var reply in replies)
        {
            if(Replies.Any(t=>t.Id == reply.Id))
            {
                continue;
            }
            Replies.Add(reply);
        }

        ReplyStats = topicInfo.ReplyStats;
        TopicStats = topicInfo.TopicStats;
        Once = topicInfo.Once;
        Liked = topicInfo.IsLiked;
        Thanked = topicInfo.IsThanked;
        Ignored = topicInfo.IsIgnored;
    }
}

