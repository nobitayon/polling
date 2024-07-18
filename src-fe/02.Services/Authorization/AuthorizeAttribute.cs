namespace Delta.Polling.FrontEnd.Services.Authorization;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class AuthorizeAttribute : Attribute
{
    public string? RoleName { get; set; }
}
