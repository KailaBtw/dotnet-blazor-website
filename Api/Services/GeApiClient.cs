using System.Net.Http.Json;
using System.Text.Json;
using Api.Models;
using Microsoft.Extensions.Options;

namespace Api.Services;

/// <summary>
/// Client for RuneScape Grand Exchange API with retry, User-Agent, and request delay
/// (repurposed from wiki_api.py MediaWikiClient pattern).
/// </summary>
public sealed class GeApiClient
{
    private readonly HttpClient _http;
    private readonly GeApiOptions _options;
    private readonly ILogger<GeApiClient> _logger;
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public GeApiClient(HttpClient http, IOptions<GeApiOptions> options, ILogger<GeApiClient> logger)
    {
        _http = http;
        _options = options.Value;
        _logger = logger;
        _http.BaseAddress = new Uri(_options.BaseUrl.TrimEnd('/') + "/");
        _http.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", _options.UserAgent);
        _http.Timeout = TimeSpan.FromSeconds(_options.TimeoutSeconds);
    }

    /// <summary>GET JSON with retry and exponential backoff; on 429 uses longer delay.</summary>
    private async Task<T> RequestAsync<T>(string requestUri, CancellationToken ct = default) where T : class
    {
        Exception? lastEx = null;
        for (var attempt = 1; attempt <= _options.MaxRetries; attempt++)
        {
            try
            {
                await Task.Delay(TimeSpan.FromMilliseconds(_options.RequestDelayMs), ct);
                var response = await _http.GetAsync(requestUri, ct);
                if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                {
                    var sleepSec = 30 * attempt;
                    _logger.LogWarning("Rate limited (429), waiting {Sleep}s before retry {Attempt}", sleepSec, attempt);
                    await Task.Delay(TimeSpan.FromSeconds(sleepSec), ct);
                    continue;
                }
                response.EnsureSuccessStatusCode();
                var data = await response.Content.ReadFromJsonAsync<T>(JsonOptions, ct);
                if (data is null)
                    throw new InvalidOperationException("Response deserialized to null.");
                return data;
            }
            catch (Exception ex)
            {
                lastEx = ex;
                if (attempt == _options.MaxRetries) break;
                var delayMs = Math.Max(_options.RequestDelayMs, 100) * (1 << (attempt - 1));
                _logger.LogDebug(ex, "Attempt {Attempt} failed, retrying in {Delay}ms", attempt, delayMs);
                await Task.Delay(TimeSpan.FromMilliseconds(delayMs), ct);
            }
        }
        throw lastEx ?? new InvalidOperationException("Request failed after retries.");
    }

    /// <summary>Catalogue items: category, alpha (first letter), page (1-based).</summary>
    public Task<GeCatalogueResponse?> GetCatalogueItemsAsync(int category, string alpha, int page, CancellationToken ct = default)
    {
        var alphaParam = alpha == "#" ? "%23" : Uri.EscapeDataString(alpha.ToLowerInvariant());
        var uri = $"api/catalogue/items.json?category={category}&alpha={alphaParam}&page={page}";
        return RequestAsync<GeCatalogueResponse>(uri, ct)!;
    }

    /// <summary>Single item detail by item ID.</summary>
    public Task<GeDetailResponse?> GetItemDetailAsync(int itemId, CancellationToken ct = default)
    {
        var uri = $"api/catalogue/detail.json?item={itemId}";
        return RequestAsync<GeDetailResponse>(uri, ct)!;
    }

    /// <summary>180-day price graph by item ID.</summary>
    public Task<GeGraphResponse?> GetItemGraphAsync(int itemId, CancellationToken ct = default)
    {
        var uri = $"api/graph/{itemId}.json";
        return RequestAsync<GeGraphResponse>(uri, ct)!;
    }

    /// <summary>Last runedate when GE data was updated.</summary>
    public async Task<JsonElement?> GetInfoAsync(CancellationToken ct = default)
    {
        var response = await _http.GetAsync("api/info.json", ct);
        response.EnsureSuccessStatusCode();
        var doc = await response.Content.ReadFromJsonAsync<JsonElement>(ct);
        return doc;
    }
}
