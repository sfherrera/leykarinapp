namespace LeyKarinApp.Models;

public record DenunciaResponse(string Folio, DateTime FechaRecepcion, bool Exito, string? Error);
