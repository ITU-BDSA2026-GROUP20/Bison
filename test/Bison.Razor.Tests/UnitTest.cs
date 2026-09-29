using Bison.Core.utils;

namespace Bison.Razor.Tests;

public class UnitTest
{
    [Fact]
    public void FromUnixSecondsString_ConvertsTimestamp()
    {
        long input = 1609459200; // 2021-01-01 at 00:00:00
        DateTime expected = new DateTime(2021,1,1,0,0,0, DateTimeKind.Utc);

        DateTime actual = TimestampConverter.FromUnixSeconds(input.ToString());

        Assert.Equal(expected, actual);
    }
    [Fact]
    public void FromUnixSecondsLong_ConvertsTimestamp()
    {
        long input = 1609459200; // 2021-01-01 at 00:00:00
        DateTime expected = new DateTime(2021,1,1,0,0,0, DateTimeKind.Utc);

        DateTime actual = TimestampConverter.FromUnixSecondsLong(input);

        Assert.Equal(expected, actual);
    }
}