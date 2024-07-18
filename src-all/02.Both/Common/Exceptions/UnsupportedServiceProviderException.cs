namespace Delta.Polling.Both.Common.Exceptions;

public class UnsupportedServiceProviderException(
    string serviceName,
    string providerName)
    : Exception($"Unsupported {serviceName} Service Provider: {providerName}.")
{
}
