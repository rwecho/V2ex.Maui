using V2ex.Api;
using V2ex.Blazor;
using V2ex.Blazor.Components;
using V2ex.Blazor.Services;
using Serilog;
using Serilog.Events;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;


Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .WriteTo.Console()
    .CreateLogger();

Log.Information("Starting up");
var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

builder.Services.AddSingleton<IAuthorizationService,AlwaysAllowAuthorizationService>();

builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
    .AddIdentityCookies();

builder.Services.AddAuthorizationCore();

builder.Services.AddOptions();

builder.Services.AddBlazorShared();

builder.Services.AddScoped<INavigationInterceptorService, NavigationInterceptorService>();
builder.Services.AddScoped<IPreferences, Preferences>();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Main).Assembly);

app.Run();
