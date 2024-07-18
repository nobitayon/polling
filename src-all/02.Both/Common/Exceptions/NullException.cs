namespace Delta.Polling.Both.Common.Exceptions;

public class NullException(
    string objectName,
    Type objectType)
    : Exception($"The instance '{objectName}' of type '{objectType.Name}' is null.")
{
}
