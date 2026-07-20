# Prueba Tecnica - Blazor Web App

## Objetivo cumplido

Se desarrollo una aplicacion en Blazor Web App (.NET 8) que consume una API REST y despliega su resultado en una grilla, siguiendo la estructura solicitada:

[
  { "Codigo": 29, "Descripcion": "Ajuste al Inventario", "VActiva": false },
  { "Codigo": 51, "Descripcion": "Avance Produccion", "VActiva": false },
  { "Codigo": 17, "Descripcion": "Balance Inicial", "VActiva": false }
]

## Alcance implementado

- Consumo HTTP de endpoint configurable.
- Mapeo tipado del JSON al modelo de dominio.
- Renderizado de datos en grilla con ordenamiento.
- Estados de interfaz: carga, error, vacio y exito.
- Filtro por texto y por estado activo/inactivo.
- Configuracion externa para URL, endpoint y autenticacion.

## Estructura de solucion

- Program.cs: registro de servicios, HttpClient tipado, cache y pipeline.
- Services/ApiSettings.cs: opciones de configuracion de API.
- Services/TipoMovimientoService.cs: consumo de API y logica de lectura.
- Models/TipoMovimiento.cs: DTO con propiedades Codigo, Descripcion y VActiva.
- Components/Pages/Movimientos.razor: grilla, filtros, estados y acciones de recarga.
- wwwroot/app.css: estilos de presentacion responsive.

## Configuracion

La aplicacion toma la configuracion desde appsettings.json:

- ApiSettings:BaseUrl
- ApiSettings:Endpoint
- ApiSettings:AuthToken
- ApiSettings:AuthHeaderName
- ApiSettings:AuthScheme
- ApiSettings:IgnoreSslErrors

## Ejecucion local

1. Restaurar y compilar:

   dotnet restore
   dotnet build

2. Ejecutar:

   dotnet run

3. Abrir la URL indicada por consola.

## Resultado esperado

Al iniciar la aplicacion, la pagina principal consulta la API y muestra en grilla los registros devueltos, incluyendo Codigo, Descripcion y estado VActiva.
