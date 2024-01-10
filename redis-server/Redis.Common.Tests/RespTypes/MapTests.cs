using FluentAssertions;
using Redis.Common.RespTypes;
using Xunit;

namespace Redis.Common.Tests.RespTypes;

public class MapTests
{
    [Fact]
    public void Serialize_ShouldReturnCorrectString()
    {
        var map = new RespMap(new Dictionary<RespType, RespType>
        {
            { new RespSimpleString("foo"), new RespInteger(1) },
            { new RespBulkString("bar"), new RespBoolean(false) }
        });
        var serializedData = map.Serialize();
        serializedData.Should().Be("%2\r\n+foo\r\n:1\r\n$3\r\nbar\r\n#f\r\n\r\n");
    }
}