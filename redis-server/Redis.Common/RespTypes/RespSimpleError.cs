namespace Redis.Common.RespTypes;

public class RespSimpleError : RespType
{
    public const char TypeDiscriminator = '-';
    public string Data { get; }

    public RespSimpleError(string data)
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
        return Data == ((RespSimpleError)obj).Data;
    }

    public override int GetHashCode()
    {
        return Data.GetHashCode();
    }
}