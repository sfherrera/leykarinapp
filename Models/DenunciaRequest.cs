namespace LeyKarinApp.Models;

public class DenunciaRequest
{
    public Guid? TipoDenunciaId { get; set; }
    public bool EsAnonima { get; set; } = true;
    public string DescripcionHechos { get; set; } = string.Empty;
    public DateTime? FechaHechos { get; set; }
    public string? LugarHechos { get; set; }

    // Denunciado
    public string? NombresDenunciado { get; set; }
    public string? ApellidosDenunciado { get; set; }
    public string? CargoDenunciado { get; set; }
    public string? RelacionConDenunciante { get; set; }

    // Denunciante (solo si no es anónima)
    public string? NombresDenunciante { get; set; }
    public string? ApellidosDenunciante { get; set; }
    public string? RutDenunciante { get; set; }
    public string? EmailDenunciante { get; set; }
    public string? TelefonoDenunciante { get; set; }
    public string? CargoDenunciante { get; set; }
    public bool SolicitaReserva { get; set; }
}
