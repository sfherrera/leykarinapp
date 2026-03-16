using System.Net.Http.Json;
using LeyKarinApp.Models;

namespace LeyKarinApp.Services;

public class DenunciaApiService(HttpClient http, ConfiguracionService config)
{
    private string BaseUrl => config.ServidorUrl.TrimEnd('/');

    public async Task<EmpresaInfoDto?> ObtenerInfoEmpresaAsync(string slug)
    {
        try
        {
            return await http.GetFromJsonAsync<EmpresaInfoDto>($"{BaseUrl}/api/portal/{slug}/info");
        }
        catch { return null; }
    }

    public async Task<List<TipoDenunciaDto>> ObtenerTiposAsync(string slug)
    {
        try
        {
            return await http.GetFromJsonAsync<List<TipoDenunciaDto>>($"{BaseUrl}/api/portal/{slug}/tipos") ?? [];
        }
        catch { return []; }
    }

    public async Task<DenunciaResponse> EnviarDenunciaAsync(string slug, DenunciaRequest request)
    {
        try
        {
            var response = await http.PostAsJsonAsync($"{BaseUrl}/api/portal/{slug}/denuncia", request);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<DenunciaResponse>()
                    ?? new DenunciaResponse(string.Empty, DateTime.UtcNow, false, "Respuesta inesperada del servidor");

            var error = await response.Content.ReadAsStringAsync();
            return new DenunciaResponse(string.Empty, DateTime.UtcNow, false, error);
        }
        catch (HttpRequestException ex)
        {
            return new DenunciaResponse(string.Empty, DateTime.UtcNow, false,
                $"No se pudo conectar al servidor: {ex.Message}");
        }
        catch (Exception ex)
        {
            return new DenunciaResponse(string.Empty, DateTime.UtcNow, false, ex.Message);
        }
    }
}
