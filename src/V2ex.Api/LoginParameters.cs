using HtmlAgilityPack;
using System;
using System.Diagnostics;

namespace V2ex.Api;

[HasXPath]
[DebuggerDisplay("{Once}")]
public class LoginParameters
{
    [XPath("(//input[@class='sl' and @type='text'])[1]", "name")]
    public string NameParameter { get; init; } = null!;

    [XPath("//input[@class='sl' and @type='password']", "name")]
    public string PasswordParameter { get; init; } = null!;

    [XPath("//input[@type='hidden' and @name='once']", "value")]
    public string Once { get; init; } = null!;

    [XPath("(//input[@class='sl' and @type='text'])[last()]", "name")]
    public string CaptchaParameter { get; init; } = null!;

    [XPath("//td/img[@id='captcha-image']", "src")]
    public string Captcha { get; init; } = null!;


    public string GetCaptchaImageUrl()
    {
        return $"{UrlUtils.BASE_URL}{this.Captcha}?once={Once}";
    }
}