using HtmlAgilityPack;
using System.Collections.Generic;

namespace V2ex.Api;

public class MemberPageInfo
{
    public string UserName { get; init; } = null!;
    public string Avatar { get; init; } = null!;
    public string Description { get; init; } = null!;
    public string Online { get; init; } = null!;
    public string FollowOnClick { get; init; } = null!;
    public string BlockOnClick { get; init; } = null!;
    public List<TopicItemInfo> TopicItems { get; init; } = null!;
    public List<ReplyDockerItemInfo> DockItems { get; init; } = null!;
    public List<ReplyContentItemInfo> ReplyContentItems { get; init; } = null!;

    public class TopicItemInfo
    {
        public string UserName { get; init; } = null!;
        public string Tag { get; init; } = null!;
        public string TagLink { get; init; } = null!;
        public string title { get; init; } = null!;
        public string Link { get; init; } = null!;
        public string Time { get; init; } = null!;
        public int ReplyNumber { get; init; }
    }

    public class ReplyDockerItemInfo
    {
        public string Title { get; init; } = null!;
        public string Link { get; init; } = null!;
        public string Time { get; init; } = null!;
    }

    public class ReplyContentItemInfo
    {
        public string Content { get; init; } = null!;

        internal static ReplyContentItemInfo Parse(HtmlNode node)
        {
            return new ReplyContentItemInfo
            {
                Content = node.InnerHtml,
            };
        }
    }
}