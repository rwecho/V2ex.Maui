using HtmlAgilityPack;
using System.Collections.Generic;

namespace V2ex.Api;

[HasXPath]
public class Problem
{
    [XPath("//div[@class='problem']//li")]
    [SkipNodeNotFound]
    public List<string> Errors { get; set; } = new();

    public bool HasProblem()
    {
        return Errors.Count > 0;
    }
}
