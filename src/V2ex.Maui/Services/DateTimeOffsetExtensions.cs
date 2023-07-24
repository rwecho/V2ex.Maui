using Microsoft.Extensions.Localization;
using Volo.Abp.Localization;

namespace V2ex.Maui.Services;

public static class DateTimeOffsetExtensions
{
    public static string ToHumanReadable(this DateTimeOffset dateTime, IStringLocalizer<MauiResource> localizer)
    {
        // convert date time to 1s ago , 5s ago ,1 minute/hour/day/week/month/year and so on.
        var ts = DateTime.Now - dateTime;

        if (ts.TotalSeconds < 10)
        {
            return localizer["JustNow"];
        }

        if (ts.TotalSeconds < 60)
        {
            return localizer.GetString("SecondsAgo", ts.Seconds);
        }

        if (ts.TotalMinutes < 60)
        {
            return localizer.GetString("MinutesAgo", Math.Round(ts.TotalMinutes, 0));
        }

        if (ts.TotalHours < 24)
        {
            return localizer.GetString("HoursAgo", Math.Round(ts.TotalHours, 0));
        }

        if (ts.TotalDays < 365)
        {
            return localizer.GetString("DaysAgo", Math.Round(ts.TotalDays, 0));
        }

        return "yyyy-MM-dd HH:mm:ss";
    }
}