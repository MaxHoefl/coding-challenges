using System.Globalization;
using System.Runtime.Serialization;
using Redis.Common.RespTypes;
using Redis.Common.RespTypes.Derived;

namespace Redis.Common;

public static class RespDeserializer
{
    private const string Delim = RespConstants.MessageDelimiter;

    private static string SerializedBodyInPrimitiveType(string serializedData)
    {
        /*
         * serializedData is the string that contains the type discriminator and the body of a primitive type
         * e.g. serializedData = "+OK\r\n" --> return "OK"
         */
        return serializedData.Substring(1, serializedData.Length - 1 - Delim.Length);
    }

    private static RespBoolean DeserializeBoolean(string? serializedData)
    {
        if (serializedData == null || serializedData.Length < 3)
            throw new SerializationException("Cannot deserialize to Boolean");
        var trueOrFalse = SerializedBodyInPrimitiveType(serializedData);
        return trueOrFalse switch
        {
            "t" => new RespBoolean(true),
            "f" => new RespBoolean(false),
            _ => throw new SerializationException("Cannot deserialize to Boolean")
        };
    }

    private static RespBulkString DeserializeBulkString(string? serializedData)
    {
        if (serializedData == null || serializedData.Length < 4)
            throw new SerializationException("Cannot deserialize to BulkString");
        var body = serializedData[(serializedData.IndexOf(Delim, StringComparison.Ordinal) + Delim.Length)..^Delim.Length];
        return new RespBulkString(body);
    }

    private static RespDouble DeserializeDouble(string? serializedData)
    {
        if (serializedData == null || serializedData.Length < 3)
            throw new SerializationException("Cannot deserialized to Double");
        var data = SerializedBodyInPrimitiveType(serializedData);
        return new RespDouble(double.Parse(data, CultureInfo.InvariantCulture));
    }

    private static RespInteger DeserializeInteger(string? serializedData)
    {
        if (serializedData == null || serializedData.Length < 3)
            throw new SerializationException("Cannot deserialized to Integer");
        var data = SerializedBodyInPrimitiveType(serializedData);
        return new RespInteger(int.Parse(data));
    }

    private static RespSimpleString DeserializeSimpleString(string? serializedData)
    {
        if (serializedData == null || serializedData.Length < 3)
            throw new SerializationException("Cannot deserialized to SimpleString");
        var data = SerializedBodyInPrimitiveType(serializedData);
        return new RespSimpleString(data);
    }

    private static RespSimpleError DeserializeSimpleError(string? serializedData)
    {
        if (serializedData == null || serializedData.Length < 3)
            throw new SerializationException("Cannot deserialized to SimpleError");
        var data = SerializedBodyInPrimitiveType(serializedData);
        return new RespSimpleError(data);
    }

    public static bool IsIndexAtPrimitiveTypeDiscriminator(char c)
    {
        return c == RespSimpleString.TypeDiscriminator || c == RespSimpleError.TypeDiscriminator ||
               c == RespBulkString.TypeDiscriminator || c == RespInteger.TypeDiscriminator ||
               c == RespDouble.TypeDiscriminator || c == RespBoolean.TypeDiscriminator;
    }

    public static bool IsIndexAtNonPrimitiveTypeDiscriminator(char c)
    {
        return c == RespArray.TypeDiscriminator || c == RespSet.TypeDiscriminator ||
               c == RespMap.TypeDiscriminator;
    }
    
    public static RespType DeserializePrimitive(string? data)
    {
        if (data == null || data.Length < 3)
            throw new SerializationException("Cannot deserialized data");
        var typeDiscriminator = data[0];
        return typeDiscriminator switch
        {
            RespSimpleString.TypeDiscriminator => DeserializeSimpleString(data),
            RespSimpleError.TypeDiscriminator => DeserializeSimpleError(data),
            RespBulkString.TypeDiscriminator => DeserializeBulkString(data),
            RespInteger.TypeDiscriminator => DeserializeInteger(data),
            RespDouble.TypeDiscriminator => DeserializeDouble(data),
            RespBoolean.TypeDiscriminator => DeserializeBoolean(data),
            _ => throw new SerializationException("Cannot deserialized data")
        };
    }

    public static void AddToPendingNonPrimitive(RespType obj, Stack<RespType> stack)
    {
        var topOfStack = stack.Peek();
        switch (topOfStack)
        {
            case RespArray array:
                array.Append(obj);
                break;
            case RespSet set:
                set.Add(obj);
                break;
            case RespMap:
                var kv = new RespKvPair
                {
                    Key = obj
                };
                stack.Push(kv);
                break;
            case RespKvPair kvPair:
            {
                kvPair.Value = obj;
                stack.Pop();
                var map = stack.Peek();
                if (map is RespMap respMap) respMap.Data[kvPair.Key] = kvPair.Value;
                break;
            }
        }
    }

    private static RespType CreateEmptyNonPrimitiveFromHead(string head)
    {
        var typeDiscriminator = head[0];
        
        switch (typeDiscriminator)
        {
            case RespArray.TypeDiscriminator:
            {
                var length = GetLengthParamFromHead(head);
                return new RespArray(new List<RespType>(length));
            }
            case RespSet.TypeDiscriminator:
            {
                var length = GetLengthParamFromHead(head);
                return new RespSet(new HashSet<RespType>(length));
            }
            case RespMap.TypeDiscriminator:
            {
                var length = GetLengthParamFromHead(head);
                return new RespMap(new Dictionary<RespType, RespType>(length));
            }
            default:
                throw new SerializationException("Cannot deserialized data");
        }
    }

    private static int IndexAtNextDelim(int ptr, string data)
    {
        return ptr + data[ptr..].IndexOf(Delim, StringComparison.Ordinal) + Delim.Length - 1;
    }

    private static int IndexAtEndOfPrimitive(int ptr, string data)
    {
        // ptr must be index at start of a primitive (e.g. SimpleString, SimpleError, BulkString, Integer, Double, Boolean)
        // e.g. ptr=0, data="+OK\r\n" --> return 4
        // e.g. ptr=0, data="$3\r\nfoo\r\n" --> return 8
        var typeDiscriminator = data[ptr];
        if (!IsIndexAtPrimitiveTypeDiscriminator(typeDiscriminator))
            throw new ArgumentException("ptr must be index at start of a primitive");
        var nextDelimIndex = IndexAtNextDelim(ptr, data);
        return typeDiscriminator != RespBulkString.TypeDiscriminator ? nextDelimIndex : IndexAtNextDelim(nextDelimIndex, data);
    }
    
    public static int IndexAtEndOfHead(int ptr, string data)
    {
        // ptr must be index at start of a non-primitive (e.g. Array, Set, Map)
        // returns the index at the end of the head of a non primitive type (e.g. "*3\r\n..." --> return 3)
        var typeDiscriminator = data[ptr];
        if (!IsIndexAtNonPrimitiveTypeDiscriminator(typeDiscriminator))
            throw new ArgumentException("ptr must be index at start of a non-primitive");
        var nextDelimIndex = ptr + data[ptr..].IndexOf(Delim, StringComparison.Ordinal) + Delim.Length - 1;
        return nextDelimIndex;
    }

    public static int GetLengthParamFromHead(string head)
    {
        /*
         * head is the string that contains the type discriminator and the length parameter,
         * e.g. head = "*3\r\n" --> return 3
         */
        var length = int.Parse(head.Substring(1, head.Length - 1 - Delim.Length));
        return length;
    }


    public static RespType Deserialize(string? data)
    {
        if (data == null || data.Length < 3)
            throw new SerializationException("Cannot deserialized data");
        var ptr1 = 0;
        var stack = new Stack<RespType>();
        while (ptr1 < data.Length)
        {
            if (IsIndexAtPrimitiveTypeDiscriminator(data[ptr1]))
            {
                var ptr2 = IndexAtEndOfPrimitive(ptr1, data);
                var obj = DeserializePrimitive(data[ptr1..(ptr2 + 1)]);
                if (stack.Count > 0) AddToPendingNonPrimitive(obj, stack);
                else return obj;
                ptr1 = ptr2 + 1;
            }
            else if (IsIndexAtNonPrimitiveTypeDiscriminator(data[ptr1]))
            {
                var ptr2 = IndexAtEndOfHead(ptr1, data);
                var head = data[ptr1..(ptr2 + 1)];
                var nonPrimitive = CreateEmptyNonPrimitiveFromHead(head);
                stack.Push(nonPrimitive);
                ptr1 = ptr2 + 1;
            }
            else
            {
                throw new SerializationException("Cannot deserialized data");
            }
        }

        if (stack.Any())
        {
            return stack.Peek();
        }
        throw new SerializationException("Cannot deserialized data");
    }
}
