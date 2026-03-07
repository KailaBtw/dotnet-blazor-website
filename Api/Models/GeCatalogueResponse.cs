namespace Api.Models;

/// <summary>Response from GE catalogue items.json API.</summary>
public sealed class GeCatalogueResponse
{
    public int Total { get; set; }
    public List<GeCatalogueItem> Items { get; set; } = new();
}
