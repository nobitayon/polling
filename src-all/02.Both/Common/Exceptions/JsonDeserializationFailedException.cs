namespace Delta.Polling.Both.Common.Exceptions;

public class JsonDeserializationFailedException(string jsonContent, Type targetType)
    : Exception($"Failed to deserialize JSON content {jsonContent} to instance of type '{targetType.Name}'.")
{
}
