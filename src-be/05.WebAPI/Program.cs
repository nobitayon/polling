using Delta.Polling.Infrastructure.Database;
using Delta.Polling.Infrastructure.Email;
using Delta.Polling.Infrastructure.Logging;
using Delta.Polling.Infrastructure.UserRole;
using Delta.Polling.Logics;
using Delta.Polling.WebAPI.Filters;
using Delta.Polling.WebAPI.Infrastructure.Authentication;
using Delta.Polling.WebAPI.Infrastructure.Authorization;
using Delta.Polling.WebAPI.Infrastructure.CurrentUser;
using Delta.Polling.WebAPI.Infrastructure.Documentation;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseLoggingService();
builder.Services.AddDatabaseService(builder.Configuration);
builder.Services.AddCurrentUserService();
builder.Services.AddDocumentationService(builder.Configuration);
builder.Services.AddEmailService(builder.Configuration);
builder.Services.AddUserRoleService(builder.Configuration);
builder.Services.AddLogics();

builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthenticationService(builder.Configuration);
builder.Services.AddAuthorizationService();
builder.Services.AddControllers(options =>
{
    _ = options.Filters.Add<CustomExceptionFilterAttribute>();
});

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();
await app.InitializeDatabaseAsync();

app.UseDocumentationService();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers().RequireAuthorization();
app.Run();
