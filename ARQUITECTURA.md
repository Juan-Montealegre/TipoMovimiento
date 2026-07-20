# Arquitectura y flujo

## Diagrama de flujo funcional

Usuario
  -> Navega a la pagina principal
  -> Components/Pages/Movimientos.razor
  -> Services/TipoMovimientoService.cs
  -> HttpClient tipado (registrado en Program.cs)
  -> API REST (BaseUrl + Endpoint desde appsettings.json)
  -> Respuesta JSON
  -> Models/TipoMovimiento.cs
  -> Render en grilla (QuickGrid)

## Capas

1. Presentacion
- Components/Layout/MainLayout.razor
- Components/Pages/Movimientos.razor

2. Aplicacion / Servicio
- Services/TipoMovimientoService.cs

3. Configuracion
- Services/ApiSettings.cs
- appsettings.json

4. Hosting y dependencias
- Program.cs

## Decisiones tecnicas clave

- HttpClientFactory con servicio tipado para manejo correcto de conexiones.
- Opciones fuertemente tipadas (ApiSettings) para evitar valores hardcoded.
- Separacion de responsabilidades entre UI, servicio y modelo.
- Cache en memoria para reducir llamadas repetidas en sesiones cercanas.
