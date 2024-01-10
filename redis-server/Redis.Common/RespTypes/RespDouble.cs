using System.Globalization;

namespace Redis.Common.RespTypes;

public class RespDouble : RespType
{
    public const char TypeDiscriminator = ',';
    private const double Epsilon = 1e-6;
    public double Data { get; }

    public RespDouble(double data)
    {
        Data = data;
    }
    
    public override string Serialize()
    {
        return $"{TypeDiscriminator}{Data.ToString(CultureInfo.InvariantCulture)}{RespConstants.MessageDelimiter}";
    }
    
    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        return Math.Abs(Data - ((RespDouble)obj).Data) < Epsilon;
    }

    public override int GetHashCode()
    {
        return Data.GetHashCode();
    }
}