# TipoMovimientoApp - Panel de Tipos de Movimiento

Aplicacion web en Blazor (.NET 8, render interactivo Server) para consultar,
filtrar y visualizar tipos de movimiento desde una API REST corporativa.

La pagina principal muestra:

- Total de registros.
- Cantidad de movimientos activos e inactivos.
- Tabla ordenable (QuickGrid) con busqueda por texto.
- Filtro por estado (todos, activos, inactivos).
- Manejo de errores de conexion con boton de reintento.

## Tecnologias

- .NET 8
- Blazor Web App (Interactive Server)
- QuickGrid
- HttpClientFactory
- IMemoryCache

## Requisitos

- SDK de .NET 8 instalado.

Verificacion rapida:

```powershell
dotnet --version
```

## Configuracion

El consumo de API se define en appsettings.json, seccion ApiSettings:

```json
"ApiSettings": {
  "BaseUrl": "https://tu-servidor/",
  "Endpoint": "api/tu-endpoint",
  "AuthToken": "tu-token",
  "AuthHeaderName": "Authorization",
  "AuthScheme": "Bearer",
  "IgnoreSslErrors": false
}
```

Descripcion de campos:

- BaseUrl: URL base del servicio.
- Endpoint: ruta relativa del recurso a consultar.
- AuthToken: token para autenticacion.
- AuthHeaderName: nombre del header (por defecto Authorization).
- AuthScheme: esquema de autenticacion (por ejemplo Bearer).
- IgnoreSslErrors: solo para ambientes de prueba con certificados no validos.

## Como ejecutar

Desde la raiz del proyecto:

```powershell
dotnet restore
dotnet build
dotnet run
```

Luego abre en el navegador la URL mostrada en consola
(normalmente https://localhost:xxxx).

## Estructura del proyecto

### Archivos raiz

- Program.cs: punto de entrada; configura servicios, HttpClient tipado, middleware y rutas.
- appsettings.json: configuracion general y parametros de conexion a API.
- TipoMovimientoApp.csproj: definicion del proyecto y paquetes NuGet.
- README.md: guia de uso rapido del proyecto.
- DOCUMENTACION.md: documentacion extendida funcional/tecnica.
- ARQUITECTURA.md: vision de arquitectura y decisiones de alto nivel.
- CHECKLIST_PRUEBA_TECNICA.md: lista de validacion para entrega.
- README_ENTREGA.md: notas de entrega final.

### Components/

- Components/_Imports.razor: usings globales para componentes Razor.
- Components/App.razor: documento host HTML principal (head, estilos, favicon y boot de Blazor).
- Components/Routes.razor: enrutador de paginas, layout por defecto y vista de NotFound.

#### Components/Layout/

- Components/Layout/MainLayout.razor: layout base de la interfaz (sidebar + topbar + area de contenido).

#### Components/Pages/

- Components/Pages/Movimientos.razor: pagina raiz "/"; carga datos de API, muestra metricas y tabla con filtros.

### Models/

- Models/TipoMovimiento.cs: DTO del JSON de la API (Codigo, Descripcion, VActiva).

### Services/

- Services/ApiSettings.cs: clase de opciones para mapear ApiSettings desde configuracion.
- Services/TipoMovimientoService.cs: logica de consumo API, validaciones de configuracion y cache en memoria.

### wwwroot/

- wwwroot/app.css: estilos globales de la aplicacion.
- wwwroot/favicon.png: icono del sitio.

## Flujo funcional resumido

1. Program.cs registra el servicio TipoMovimientoService y su HttpClient.
2. Movimientos.razor se inicializa y llama CargarDatosAsync.
3. TipoMovimientoService consulta la API y cachea la respuesta por 5 minutos.
4. La UI calcula metricas y aplica filtros para renderizar QuickGrid.
5. Si ocurre un error, la pagina lo muestra y permite reintentar.

## Comandos utiles

```powershell
dotnet clean
dotnet build
dotnet run
```

## Nota de seguridad

Si usas un token real en appsettings.json, evita subirlo a repositorios publicos.
Para produccion, considera variables de entorno o user-secrets.
