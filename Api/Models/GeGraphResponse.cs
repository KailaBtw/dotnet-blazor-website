namespace Api.Models;

/// <summary>Response from GE graph API - daily and average prices by timestamp (ms since epoch).</summary>
public sealed class GeGraphResponse
{
    public Dictionary<string, long> Daily { get; set; } = new();
    public Dictionary<string, long> Average { get; set; } = new();
}
