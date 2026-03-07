namespace Api.Models;

/// <summary>Response from GE catalogue detail.json API.</summary>
public sealed class GeDetailResponse
{
    public GeItemDetail? Item { get; set; }
}
