using System.Text;

namespace Redis.Common.RespTypes;

public class RespSet : RespType
{
    public const char TypeDiscriminator = '~';
    private HashSet<RespType> _data;
    public HashSet<RespType> Data => _data;

    public RespSet(HashSet<RespType> data)
    {
        _data = data;
    }
    
    public override string Serialize()
    {
        var result = new StringBuilder();
        result.Append($"{TypeDiscriminator}{_data.Count}{RespConstants.MessageDelimiter}");
        foreach (var respType in _data)
        {
            result.Append(respType.Serialize());
        }

        result.Append(RespConstants.MessageDelimiter);
        return result.ToString();
    }
    
    public void Add(RespType element)
    {
        _data.Add(element);
    }
}