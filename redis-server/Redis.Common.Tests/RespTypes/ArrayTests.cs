using FluentAssertions;
using Redis.Common.RespTypes;
using Xunit;

namespace Redis.Common.Tests.RespTypes;

public class ArrayTests
{
    [Fact]
    public void Serialize_ShouldReturnCorrectString()
    {
        var array = new RespArray(new RespType[]
        {
            new RespSimpleString("foo"),
            new RespInteger(1),
            new RespNull()
        });
        var result = array.Serialize();
        result.Should().BeEquivalentTo("*3\r\n+foo\r\n:1\r\n_\r\n\r\n");
    }
}