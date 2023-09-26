namespace V2ex.Api;

public class UnitInfo
{
    public string Url { get; internal set; } = null!;

    internal static UnitInfo Parse(string html)
    {
        return new UnitInfo();
    }
}
