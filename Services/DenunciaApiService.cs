using System.Net.Http.Json;
using System.Text.Json;
using LeyKarinApp.Models;

namespace LeyKarinApp.Services;

public class DenunciaApiService
{
    private readonly HttpClient _http;
    private readonly ConfiguracionService _config;
    private const string CacheTiposKey = "cache_tipos";

    public DenunciaApiService(HttpClient http, ConfiguracionService config)
    {
        _http = http;
        _config = config;
    }

    private string BaseUrl => _config.ServidorUrl.TrimEnd('/');

    public async Task<EmpresaInfoDto?> ObtenerInfoEmpresaAsync(string slug)
    {
        try
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            return await _http.GetFromJsonAsync<EmpresaInfoDto>(
                $"{BaseUrl}/api/portal/{slug}/info", cts.Token);
        }
        catch { return null; }
    }

    /// <summary>Carga tipos desde el servidor. Si falla, devuelve el caché local.</summary>
    public async Task<List<TipoDenunciaDto>> ObtenerTiposAsync(string slug)
    {
        try
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            var tipos = await _http.GetFromJsonAsync<List<TipoDenunciaDto>>(
                $"{BaseUrl}/api/portal/{slug}/tipos", cts.Token);

            if (tipos != null && tipos.Count > 0)
            {
                // Guardar en caché local
                try { Preferences.Set(CacheTiposKey, JsonSerializer.Serialize(tipos)); }
                catch { /* ignore */ }
                return tipos;
            }
        }
        catch { /* continuar con caché */ }

        // Sin conexión: usar caché
        return CargarTiposDesdeCache();
    }

    public List<TipoDenunciaDto> CargarTiposDesdeCache()
    {
        try
        {
            var json = Preferences.Get(CacheTiposKey, string.Empty);
            if (!string.IsNullOrEmpty(json))
                return JsonSerializer.Deserialize<List<TipoDenunciaDto>>(json) ?? [];
        }
        catch { /* ignore */ }

        // Tipos por defecto (fallback sin red ni caché)
        return
        [
            new(Guid.Empty, "Acoso Laboral", "Conductas que atentan contra la dignidad laboral", null),
            new(Guid.Empty, "Acoso Sexual", "Conductas de connotación sexual no deseadas", null),
            new(Guid.Empty, "Violencia Laboral", "Maltrato físico o psicológico en el trabajo", null),
            new(Guid.Empty, "Discriminación", "Trato diferenciado injustificado", null),
        ];
    }

    public async Task<DenunciaResponse> EnviarDenunciaAsync(string slug, DenunciaRequest request)
    {
        try
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            var response = await _http.PostAsJsonAsync(
                $"{BaseUrl}/api/portal/{slug}/denuncia", request, cts.Token);

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<DenunciaResponse>(cts.Token)
                    ?? new DenunciaResponse(string.Empty, DateTime.UtcNow, false, "Respuesta inesperada");

            var error = await response.Content.ReadAsStringAsync();
            return new DenunciaResponse(string.Empty, DateTime.UtcNow, false, error);
        }
        catch (TaskCanceledException)
        {
            return new DenunciaResponse(string.Empty, DateTime.UtcNow, false,
                "La operación tardó demasiado. Verifica tu conexión e intenta nuevamente.");
        }
        catch (HttpRequestException)
        {
            return new DenunciaResponse(string.Empty, DateTime.UtcNow, false,
                "Sin conexión al servidor. Verifica tu red e intenta nuevamente.");
        }
        catch (Exception ex)
        {
            return new DenunciaResponse(string.Empty, DateTime.UtcNow, false, ex.Message);
        }
    }
}
