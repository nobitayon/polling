using System.Text;

namespace Delta.Polling.Both.Common.Exceptions;

public class ModelValidationException(IEnumerable<string> errorMessages)
    : Exception("One or more validation failures have occurred.")
{
    public IEnumerable<string> ErrorMessages { get; } = errorMessages;

    public string Summary
    {
        get
        {
            var summary = new StringBuilder();

            foreach (var errorMessage in ErrorMessages)
            {
                _ = summary.AppendLine(errorMessage);
            }

            return summary.ToString();
        }
    }

    public override string ToString()
    {
        return Summary;
    }
}
