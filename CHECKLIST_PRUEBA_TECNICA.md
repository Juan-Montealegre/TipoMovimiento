# Checklist de validacion

## Requisitos funcionales

- [x] Proyecto construido en Blazor Web App (.NET 8).
- [x] Consumo de API REST.
- [x] Visualizacion en grilla de Codigo, Descripcion y VActiva.
- [x] Manejo de estado de carga.
- [x] Manejo de error de consulta.
- [x] Manejo de estado sin resultados.
- [x] Boton de recarga/reintento.

## Requisitos tecnicos

- [x] Configuracion externa en appsettings.json.
- [x] Inyeccion de dependencias en Program.cs.
- [x] Servicio dedicado para acceso a API.
- [x] Modelo tipado para deserializacion.
- [x] Compilacion exitosa con dotnet build.

## Validaciones recomendadas antes de entregar

- [ ] Confirmar URL y token de ambiente evaluador.
- [ ] Ejecutar dotnet run y validar render completo en navegador.
- [ ] Verificar respuesta ante error 401/403 o timeout.
- [ ] Adjuntar capturas de pantalla del estado con datos.
