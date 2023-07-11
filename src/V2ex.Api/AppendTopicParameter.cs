using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;

namespace V2ex.Api;

public class AppendTopicParameter
{
    public string Once { get; init; } = null!;
    public List<TipInfo> Tips { get; set; } = new();
    public ProblemInfo? Problem { get; init; }
    internal static AppendTopicParameter Parse(string html)
    {
        var document = new HtmlDocument();
        document.LoadHtml(html);
        var wrapper = document.DocumentNode.SelectSingleNode("div#Wrapper");

        var once = wrapper.SelectSingleNode("input[name='once']").Attributes["value"].Value;
        var tips = wrapper.SelectNodes("div.inner ul li")
            .Select(TipInfo.Parse)
            .ToList();
        var problem = ProblemInfo.Parse(wrapper.SelectSingleNode("div.problem"));
        return new AppendTopicParameter
        {
            Once = once,
            Tips = tips,
            Problem = problem,
        };
    }

    public class TipInfo
    {
        public string Text { get; init; } = null!;

        internal static TipInfo Parse(HtmlNode node)
        {
            var text = node.InnerText;
            return new TipInfo
            {
                Text = text,
            };
        }
    }

    public class ProblemInfo
    {
        public string Title { get; init; } = null!;
        public List<string> Tips { get; init; } = new();

        internal static ProblemInfo Parse(HtmlNode htmlNode)
        {
            var title = htmlNode.GetAttributeValue("ownText", null);
            var tips = htmlNode.SelectNodes("ul li").Select(x => x.InnerText).ToList();
            return new ProblemInfo
            {
                Title = title,
                Tips = tips,
            };
        }
    }
}
