using System.Net.Http.Json;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using TipoMovimientoApp.Models;

namespace TipoMovimientoApp.Services;

/// <summary>
/// Servicio de acceso a datos para consultar tipos de movimiento desde la API.
/// Incluye cache en memoria para reducir llamadas repetidas.
/// </summary>
public class TipoMovimientoService
{
    private const string CacheKey = "tipos-movimiento";
    private static readonly SemaphoreSlim RefreshLock = new(1, 1);

    private readonly HttpClient _httpClient;
    private readonly ApiSettings _apiSettings;
    private readonly IMemoryCache _cache;

    public TipoMovimientoService(
        HttpClient httpClient,
        IOptions<ApiSettings> apiSettings,
        IMemoryCache cache)
    {
        _httpClient = httpClient;
        _apiSettings = apiSettings.Value;
        _cache = cache;
    }

    /// <summary>
    /// Obtiene la lista de tipos de movimiento.
    /// Primero intenta leer desde cache y, si no existe, consume la API.
    /// </summary>
    /// <param name="ct">Token de cancelacion para la solicitud HTTP.</param>
    /// <returns>Lista de tipos de movimiento.</returns>
    /// <exception cref="InvalidOperationException">
    /// Se lanza cuando falta configuracion critica de ApiSettings.
    /// </exception>
    public async Task<List<TipoMovimiento>> ObtenerTiposMovimientoAsync(CancellationToken ct = default)
    {
        // Atajo rapido: devolver una copia de cache para no exponer la instancia interna.
        if (_cache.TryGetValue(CacheKey, out List<TipoMovimiento>? cached) && cached is not null)
        {
            return cached.ToList();
        }

        await RefreshLock.WaitAsync(ct);

        try
        {
            // Doble verificacion despues del lock para evitar llamadas simultaneas a la API.
            if (_cache.TryGetValue(CacheKey, out cached) && cached is not null)
            {
                return cached.ToList();
            }

            if (string.IsNullOrWhiteSpace(_apiSettings.Endpoint))
            {
                throw new InvalidOperationException("Falta configurar ApiSettings:Endpoint en appsettings.json");
            }

            if (_httpClient.BaseAddress is null)
            {
                throw new InvalidOperationException("Falta configurar ApiSettings:BaseUrl en appsettings.json");
            }

            if (_httpClient.BaseAddress.Host.Equals("tu-api.ejemplo.com", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(
                    "ApiSettings:BaseUrl aun tiene el valor de ejemplo. Configura la URL real de tu API para obtener todos los datos.");
            }

            var endpoint = _apiSettings.Endpoint.TrimStart('/');
            var resultado = await _httpClient.GetFromJsonAsync<List<TipoMovimiento>>(endpoint, ct);
            var movimientos = resultado ?? new List<TipoMovimiento>();

            _cache.Set(CacheKey, movimientos, TimeSpan.FromMinutes(5));
            return movimientos.ToList();
        }
        finally
        {
            RefreshLock.Release();
        }
    }
}
