using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Heartbreak.Boulevard.UI.Configuration;

namespace Heartbreak.Boulevard.UI.Pages;

public partial class App
{

    [Inject]
    public IOptions<HBBConfiguration> Configuration { get; set; }

    private string AppBase => string.IsNullOrWhiteSpace(Configuration.Value.HostingBasePath) ? "/" : $"/{Configuration.Value.HostingBasePath}/";

}
