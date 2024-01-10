using FluentAssertions;
using Redis.Common.RespTypes;
using Xunit;

namespace Redis.Common.Tests.RespTypes;

public class BulkStringTests
{
    [Fact]
    public void Serialize_ShouldReturnCorrectString()
    {
        var s = new RespBulkString("foo");
        var serializedData = s.Serialize();
        serializedData.Should().Be("$3\r\nfoo\r\n");
    }
}