namespace Delta.Polling.Both.Common.Exceptions;

public class ConfigurationBindingFailedException(
    string configurationSection,
    Type targetType)
    : Exception($"Failed to bind configuration section {configurationSection} to instance of type {targetType.Name}.")
{
}
