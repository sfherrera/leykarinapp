namespace LeyKarinApp.Services;

public class ConfiguracionService
{
    private const string KeyServidor = "servidor_url";
    private const string KeySlug = "empresa_slug";
    private const string KeyNombre = "empresa_nombre";

    public string ServidorUrl
    {
        get => ObtenerPreferencia(KeyServidor);
        set => GuardarPreferencia(KeyServidor, value);
    }

    public string EmpresaSlug
    {
        get => ObtenerPreferencia(KeySlug);
        set => GuardarPreferencia(KeySlug, value);
    }

    public string EmpresaNombre
    {
        get => ObtenerPreferencia(KeyNombre);
        set => GuardarPreferencia(KeyNombre, value);
    }

    public bool EstaConfigurada =>
        !string.IsNullOrEmpty(ServidorUrl) && !string.IsNullOrEmpty(EmpresaSlug);

    public void Limpiar()
    {
        try
        {
            Preferences.Remove(KeyServidor);
            Preferences.Remove(KeySlug);
            Preferences.Remove(KeyNombre);
        }
        catch { /* ignore */ }
    }

    private static string ObtenerPreferencia(string key)
    {
        try { return Preferences.Get(key, string.Empty); }
        catch { return string.Empty; }
    }

    private static void GuardarPreferencia(string key, string value)
    {
        try { Preferences.Set(key, value); }
        catch { /* ignore */ }
    }
}
