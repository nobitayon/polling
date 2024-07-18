using System.Text.Json;

namespace Delta.Polling.Both.Common.Statics;

public static class JsonSerializerOptionsFor
{
    public static readonly JsonSerializerOptions Deserialize = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static readonly JsonSerializerOptions Serialize = new()
    {
        WriteIndented = true
    };
}
