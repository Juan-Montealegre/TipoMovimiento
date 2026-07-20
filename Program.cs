using TipoMovimientoApp.Components;
using TipoMovimientoApp.Services;
using System.Net.Http.Headers;
using Microsoft.Extensions.Options;

// Punto de entrada de la aplicacion Blazor Server.
// Configura servicios, cliente HTTP, middleware y rutas.
var builder = WebApplication.CreateBuilder(args);

// Permite resolver _content de paquetes (ej. QuickGrid) al ejecutar localmente.
builder.WebHost.UseStaticWebAssets();

// Componentes Razor con render interactivo del lado del servidor
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Cache en memoria requerida por TipoMovimientoService
builder.Services.AddMemoryCache();

// Configuracion para API REST (BaseUrl + Endpoint)
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));

// Registra un HttpClient tipado para centralizar:
// - URL base
// - encabezados de autenticacion
// - comportamiento SSL opcional para ambientes de prueba
builder.Services.AddHttpClient<TipoMovimientoService>((serviceProvider, client) =>
{
    var apiSettings = serviceProvider.GetRequiredService<IOptions<ApiSettings>>().Value;

    if (string.IsNullOrWhiteSpace(apiSettings.BaseUrl))
    {
        throw new InvalidOperationException("Falta configurar ApiSettings:BaseUrl en appsettings.json");
    }

    client.BaseAddress = new Uri(apiSettings.BaseUrl);

    if (!string.IsNullOrWhiteSpace(apiSettings.AuthToken))
    {
        var headerName = string.IsNullOrWhiteSpace(apiSettings.AuthHeaderName)
            ? "Authorization"
            : apiSettings.AuthHeaderName;

        if (headerName.Equals("Authorization", StringComparison.OrdinalIgnoreCase))
        {
            var scheme = string.IsNullOrWhiteSpace(apiSettings.AuthScheme)
                ? "Bearer"
                : apiSettings.AuthScheme;

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme, apiSettings.AuthToken);
        }
        else
        {
            client.DefaultRequestHeaders.Remove(headerName);
            client.DefaultRequestHeaders.TryAddWithoutValidation(headerName, apiSettings.AuthToken);
        }
    }
})
.ConfigurePrimaryHttpMessageHandler(serviceProvider =>
{
    var apiSettings = serviceProvider.GetRequiredService<IOptions<ApiSettings>>().Value;

    if (!apiSettings.IgnoreSslErrors)
    {
        return new HttpClientHandler();
    }

    return new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    };
});

var app = builder.Build();

// Pipeline de errores para ambientes distintos a desarrollo.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

var httpsPort = app.Configuration["ASPNETCORE_HTTPS_PORT"];
var urls = app.Configuration["ASPNETCORE_URLS"];
var hasHttpsUrl = !string.IsNullOrWhiteSpace(urls) && urls.Contains("https://", StringComparison.OrdinalIgnoreCase);

// Solo fuerza redireccion HTTPS cuando existe configuracion HTTPS activa.
if (!string.IsNullOrWhiteSpace(httpsPort) || hasHttpsUrl)
{
    app.UseHttpsRedirection();
}

app.UseStaticFiles();
app.UseAntiforgery();

// Publica los componentes Razor y habilita render interactivo en servidor.
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
