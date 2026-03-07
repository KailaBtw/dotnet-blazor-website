using System.Text.Json.Serialization;

namespace Api.Models;

/// <summary>Full item detail from GE detail.json API.</summary>
public sealed class GeItemDetail
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
    public GePriceChange? Day30 { get; set; }
    public GePriceChange? Day90 { get; set; }
    public GePriceChange? Day180 { get; set; }
}

/// <summary>Price change over a period (e.g. day30).</summary>
public sealed class GePriceChange
{
    public string? Trend { get; set; }
    public string? Change { get; set; }
}
