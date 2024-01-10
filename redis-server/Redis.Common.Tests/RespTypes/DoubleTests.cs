using FluentAssertions;
using Redis.Common.RespTypes;
using Xunit;

namespace Redis.Common.Tests.RespTypes;

public class DoubleTests
{
    [Fact]
    public void Serialize_ShouldReturnCorrectString()
    {
        var d1 = new RespDouble(1.23);
        var s1 = d1.Serialize();
        s1.Should().BeEquivalentTo(",1.23\r\n");
        
        var d2 = new RespDouble(-1.23);
        var s2 = d2.Serialize();
        s2.Should().BeEquivalentTo(",-1.23\r\n");
        
        var d3 = new RespDouble(10);
        var s3 = d3.Serialize();
        s3.Should().BeEquivalentTo(",10\r\n");
    }
}