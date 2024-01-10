using System.Text;

namespace Redis.Common.RespTypes;

public class RespMap : RespType
{
    public const char TypeDiscriminator = '%';
    public Dictionary<RespType, RespType> Data { get; }

    public RespMap(Dictionary<RespType, RespType> data)
    {
        Data = data;
    }
    public override string Serialize()
    {
        var sb = new StringBuilder();
        sb.Append($"{TypeDiscriminator}{Data.Count}{RespConstants.MessageDelimiter}");
        foreach (var (key, value) in Data)
        {
            sb.Append($"{key.Serialize()}{value.Serialize()}");
        }
        sb.Append(RespConstants.MessageDelimiter);
        return sb.ToString();
    }
}