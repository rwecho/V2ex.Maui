using HtmlAgilityPack;
using System.Diagnostics;

namespace V2ex.Api;

[HasXPath]
[DebuggerDisplay("{Name}")]
public class UserInfo
{
    [XPath("//td/span/a")]
    public string Name { get; set; } = null!;

    [XPath("//td/a/img", "src")]
    public string Avatar { get; set; } = null!;

    [XPath("//td/a[@href='/my/nodes']/span")]
    public int Nodes { get; set; }
    [XPath("//td/a[@href='/my/topics']/span")]
    public int Topics { get; set; }
    [XPath("//td/a[@href='/my/following']/span")]
    public int Following { get; set; }

    [XPath("//div/a[@href='/notifications']")]
    public string? Notifications { get; set; }

    [XPath("//div[@id='money']/a/text()[1]")]
    public string? MoneyGold { get; set; }
    [XPath("//div[@id='money']/a/text()[2]")]
    public string? MoneySilver { get; set; }
    [XPath("//div[@id='money']/a/text()[3]")]
    public string? MoneyBronze { get; set; }
}
