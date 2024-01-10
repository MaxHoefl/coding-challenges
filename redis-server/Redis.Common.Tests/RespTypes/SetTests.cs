using FluentAssertions;
using Redis.Common.RespTypes;
using Xunit;

namespace Redis.Common.Tests.RespTypes;

public class SetTests
{
    [Fact]
    public void Serialize_ShouldReturnCorrectString()
    {
        var set = new RespSet(new HashSet<RespType>
        {
            new RespSimpleString("foo"), new RespInteger(1), new RespInteger(1)
        });
        var serializedData = set.Serialize();
        serializedData.Should().BeEquivalentTo("~2\r\n+foo\r\n:1\r\n\r\n");
    }
}