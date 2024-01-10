using System.Collections;
using System.Text;

namespace Redis.Common.RespTypes;

public class RespArray : RespType
{
    public const char TypeDiscriminator = '*';
    private IEnumerable<RespType> _data;
    public IEnumerable<RespType> Data => _data;

    public RespArray(IEnumerable<RespType> data)
    {
        _data = data;
    }

    public override string Serialize()
    {
        var result = new StringBuilder();
        result.Append($"{TypeDiscriminator}{_data.ToArray().Length}{RespConstants.MessageDelimiter}");
        foreach (var respType in _data)
        {
            result.Append(respType.Serialize());
        }

        result.Append(RespConstants.MessageDelimiter);
        return result.ToString();
    }

    public void Append(RespType element)
    {
        _data = _data.Append(element);
    }
}