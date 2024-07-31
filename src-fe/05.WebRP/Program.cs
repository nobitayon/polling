using Delta.Polling.FrontEnd.Infrastructure;
using Delta.Polling.FrontEnd.Logics;
using Delta.Polling.WebRP;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddInfrastructure(builder.Host, builder.Configuration);
builder.Services.AddLogics();
builder.Services.AddWebRP(builder.Environment, builder.Configuration);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    _ = app.UseExceptionHandler("/Error");
    _ = app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.Run();
