namespace Delta.Polling.Both.Common.Exceptions;

public class MismatchException(string propertyName, object valueInRoute, object valueInForm)
    : Exception($"{propertyName} value in the Route [{valueInRoute}] does not match {propertyName} value in the Form [{valueInForm}]")
{
    public string PropertyName { get; set; } = propertyName;
    public object ValueInRoute { get; set; } = valueInRoute;
    public object ValueInForm { get; set; } = valueInForm;
}
