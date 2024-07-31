namespace Delta.Polling.FrontEnd.Services.Authorization;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class AuthorizeAttribute : Attribute
{
    public string? RoleName { get; set; }
}
