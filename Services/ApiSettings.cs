namespace TipoMovimientoApp.Services;

/// <summary>
/// Opciones de configuracion para conectar con la API de tipos de movimiento.
/// Se carga desde la seccion ApiSettings de appsettings.json.
/// </summary>
public class ApiSettings
{
    /// <summary>URL base de la API (incluye host y ruta base).</summary>
    public string BaseUrl { get; set; } = string.Empty;

    /// <summary>Ruta relativa del endpoint a consultar.</summary>
    public string Endpoint { get; set; } = string.Empty;

    /// <summary>Token de autenticacion enviado al consumir la API.</summary>
    public string AuthToken { get; set; } = string.Empty;

    /// <summary>Nombre del encabezado HTTP para autenticacion.</summary>
    public string AuthHeaderName { get; set; } = "Authorization";

    /// <summary>Esquema del encabezado Authorization (por ejemplo: Bearer).</summary>
    public string AuthScheme { get; set; } = "Bearer";

    /// <summary>
    /// Permite ignorar errores SSL en entornos no productivos.
    /// Debe permanecer en false para produccion.
    /// </summary>
    public bool IgnoreSslErrors { get; set; }
}
