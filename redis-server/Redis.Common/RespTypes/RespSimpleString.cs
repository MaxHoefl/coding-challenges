using System.Runtime.Serialization;

namespace Redis.Common.RespTypes;

public class RespSimpleString : RespType
{
    public const char TypeDiscriminator = '+';
    public string Data { get; }

    public RespSimpleString(string data)
    {
        Data = data;
    }

    public override string Serialize()
    {
        return $"{TypeDiscriminator}{Data}{RespConstants.MessageDelimiter}";
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        return Data == ((RespSimpleString)obj).Data;
    }

    public override int GetHashCode()
    {
        return Data.GetHashCode();
    }
}