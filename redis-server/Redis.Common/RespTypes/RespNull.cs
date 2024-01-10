namespace Redis.Common.RespTypes;

public class RespNull : RespType
{
    public const char TypeDiscriminator = '_';
    
    public override string Serialize()
    {
        return $"{TypeDiscriminator}{RespConstants.MessageDelimiter}";
    }
}