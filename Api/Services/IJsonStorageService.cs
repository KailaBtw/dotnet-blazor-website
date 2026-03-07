namespace Api.Services;

/// <summary>File-based JSON storage (repurposed from wiki_api.py write_json / write_jsonl).</summary>
public interface IJsonStorageService
{
    /// <summary>Serialize and save to a file under the configured base path. Creates directory if needed.</summary>
    Task SaveJsonAsync<T>(string relativePath, T data, CancellationToken ct = default);

    /// <summary>Load and deserialize from a file; returns null if file does not exist or is empty.</summary>
    Task<T?> LoadJsonAsync<T>(string relativePath, CancellationToken ct = default) where T : class;

    /// <summary>Append records as one JSON object per line (JSONL).</summary>
    Task SaveJsonLinesAsync<T>(string relativePath, IEnumerable<T> records, CancellationToken ct = default);
}
