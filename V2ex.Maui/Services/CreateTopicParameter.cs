using HtmlAgilityPack;

namespace V2ex.Maui.Services;

public class CreateTopicParameter
{
    public string Once { get; init; } = null!;
    public List<string> HotNodes { get; init } = new();
    public ProblemInfo? Problem { get; init; }

    internal static CreateTopicParameter Parse(string html)
    {
        var document = new HtmlDocument();
        document.LoadHtml(html);
        var wrapper = document.DocumentNode.SelectSingleNode("div#Wrapper");

        var once = wrapper.SelectSingleNode("input[name='once']").GetAttributeValue("value", null);
        var hotTitles = wrapper.SelectNodes("a.node")
            .Select(o => o.GetAttributeValue("href", null))
            .ToList();
        var problem = ProblemInfo.Parse(wrapper.SelectSingleNode("div.problem"));

        return new CreateTopicParameter
        {
            Once = once,
            HotNodes = hotTitles,
            Problem = problem,
        };
    }

    public class ProblemInfo
    {
        public string Title { get; init; } = null!;
        public List<string> Tips { get; init; } = new();

        internal static ProblemInfo Parse(HtmlNode node)
        {
            var title = node.GetAttributeValue("ownText", null);
            var tips = node.SelectNodes("ul li").Select(o => o.InnerText).ToList();
            return new ProblemInfo
            {
                Title = title,
                Tips = tips,
            };
        }
    }
}