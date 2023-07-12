using HtmlAgilityPack;
using System.Collections.Generic;

namespace V2ex.Api;

[HasXPath]
public class LoginProblem
{
    [XPath("//div[@class='problem']//li")]
    public List<string> Errors { get; set; } = new();

    public bool HasProblem()
    {
        return Errors.Count > 0;
    }
}
