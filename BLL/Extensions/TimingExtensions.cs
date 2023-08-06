namespace BusinessLogicLayer.Extensions;

public static class TimingExtensions
{
    public static int? ConvertFromDateTimeToUnixTimestamp(this DateTime? date)
    {
        if (date == null)
            return null;

        return (int?)date.Value.ToUniversalTime()
              .Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc))
              .TotalSeconds;
    }

    public static int ConvertFromTimeStringToMinutes(this string time)
    {
        var timeSpan = TimeSpan.Parse(time);

        return (int)timeSpan.TotalMinutes;
    }

    public static DateTime? ConvertFromUnixTimestampToDateTime(this int? timestamp)
    {
        if (timestamp == null) return null;

        var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        return epoch.AddSeconds((int)timestamp);
    }

    public static string ConvertFromMinutesToTimeString(this int? minutes)
    {
        if (minutes == null) return null;

        var timeSpan = TimeSpan.FromMinutes((int)minutes);

        return timeSpan.ToString(@"hh\:mm");
    }
}
