using FluentAssertions;
using Redis.Common.RespTypes;
using Xunit;

namespace Redis.Common.Tests.RespTypes;

public class BooleanTests
{
    [Fact]
    public void Serialize_ShouldReturnCorrectString()
    {
        var isTrue = new RespBoolean(true);
        isTrue.Serialize().Should().BeEquivalentTo("#t\r\n");
        
        var isFalse = new RespBoolean(false);
        isFalse.Serialize().Should().BeEquivalentTo("#f\r\n");
    }
}