using System.Text;
using System.Text.Json;

namespace BlogSeoAi.Services;

public interface IAiClient
{
    Task<string> GenerateAsync(string prompt, CancellationToken ct = default);
}

public class GeminiAiClient : IAiClient
{
    private readonly IHttpClientFactory _http;
    private readonly IConfiguration _cfg;

    public GeminiAiClient(IHttpClientFactory http, IConfiguration cfg)
    {
        _http = http;
        _cfg = cfg;
    }

    public async Task<string> GenerateAsync(string prompt, CancellationToken ct = default)
    {
        var key = _cfg["Ai:ApiKey"] ?? throw new InvalidOperationException("Ai:ApiKey missing");
        var model = _cfg["Ai:Model"] ?? "gemini-2.5-flash";
        var endpointTpl = _cfg["Ai:Endpoint"]
            ?? "https://generativelanguage.googleapis.com/v1/models/{model}:generateContent?key={key}";

        var url = endpointTpl.Replace("{model}", model).Replace("{key}", key);

        var payload = new
        {
            contents = new[]
            {
                new {
                    parts = new[] { new { text = prompt } }
                }
            }
        };

        var json = JsonSerializer.Serialize(payload);
        var client = _http.CreateClient();

        using var res = await client.PostAsync(url,
            new StringContent(json, Encoding.UTF8, "application/json"), ct);

        var body = await res.Content.ReadAsStringAsync(ct);
        res.EnsureSuccessStatusCode();

        using var doc = JsonDocument.Parse(body);

        // candidates[0].content.parts[0].text
        var text = doc.RootElement
            .GetProperty("candidates")[0]
            .GetProperty("content")
            .GetProperty("parts")[0]
            .GetProperty("text")
            .GetString();

        return text ?? "";
    }
}