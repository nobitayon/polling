namespace Delta.Polling.Both.Common.Exceptions;

public class EntityNotFoundException(string entityType, Guid id)
    : Exception($"Entity {entityType} with ID {id} could not be found.")
{
}
