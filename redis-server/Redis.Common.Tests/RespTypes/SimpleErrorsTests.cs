using Redis.Common.RespTypes;
using Xunit;
using Assert = Xunit.Assert;

namespace Redis.Common.Tests.RespTypes;

public class SimpleErrorsTests
{
    [Fact]
    public void Serialize_ShouldReturnCorrectString()
    {
        var error = new RespSimpleError("ERR unknown command 'foobar'");
        var result = error.Serialize();
        Assert.Equal("-ERR unknown command 'foobar'\r\n", result);
    }
}