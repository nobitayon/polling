using Delta.Polling.Infrastructure;
using Delta.Polling.Infrastructure.Database;
using Delta.Polling.Infrastructure.Documentation;
using Delta.Polling.Logics;
using Delta.Polling.WebAPI;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddInfrastructure(builder.Host, builder.Configuration);
builder.Services.AddLogics();
builder.Services.AddWebAPI();

var app = builder.Build();
await app.InitializeDatabaseAsync();
app.UseDocumentation();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers().RequireAuthorization();
app.Run();
