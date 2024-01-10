using FluentAssertions;
using Redis.Common.RespTypes;
using Redis.Common.RespTypes.Derived;
using Xunit;

namespace Redis.Common.Tests;

public class RespDeserializerTests
{
    [Fact]
    public void Test_Deserialize_SimpleString()
    {
        var s1 = "+Hello, World!\r\n";
        var res = RespDeserializer.Deserialize(s1);
        res.Should().BeOfType<RespSimpleString>();
        ((RespSimpleString)res).Data.Should().Be("Hello, World!");
    } 
    
    [Fact]
    public void Test_Deserialize_Array()
    {
        var a = "*3\r\n$3\r\nfoo\r\n$3\r\nbar\r\n$5\r\nHello\r\n";
        var res = RespDeserializer.Deserialize(a);
        res.Should().BeOfType<RespArray>();
        var data = ((RespArray)res).Data.ToList();
        data.Count.Should().Be(3);
        data[0].Should().BeOfType<RespBulkString>();
        data[1].Should().BeOfType<RespBulkString>();
        data[2].Should().BeOfType<RespBulkString>();
        ((RespBulkString)data[0]).Data.Should().Be("foo");
        ((RespBulkString)data[1]).Data.Should().Be("bar");
        ((RespBulkString)data[2]).Data.Should().Be("Hello");
    }

    [Fact]
    public void Test_Deserialize_Map()
    {
        var s = "%2\r\n+first\r\n:1\r\n+second\r\n:2\r\n";
        var res = RespDeserializer.Deserialize(s);
        res.Should().BeOfType<RespMap>();
        var data = ((RespMap)res).Data;
        data.Count.Should().Be(2);
        data.Keys.ToList()[0].Should().BeOfType<RespSimpleString>();
        data.Keys.ToList()[1].Should().BeOfType<RespSimpleString>();
        data.Values.ToList()[0].Should().BeOfType<RespInteger>();
        data.Values.ToList()[1].Should().BeOfType<RespInteger>();
        ((RespSimpleString)data.Keys.ToList()[0]).Data.Should().Be("first");
        ((RespSimpleString)data.Keys.ToList()[1]).Data.Should().Be("second");
        ((RespInteger)data.Values.ToList()[0]).Data.Should().Be(1);
        ((RespInteger)data.Values.ToList()[1]).Data.Should().Be(2);
    }

    [Fact]
    public void Test_Deserialize_Set()
    {
        var s = "~2\r\n+first\r\n+second\r\n";
        var res = RespDeserializer.Deserialize(s);
        res.Should().BeOfType<RespSet>();
        var data = ((RespSet)res).Data.ToHashSet();
        data.Count.Should().Be(2);
        data.ToList()[0].Should().BeOfType<RespSimpleString>();
        data.ToList()[1].Should().BeOfType<RespSimpleString>();
        ((RespSimpleString)data.ToList()[0]).Data.Should().Be("first");
        ((RespSimpleString)data.ToList()[1]).Data.Should().Be("second");
    }
    
    [Fact]
    public void Test_IsIndexAtPrimitiveTypeDiscriminator()
    {
        var s = "+Hello, World!\r\n";
        var res = RespDeserializer.IsIndexAtPrimitiveTypeDiscriminator(s[0]);
        res.Should().BeTrue();
        
        s = "-Error\r\n";
        res = RespDeserializer.IsIndexAtPrimitiveTypeDiscriminator(s[0]);
        res.Should().BeTrue();
        
        s = ":-1\r\n";
        res = RespDeserializer.IsIndexAtPrimitiveTypeDiscriminator(s[0]);
        res.Should().BeTrue();
        
        s = "$1\r\n+OK\r\n\r\n";
        res = RespDeserializer.IsIndexAtPrimitiveTypeDiscriminator(s[0]);
        res.Should().BeTrue();
        
        s = "#t\r\n";
        res = RespDeserializer.IsIndexAtPrimitiveTypeDiscriminator(s[0]);
        res.Should().BeTrue();
        
        s = ",1.5\r\n";
        res = RespDeserializer.IsIndexAtPrimitiveTypeDiscriminator(s[0]);
        res.Should().BeTrue();
        
        s = "*1+OK\r\n";
        res = RespDeserializer.IsIndexAtPrimitiveTypeDiscriminator(s[0]);
        res.Should().BeFalse();
        
        s = "%2\r\n+first\r\n:1\r\n+second\r\n:2\r\n";
        res = RespDeserializer.IsIndexAtPrimitiveTypeDiscriminator(s[0]);
        res.Should().BeFalse();

        s = "~1\r\n+first\r\n";
        res = RespDeserializer.IsIndexAtPrimitiveTypeDiscriminator(s[0]);
        res.Should().BeFalse();
    }

    [Fact]
    public void Test_IsIndexAtNonPrimitiveTypeDiscriminator()
    {
        var s = "*1+OK\r\n";
        var res = RespDeserializer.IsIndexAtNonPrimitiveTypeDiscriminator(s[0]);
        res.Should().BeTrue();
        
        s = "%\r\n+first\r\n:1\r\n+second\r\n:2\r\n";
        res = RespDeserializer.IsIndexAtNonPrimitiveTypeDiscriminator(s[0]);
        res.Should().BeTrue();

        s = "~1\r\n+first\r\n";
        res = RespDeserializer.IsIndexAtNonPrimitiveTypeDiscriminator(s[0]);
        res.Should().BeTrue();
        
        s = "+OK\r\n";
        res = RespDeserializer.IsIndexAtNonPrimitiveTypeDiscriminator(s[0]);
        res.Should().BeFalse();
    }

    [Fact]
    public void Test_DeserializePrimitive()
    {
        var s = "+Hello, World!\r\n";
        var res = RespDeserializer.DeserializePrimitive(s);
        res.Should().BeOfType<RespSimpleString>();
        ((RespSimpleString)res).Data.Should().Be("Hello, World!");
        
        s = "-Error\r\n";
        res = RespDeserializer.DeserializePrimitive(s);
        res.Should().BeOfType<RespSimpleError>();
        ((RespSimpleError)res).Data.Should().Be("Error");
        
        s = ":-1\r\n";
        res = RespDeserializer.DeserializePrimitive(s);
        res.Should().BeOfType<RespInteger>();
        ((RespInteger)res).Data.Should().Be(-1);
        
        s = "$3\r\nfoo\r\n";
        res = RespDeserializer.DeserializePrimitive(s);
        res.Should().BeOfType<RespBulkString>();
        ((RespBulkString)res).Data.Length.Should().Be(3);
        ((RespBulkString)res).Data.Should().Be("foo");
        
        s = "#t\r\n";
        res = RespDeserializer.DeserializePrimitive(s);
        res.Should().BeOfType<RespBoolean>();
        ((RespBoolean)res).Data.Should().BeTrue();
        
        s = ",1.5\r\n";
        res = RespDeserializer.DeserializePrimitive(s);
        res.Should().BeOfType<RespDouble>();
        ((RespDouble)res).Data.Should().Be(1.5);
    }

    [Fact]
    public void Test_AddToPendingNonPrimitive_For_Array_On_Stack()
    {
        var obj = new RespSimpleString("OK");
        var stack = new Stack<RespType>();
        stack.Push(new RespArray(Array.Empty<RespType>()));
        RespDeserializer.AddToPendingNonPrimitive(obj, stack);
        stack.Count.Should().Be(1);
        var arr = stack.Pop();
        arr.Should().BeOfType<RespArray>();
        ((RespArray)arr).Data.ToList().Count.Should().Be(1);
    }

    [Fact]
    public void Test_AddToPendingNonPrimitive_For_KV_On_Stack()
    {
        var keyObj = new RespSimpleString("name");
        var stack = new Stack<RespType>();
        stack.Push(new RespMap(new Dictionary<RespType, RespType>()));
        
        RespDeserializer.AddToPendingNonPrimitive(keyObj, stack);
        stack.Count.Should().Be(2);
        var kv = (RespKvPair)stack.Peek();
        kv.Should().BeOfType<RespKvPair>();
        kv.Key.Should().BeOfType<RespSimpleString>();
        ((RespSimpleString)kv.Key).Data.Should().Be("name");
        
        var valueObj = new RespInteger(1);
        RespDeserializer.AddToPendingNonPrimitive(valueObj, stack);
        var map = (RespMap) stack.Peek();
        map.Data.Count.Should().Be(1);
        map.Data[keyObj].Should().BeEquivalentTo(valueObj);
    }

    [Fact]
    public void Test_IndexAtEndOfHead()
    {
        var ptr = 0;
        var data = "*3\r\n$3\r\nfoo\r\n$3\r\nbar\r\n$5\r\nHello\r\n";
        var res = RespDeserializer.IndexAtEndOfHead(ptr, data);
        res.Should().Be(3);
    }

    [Fact]
    public void Test_GetLengthParamFromHead()
    {
        var head = "*3\r\n";
        RespDeserializer.GetLengthParamFromHead(head).Should().Be(3);
    }
}