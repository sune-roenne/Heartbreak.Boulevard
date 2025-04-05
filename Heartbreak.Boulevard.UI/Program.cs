using Heartbreak.Boulevard.UI;
using Heartbreak.Boulevard.UI.Components;
using Heartbreak.Boulevard.UI.Configuration;
using Heartbreak.Boulevard.UI.Integration;
using Heartbreak.Boulevard.UI.Pages;



var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json", optional: false);
builder.Configuration.AddJsonFile("appsettings.local.json", optional: true);
builder.Configuration.AddEnvironmentVariables();
builder.Services.Configure<HBBConfiguration>(builder.Configuration.GetSection(HBBConfiguration.ConfigurationElementName));
var appConfig = new HBBConfiguration();
builder.Configuration.GetSection(HBBConfiguration.ConfigurationElementName).Bind(appConfig);
// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSingleton<IHBBGitHubClient, HBBGitHubClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

if (!string.IsNullOrEmpty(appConfig.HostingBasePath))
    app.MapBlazorHub("/" + appConfig.HostingBasePath)
    .WithOrder(-1);

app.Run();
