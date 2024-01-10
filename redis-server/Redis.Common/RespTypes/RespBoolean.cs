namespace Redis.Common.RespTypes;

public class RespBoolean : RespType
{
    public const char TypeDiscriminator = '#';
    public bool Data { get; }

    public RespBoolean(bool data)
    {
        Data = data;
    }
    
    public override string Serialize()
    {
        var trueOrFalse = Data ? "t" : "f";
        return $"{TypeDiscriminator}{trueOrFalse}{RespConstants.MessageDelimiter}";
    }
    
    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        return Data == ((RespBoolean)obj).Data;
    }

    public override int GetHashCode()
    {
        return Data.GetHashCode();
    }
}