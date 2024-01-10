namespace Redis.Common.RespTypes.Derived;

public class RespKvPair: RespType
{
    public RespType Key { get; set; } = default!;
    public RespType Value { get; set; } = default!;
    public override string Serialize()
    {
        throw new NotImplementedException();
    }
}