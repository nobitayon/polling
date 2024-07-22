using Delta.Polling.FrontEnd.Infrastructure.BackEnd;
using Delta.Polling.FrontEnd.Infrastructure.Logging;
using Delta.Polling.FrontEnd.Logics;
using Delta.Polling.WebRP.Infrastructure.Authentication;
using Delta.Polling.WebRP.Infrastructure.Authorization;
using Delta.Polling.WebRP.Infrastructure.CurrentUser;
using Delta.Polling.WebRP.Infrastructure.UserProfile;
using Delta.Polling.WebRP.Infrastructure.UserRole;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddBackEndService(builder.Configuration);
builder.Host.UseLoggingService();
builder.Services.AddLogics();
builder.Services.AddAuthenticationService(builder.Configuration);
builder.Services.AddAuthorizationService();
builder.Services.AddUserProfileService(builder.Configuration);
builder.Services.AddUserRoleService(builder.Configuration);
builder.Services.AddCurrentUserService();
builder.Services.AddHttpContextAccessor();
builder.Services.AddRazorPages();
builder.Services.AddPager();

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
