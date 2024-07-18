namespace Delta.Polling.Both.Common.Exceptions;

public class GetAccessTokenFailedException(
    string tokenUrl,
    string? error,
    string? errorDescription)
    : Exception($"Cannot retrieve Access Token from {tokenUrl}. Error: {error}. Error description: {errorDescription}")
{
}
