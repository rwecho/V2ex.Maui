using HtmlAgilityPack;
using System.Diagnostics;

namespace V2ex.Api;

[HasXPath]
[DebuggerDisplay("{Name}")]
public class UserInfo
{
    [XPath("//div[@id='site-header-menu']//img[@class='avatar mobile']", "alt")]
    [SkipNodeNotFound]
    public string? Name { get; init; }

    [XPath("//div[@id='site-header-menu']//img[@class='avatar mobile']", "src")]
    [SkipNodeNotFound]
    public string? Avatar { get; init; }

   
    [XPath("//div[@class='cell flex-one-row']/a[@href='/notifications']")]
    [SkipNodeNotFound]
    public string? Notifications { get; set; }

    [XPath("//div[@class='cell flex-one-row']/a[@href='/balance']/img[@alt='G']/preceding-sibling::text()[1]")]
    [SkipNodeNotFound]
    public string? MoneyGold { get; set; }
    [XPath("//div[@class='cell flex-one-row']/a[@href='/balance']/img[@alt='S']/preceding-sibling::text()[1]")]
    [SkipNodeNotFound]
    public string? MoneySilver { get; set; }
    [XPath("//div[@class='cell flex-one-row']/a[@href='/balance']/img[@alt='B']/preceding-sibling::text()[1]")]
    [SkipNodeNotFound]
    public string? MoneyBronze { get; set; }

    [XPath("//div/a[@href='/mission/daily']")]
    [SkipNodeNotFound]
    public string? DailyMission { get; set; }
}
