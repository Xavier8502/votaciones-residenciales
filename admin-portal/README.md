# Votaciones Residenciales Admin

Frontend administrativo independiente para la API de `VotacionesResidenciales`.

## Stack

- Angular 21
- Standalone components
- Reactive Forms
- Arquitectura por capas en frontend
- CQRS ligero con `CommandBus` y `QueryBus`
- Repositorios HTTP desacoplados de la presentación

## Estructura

```text
src/app/
  core/
    auth/
    guards/
    http/
  admin/
    domain/
    application/
    infrastructure/
    presentation/
```

### Capas

- `domain`: contratos, entidades y catálogos del negocio
- `application`: comandos, consultas y handlers
- `infrastructure`: implementación HTTP de repositorios
- `presentation`: shell, páginas y formularios

## Funcionalidad inicial

- Login administrativo contra `/api/auth/login`
- Shell privada con navegación
- Dashboard operativo
- Gestión de conjuntos
- Gestión de inmuebles
- Gestión de residentes
- Gestión de votaciones con apertura, cierre y lectura de resultados

## Desarrollo local

La app usa `proxy.conf.json` para reenviar `/api` y `/hubs` hacia la API en `https://localhost:7001`.

### Front

```bash
npm install
npm start
```

Por defecto arranca en:

```text
http://localhost:4300
```

### API esperada

```text
https://localhost:7001
http://localhost:5001
```

## Credenciales seed

- Cédula: `9000000001`
- PIN: `1234`

## Notas

- El portal está pensado para `AdminConjunto`.
- Si el login devuelve otro rol, la sesión se descarta.
- La integración está lista para crecer hacia SignalR y reportes sin mezclar la UI con la capa HTTP.
