using Delta.Polling.Infrastructure;
using Delta.Polling.Infrastructure.Database;
using Delta.Polling.Infrastructure.Documentation;

//using Delta.Polling.Infrastructure.Storage.LocalFolder;
using Delta.Polling.Logics;
using Delta.Polling.Logics.SignalR;
using Delta.Polling.WebAPI;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddInfrastructure(builder.Host, builder.Configuration);
builder.Services.AddLogics();
builder.Services.AddWebAPI();
builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            _ = builder.WithOrigins("https://localhost:44322")
                .AllowAnyHeader()
                .WithMethods("GET", "POST")
                .AllowCredentials();
        });
});

var app = builder.Build();
await app.InitializeDatabaseAsync();
app.UseDocumentation();
app.UseHttpsRedirection();
//app.UseLocalFolderStorage(app.Configuration);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers().RequireAuthorization();
app.UseCors();
app.MapHub<PollHub>("api/poll-hub");
app.Run();
