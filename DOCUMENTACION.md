# Documentacion completa - TipoMovimientoApp

## 1) Objetivo de la aplicacion

Esta aplicacion esta hecha en Blazor Web App (.NET 8) y su objetivo es:

- Consumir una API REST que devuelve una lista de tipos de movimiento.
- Mostrar esos datos en una grilla.
- Soportar estados de carga, error y reintento.
- Permitir cambiar la API sin tocar codigo (solo configuracion).

Formato de datos esperado:

```json
[
  { "Codigo": 29, "Descripcion": "Ajuste al Inventario", "VActiva": false },
  { "Codigo": 51, "Descripcion": "Avance Produccion", "VActiva": false },
  { "Codigo": 17, "Descripcion": "Balance Inicial", "VActiva": false }
]
```

## 2) Arquitectura general

La app sigue una estructura simple por capas:

1. UI (pagina Blazor)
2. Servicio HTTP (consume API)
3. Modelo (mapea JSON)
4. Configuracion (appsettings)

Flujo:

1. El usuario abre la pagina principal.
2. La pagina llama al servicio `TipoMovimientoService`.
3. El servicio lee configuracion (`BaseUrl`, `Endpoint`) y hace un GET.
4. Si la respuesta llega bien, se deserializa a `List<TipoMovimiento>`.
5. La UI pinta la grilla con `QuickGrid`.

## 3) Estructura de archivos clave

- `Program.cs`
  - Configura Blazor, servicios y `HttpClient` tipado.
  - Lee opciones desde `ApiSettings`.
  - Activa static web assets para recursos de paquetes (QuickGrid).

- `appsettings.json`
  - Guarda configuracion de API:
    - `ApiSettings:BaseUrl`
    - `ApiSettings:Endpoint`
    - `ApiSettings:IgnoreSslErrors`

- `Services/ApiSettings.cs`
  - Clase de opciones para mapear la seccion `ApiSettings`.

- `Services/TipoMovimientoService.cs`
  - Contiene la llamada HTTP al endpoint configurado.
  - Si faltan configuraciones clave, lanza errores explicativos.
  - Si `BaseUrl` sigue en placeholder (`tu-api.ejemplo.com`), devuelve datos demo para que la grilla funcione.

- `Models/TipoMovimiento.cs`
  - DTO que mapea exactamente el JSON:
    - `Codigo`
    - `Descripcion`
    - `VActiva`

- `Components/Pages/Movimientos.razor`
  - Pagina principal (`/`).
  - Muestra:
    - Estado de carga
    - Estado vacio
    - Estado de error
    - Grilla con ordenamiento

- `Components/App.razor`
  - Layout HTML base del documento.
  - Incluye stylesheet principal y favicon.

- `wwwroot/app.css`
  - Estilos de tabla, badges y alertas.

## 4) Configuracion de API

En `appsettings.json`:

```json
"ApiSettings": {
  "BaseUrl": "https://tu-api.ejemplo.com/",
  "Endpoint": "api/TipoMovimiento",
  "IgnoreSslErrors": true
}
```

### Significado de cada valor

- `BaseUrl`
  - Dominio base de la API.
  - Ejemplo: `https://api.miempresa.com/`

- `Endpoint`
  - Ruta relativa del recurso.
  - Ejemplo: `api/TipoMovimiento`

- `IgnoreSslErrors`
  - `true`: ignora errores de certificado SSL (solo para desarrollo/pruebas).
  - `false`: validacion SSL normal (recomendado para produccion).

## 5) Modo demo (sin API real)

Si aun no tienes la URL real y dejas `BaseUrl` en `https://tu-api.ejemplo.com/`,
la app entra en modo demo y devuelve internamente los 3 registros de ejemplo.

Esto permite:

- Demostrar funcionamiento de la grilla.
- Validar UI y estados sin depender del backend real.

Cuando te pasen la API real, solo cambias `BaseUrl` y `Endpoint`.

## 6) Como ejecutar en local

Desde la carpeta del proyecto:

```powershell
$env:ASPNETCORE_ENVIRONMENT='Development'
& "C:\Program Files\dotnet\dotnet.exe" run
```

Luego abrir:

- http://localhost:5000

## 7) Errores comunes y solucion

### A) "dotnet no se reconoce"

Causa: `dotnet` no esta en PATH de esa terminal.

Solucion rapida:

```powershell
& "C:\Program Files\dotnet\dotnet.exe" run
```

Solucion de sesion:

```powershell
$env:Path = [Environment]::GetEnvironmentVariable('Path','Machine') + ';' + [Environment]::GetEnvironmentVariable('Path','User')
```

### B) "address already in use" (puerto 5000 ocupado)

Causa: ya hay otra instancia de la app ejecutandose.

Solucion:

```powershell
Get-NetTCPConnection -LocalPort 5000 -State Listen | Select-Object -First 1 -ExpandProperty OwningProcess | ForEach-Object { Stop-Process -Id $_ -Force }
```

### C) Error SSL al consultar API

Causa: certificado no valido o endpoint de ejemplo.

Soluciones:

1. Verifica `BaseUrl` real.
2. Para pruebas locales, usa `IgnoreSslErrors: true`.
3. En produccion, usa certificado valido y `IgnoreSslErrors: false`.

### D) QuickGrid no carga recurso JS

Causa: recursos `_content` no resueltos en cierta ejecucion.

Solucion aplicada en el proyecto:

- `builder.WebHost.UseStaticWebAssets();` en `Program.cs`.
- Ejecutar en `Development` durante pruebas locales.

## 8) Comportamiento de la pagina principal

`Movimientos.razor` maneja 4 escenarios:

1. Cargando: muestra mensaje de carga.
2. Error: muestra mensaje y boton Reintentar.
3. Sin datos: informa que no hay registros.
4. Con datos: renderiza QuickGrid.

La columna `Activa` usa badge visual:

- `Si` si `VActiva = true`
- `No` si `VActiva = false`

## 9) Como pasar de demo a produccion

1. Reemplazar `BaseUrl` y `Endpoint` por valores reales.
2. Desactivar `IgnoreSslErrors`.
3. Si aplica, agregar autenticacion en `AddHttpClient` (token/API key).
4. Probar errores de red y respuestas vacias.
5. Publicar.

## 10) Mejoras recomendadas (siguientes pasos)

- Agregar logging estructurado del request/response.
- Agregar timeout y politicas de reintento.
- Agregar tests del servicio (mock de HttpClient).
- Agregar paginacion en grilla si el volumen crece.
- Agregar soporte de autenticacion JWT/API key si la API lo requiere.

---

Si quieres, en un siguiente paso puedo crear una version "para entregar" en PDF/Markdown breve (1 pagina) y otra tecnica detallada para repositorio.
