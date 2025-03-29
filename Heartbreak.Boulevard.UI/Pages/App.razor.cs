using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using SuneDoes.UI.Configuration;

namespace SuneDoes.UI.Components;

public partial class App
{

    [Inject]
    public IOptions<SuneDoesConfiguration> Configuration { get; set; }

    private string AppBase => string.IsNullOrWhiteSpace(Configuration.Value.HostingBasePath) ? "/" : $"/{Configuration.Value.HostingBasePath}/";

}
