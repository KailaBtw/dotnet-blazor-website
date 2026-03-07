namespace Api.Services;

/// <summary>Configuration for the GE API client (polite defaults, retry, delay).</summary>
public sealed class GeApiOptions
{
    public const string SectionName = "GeApi";

    /// <summary>Base URL for GE API, e.g. https://secure.runescape.com/m=itemdb_rs or m=itemdb_oldschool for OSRS.</summary>
    public string BaseUrl { get; set; } = "https://secure.runescape.com/m=itemdb_rs";

    /// <summary>User-Agent sent with requests (polite identification).</summary>
    public string UserAgent { get; set; } = "DotNetBlazorWebsite/1.0 (local GE price tracking; contact via repo)";

    /// <summary>Delay between requests in milliseconds.</summary>
    public int RequestDelayMs { get; set; } = 100;

    public int MaxRetries { get; set; } = 3;
    public int TimeoutSeconds { get; set; } = 20;
}
