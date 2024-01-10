using FluentAssertions;
using Redis.Common.RespTypes;
using Xunit;
using Assert = Xunit.Assert;

namespace Redis.Common.Tests.RespTypes;

public class SimpleStringTests
{
    [Fact]
    public void Serialize_ShouldReturnCorrectString()
    {
        var s = new RespSimpleString("foo");
        var serializedData = s.Serialize();
        serializedData.Should().Be("+foo\r\n");
    }
}