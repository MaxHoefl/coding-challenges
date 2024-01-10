using Redis.Common;
using Redis.Common.RespTypes;

namespace Redis.Server;

public class CommandService
{
    private readonly Dictionary<object, object> _db = new();
    
    public RespType ProcessCommand(string serializedCommand)
    {
        var resp = RespDeserializer.Deserialize(serializedCommand);
        var respArray = resp as RespArray;
        if (respArray == null)
        {
            return new RespSimpleError("ERR: Command not recognized");
        }
        var command = respArray.Data.ToList()[0] as RespBulkString;
        if (command == null)
        {
            return new RespSimpleError("ERR: Command not recognized");
        }
        var commandName = command.Data;
        switch (commandName)
        {
            case "PING":
                return ProcessPingCommand(respArray);
            case "SET":
                return ProcessSetCommand(respArray);
            case "GET":
                return ProcessGetCommand(respArray);
            case "DEL":
                return ProcessDelCommand(respArray);
            case "KEYS":
                return ProcessKeysCommand(respArray);
            case "FLUSHDB":
                return ProcessFlushDbCommand(respArray);
            default:
                return new RespSimpleError("ERR: Command not recognized");
        }
    }

    private RespType ProcessFlushDbCommand(RespArray respArray)
    {
        throw new NotImplementedException();
    }

    private RespType ProcessKeysCommand(RespArray respArray)
    {
        throw new NotImplementedException();
    }

    private RespType ProcessDelCommand(RespArray respArray)
    {
        throw new NotImplementedException();
    }

    private RespType ProcessGetCommand(RespArray respArray)
    {
        if (respArray.Data.Count() != 2)
        {
            return new RespSimpleError("ERR: Wrong number of arguments for GET command");
        }
        var key = respArray.Data.ToList()[1] as RespBulkString;
        if (key == null)
        {
            return new RespSimpleError("ERR: Wrong type of arguments for GET command");
        }
        if (!_db.ContainsKey(key.Data))
        {
            return new RespBulkString(null);
        }
        return new RespBulkString(_db[key.Data].ToString() ?? string.Empty);
    }

    private RespType ProcessSetCommand(RespArray respArray)
    {
        if (respArray.Data.Count() != 3)
        {
            return new RespSimpleError("ERR: Wrong number of arguments for SET command");
        }
        var key = respArray.Data.ToList()[1] as RespBulkString;
        var value = respArray.Data.ToList()[2] as RespBulkString;
        if (key == null || value == null)
        {
            return new RespSimpleError("ERR: Wrong type of arguments for SET command");
        }
        _db[key.Data] = value.Data;
        return new RespSimpleString("OK");
        
    }

    private RespType ProcessPingCommand(RespArray respArray)
    {
        if (respArray.Data.Count() != 1)
        {
            return new RespSimpleError("ERR: Wrong number of arguments for PING command");
        }
        return new RespSimpleString("PONG");
    }
}