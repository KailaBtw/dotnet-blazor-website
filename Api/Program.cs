using Api.Models;
using Api.Services;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<GeApiOptions>(builder.Configuration.GetSection(GeApiOptions.SectionName));
builder.Services.Configure<StorageOptions>(builder.Configuration.GetSection(StorageOptions.SectionName));
builder.Services.AddHttpClient<GeApiClient>();
builder.Services.AddSingleton<IJsonStorageService, JsonStorageService>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.SetIsOriginAllowed(origin =>
            {
                if (string.IsNullOrEmpty(origin)) return false;
                var uri = new Uri(origin);
                return uri.Host is "localhost" or "127.0.0.1";
            })
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors();
app.UseHttpsRedirection();

// GE API proxy with optional JSON cache
app.MapGet("/api/ge/items", async (
    int category,
    string alpha,
    int page,
    string? game,
    GeApiClient ge,
    IJsonStorageService storage,
    CancellationToken ct) =>
{
    var cacheKey = $"ge/items_{game ?? "rs3"}_{category}_{alpha}_{page}.json";
    var cached = await storage.LoadJsonAsync<GeCatalogueResponse>(cacheKey, ct);
    if (cached is not null)
        return Results.Ok(cached);

    var response = await ge.GetCatalogueItemsAsync(category, alpha, page, ct);
    if (response is null)
        return Results.NotFound();
    await storage.SaveJsonAsync(cacheKey, response, ct);
    return Results.Ok(response);
}).Produces<GeCatalogueResponse>();

app.MapGet("/api/ge/items/{id:int}", async (
    int id,
    GeApiClient ge,
    IJsonStorageService storage,
    CancellationToken ct) =>
{
    var cacheKey = $"ge/detail_{id}.json";
    var cached = await storage.LoadJsonAsync<GeDetailResponse>(cacheKey, ct);
    if (cached is not null)
        return Results.Ok(cached);

    var response = await ge.GetItemDetailAsync(id, ct);
    if (response is null)
        return Results.NotFound();
    await storage.SaveJsonAsync(cacheKey, response, ct);
    return Results.Ok(response);
}).Produces<GeDetailResponse>();

app.MapGet("/api/ge/items/{id:int}/graph", async (
    int id,
    GeApiClient ge,
    IJsonStorageService storage,
    CancellationToken ct) =>
{
    var cacheKey = $"ge/graph_{id}.json";
    var cached = await storage.LoadJsonAsync<GeGraphResponse>(cacheKey, ct);
    if (cached is not null)
        return Results.Ok(cached);

    var response = await ge.GetItemGraphAsync(id, ct);
    if (response is null)
        return Results.NotFound();
    await storage.SaveJsonAsync(cacheKey, response, ct);
    return Results.Ok(response);
}).Produces<GeGraphResponse>();

app.Run();
