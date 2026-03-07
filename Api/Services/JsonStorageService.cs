using System.Text.Json;
using Microsoft.Extensions.Options;

namespace Api.Services;

/// <summary>Stores and loads JSON under a configurable base path (e.g. Data/).</summary>
public sealed class JsonStorageService : IJsonStorageService
{
    private readonly string _basePath;
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };

    public JsonStorageService(IOptions<StorageOptions> options)
    {
        var path = options.Value.BasePath ?? "Data";
        _basePath = Path.GetFullPath(Path.IsPathRooted(path) ? path : Path.Combine(AppContext.BaseDirectory, path));
    }

    private string GetFullPath(string relativePath)
    {
        var combined = Path.Combine(_basePath, relativePath);
        var full = Path.GetFullPath(combined);
        var baseNorm = _basePath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        if (!full.StartsWith(baseNorm + Path.DirectorySeparatorChar) && full != baseNorm)
            throw new ArgumentException("Path must stay under base path.", nameof(relativePath));
        return full;
    }

    public async Task SaveJsonAsync<T>(string relativePath, T data, CancellationToken ct = default)
    {
        var full = GetFullPath(relativePath);
        var dir = Path.GetDirectoryName(full);
        if (!string.IsNullOrEmpty(dir))
            Directory.CreateDirectory(dir);
        await File.WriteAllTextAsync(full, JsonSerializer.Serialize(data, Options), ct);
    }

    public async Task<T?> LoadJsonAsync<T>(string relativePath, CancellationToken ct = default) where T : class
    {
        var full = GetFullPath(relativePath);
        if (!File.Exists(full))
            return null;
        var json = await File.ReadAllTextAsync(full, ct);
        if (string.IsNullOrWhiteSpace(json))
            return null;
        return JsonSerializer.Deserialize<T>(json, Options);
    }

    public async Task SaveJsonLinesAsync<T>(string relativePath, IEnumerable<T> records, CancellationToken ct = default)
    {
        var full = GetFullPath(relativePath);
        var dir = Path.GetDirectoryName(full);
        if (!string.IsNullOrEmpty(dir))
            Directory.CreateDirectory(dir);
        await using var writer = new StreamWriter(full, append: false, System.Text.Encoding.UTF8);
        foreach (var rec in records)
        {
            var line = JsonSerializer.Serialize(rec, Options) + Environment.NewLine;
            await writer.WriteAsync(line.AsMemory(), ct);
        }
    }
}

/// <summary>Configuration for JSON storage base path.</summary>
public sealed class StorageOptions
{
    public const string SectionName = "Storage";
    public string? BasePath { get; set; }
}
