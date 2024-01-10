namespace Redis.Common.RespTypes;

public class RespInteger : RespType
{
    public const char TypeDiscriminator = ':';

    public int Data { get; }

    public RespInteger(int data)
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
        return Data == ((RespInteger)obj).Data;
    }

    public override int GetHashCode()
    {
        return Data.GetHashCode();
    }
}