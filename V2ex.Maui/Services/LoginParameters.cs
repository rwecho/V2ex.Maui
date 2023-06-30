using HtmlAgilityPack;

namespace V2ex.Maui.Services;

public class LoginParameters
{
    public string NameParameter { get; init; } = null!;
    public string PasswordParameter{ get; init; } = null!;
    public string Once{ get; init; } = null!;
    public string CaptchaParameter { get; init; } = null!;

    public string? Problem { get; init; }
    public static LoginParameters Parse(string html)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(html);
        var name = doc.DocumentNode
            .SelectSingleNode("input.sl[type=text]")
            ?.GetAttributeValue("name", null);

        var password = doc.DocumentNode
            .SelectSingleNode("input[type=password]")
            ?.GetAttributeValue("name", null);

        var once = doc.DocumentNode
            .SelectSingleNode("input[name=once]")
            ?.GetAttributeValue("value", null);
        var problem = doc.DocumentNode
            .SelectSingleNode("div.problem")
            ?.InnerHtml;

        var captcha = doc.DocumentNode
            .SelectSingleNode("input[placeholder*=验证码]")
            ?.GetAttributeValue("name", null);

        if (name is null || password is null || once is null || captcha is null)
            throw new InvalidOperationException("解析登录参数失败");

        return new LoginParameters
        {
            NameParameter = name,
            PasswordParameter = password,
            Once = once,
            CaptchaParameter = captcha,
            Problem = problem,
        };
    }
}
