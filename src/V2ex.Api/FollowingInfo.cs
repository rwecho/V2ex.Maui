﻿using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace V2ex.Api;

[HasXPath]
[DebuggerDisplay("{CurrentPage}/{MaximumPage}")]
public class FollowingInfo
{
    [XPath("(//div[@id='Wrapper']//input)[1]", "min")]
    [SkipNodeNotFound]
    public int CurrentPage { get; set; }

    [XPath("(//div[@id='Wrapper']//input)[1]", "max")]
    [SkipNodeNotFound]
    public int MaximumPage { get; set; }

    [XPath("//div[@id='Wrapper']//div[@class='cell item']", ReturnType.OuterHtml)]
    [SkipNodeNotFound]
    public List<ItemInfo> Items { get; set; } = [];

    [HasXPath]
    [DebuggerDisplay("{UserName,nq} {TopicTitle,nq}")]
    public class ItemInfo
    {
        [XPath("//td/a/img", "src")]
        [SkipNodeNotFound]
        public string? Avatar { get; init; }

        [XPath("//span[@class='topic_info']/strong[1]/a")]
        public string UserName { get; init; } = null!;

        [XPath("//span[@class='topic_info']/strong[1]/a", "href")]
        public string UserLink { get; init; } = null!;

        [XPath("//span[@class='item_title']/a")]
        public string TopicTitle { get; init; } = null!;

        [XPath("//span[@class='item_title']/a", "href")]
        public string TopicLink { get; init; } = null!;

        [XPath("//td/a[contains(@class, 'count_')]")]
        [SkipNodeNotFound]
        public int Replies { get; init; }

        [XPath("//span[@class='topic_info']/a[@class='node']")]
        public string NodeName { get; init; } = null!;

        [XPath("//span[@class='topic_info']/a[@class='node']", "href")]
        public string NodeLink { get; init; } = null!;

        [XPath("//span[@class='topic_info']/span", "title")]
        [SkipNodeNotFound]
        public DateTime Created { get; set; }

        [XPath("//span[@class='topic_info']/span")]
        [SkipNodeNotFound]
        public string? CreatedText { get; set; }

        [XPath("//span[@class='topic_info']/strong[2]/a")]
        [SkipNodeNotFound]
        public string? LastReplyUserName { get; set; }

        [XPath("//span[@class='topic_info']/strong[2]/a", "href")]
        [SkipNodeNotFound]
        public string? LastReplyUserLink { get; set; }

        public string Id => UrlUtilities.ParseId(TopicLink);

        public string NodeId => UrlUtilities.ParseId(NodeLink);
    }
}