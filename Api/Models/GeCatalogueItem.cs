using System.Text.Json.Serialization;

namespace Api.Models;

/// <summary>Single item from GE catalogue items API.</summary>
public sealed class GeCatalogueItem
{
    public string? Icon { get; set; }

    [JsonPropertyName("icon_large")]
    public string? IconLarge { get; set; }
    public int Id { get; set; }
    public string? Type { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public GePricePoint? Current { get; set; }
    public GePricePoint? Today { get; set; }
    public bool Members { get; set; }
}

/// <summary>Price and trend for a time window.</summary>
public sealed class GePricePoint
{
    public string? Trend { get; set; }
    public string? Price { get; set; }
}
