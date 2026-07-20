using System.Text.Json.Serialization;

namespace TipoMovimientoApp.Models;

/// <summary>
/// DTO que representa un tipo de movimiento recibido desde la API.
/// </summary>
public class TipoMovimiento
{
    /// <summary>Codigo numerico del tipo de movimiento.</summary>
    [JsonPropertyName("Codigo")]
    public int Codigo { get; set; }

    /// <summary>Descripcion legible del movimiento.</summary>
    [JsonPropertyName("Descripcion")]
    public string Descripcion { get; set; } = string.Empty;

    /// <summary>Indica si el tipo de movimiento esta activo.</summary>
    [JsonPropertyName("VActiva")]
    public bool VActiva { get; set; }
}
