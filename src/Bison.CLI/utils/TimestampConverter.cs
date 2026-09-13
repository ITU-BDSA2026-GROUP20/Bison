namespace Bison.CLI.utils;

public static class TimestampConverter
{
    public static DateTime FromUnixSeconds(string secondsString)
        => DateTime.UnixEpoch.AddSeconds(long.Parse(secondsString));

    //Should implement version with long as well
}