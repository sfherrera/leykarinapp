namespace LeyKarinApp.Services;

public class ConfiguracionService
{
    private const string KeyServidor = "servidor_url";
    private const string KeySlug = "empresa_slug";
    private const string KeyNombre = "empresa_nombre";

    public string ServidorUrl
    {
        get => Preferences.Default.Get(KeyServidor, string.Empty);
        set => Preferences.Default.Set(KeyServidor, value);
    }

    public string EmpresaSlug
    {
        get => Preferences.Default.Get(KeySlug, string.Empty);
        set => Preferences.Default.Set(KeySlug, value);
    }

    public string EmpresaNombre
    {
        get => Preferences.Default.Get(KeyNombre, string.Empty);
        set => Preferences.Default.Set(KeyNombre, value);
    }

    public bool EstaConfigurada =>
        !string.IsNullOrEmpty(ServidorUrl) && !string.IsNullOrEmpty(EmpresaSlug);

    public void Limpiar()
    {
        Preferences.Default.Remove(KeyServidor);
        Preferences.Default.Remove(KeySlug);
        Preferences.Default.Remove(KeyNombre);
    }
}
