using System.Text.Json;
using Delta.Polling.Both.Common.Statics;

namespace Delta.Polling.Both.Common.Extensions;

public static class GenericExtensions
{
    public static string ToPrettyJson<T>(this T data)
    {
        return JsonSerializer.Serialize(data, JsonSerializerOptionsFor.Serialize);
    }
}