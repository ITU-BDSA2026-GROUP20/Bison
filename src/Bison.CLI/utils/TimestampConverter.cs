namespace Bison.CLI.utils;

public static class TimestampConverter
{
    public static DateTime FromUnixSeconds(string secondsString)
        => DateTime.UnixEpoch.AddSeconds(long.Parse(secondsString));

    public static DateTime FromUnixSecondsLong(long seconds)
        => DateTime.UnixEpoch.AddSeconds(seconds);
}