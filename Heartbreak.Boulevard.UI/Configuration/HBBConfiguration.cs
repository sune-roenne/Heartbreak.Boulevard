namespace Heartbreak.Boulevard.UI.Configuration;

public class HBBConfiguration {
    public const string ConfigurationElementName = "Hbb";

    public string? HostingBasePath { get; set; }
    public string GitHubToken { get; set; }

    public string GitHubRepoName { get; set; }
    public string GitHubRepoOwner { get; set; }
    public string IndexFile { get; set; }

}