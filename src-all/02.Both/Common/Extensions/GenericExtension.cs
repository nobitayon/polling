using System.Text.Json;

namespace Delta.Polling.Both.Common.Extensions;

public static class GenericExtensions
{
    public static string ToPrettyJson<T>(this T data)
    {
        return JsonSerializer.Serialize(data, JsonSerializerOptionsFor.Serialize);
    }
}
