namespace Redis.Common.RespTypes;

public class RespBulkString : RespType
{
    public const char TypeDiscriminator = '$';
    public string? Data { get; }
    public int Length => Data.Length;
    
    public RespBulkString(string? data)
    {
        Data = data;
    }
    
    public override string Serialize()
    {
        return $"{TypeDiscriminator}{Data.Length}{RespConstants.MessageDelimiter}" +
               $"{Data}{RespConstants.MessageDelimiter}";
    }
    
    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        return Data == ((RespBulkString)obj).Data;
    }

    public override int GetHashCode()
    {
        return Data.GetHashCode();
    }
}