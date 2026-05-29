# Votaciones Residenciales Resident

Frontend independiente para residentes, diseñado como PWA mobile-first sobre la API de `VotacionesResidenciales`.

## Stack

- Angular 21
- Standalone components
- Service Worker y Web App Manifest
- Arquitectura por capas
- CQRS ligero con `CommandBus` y `QueryBus`
- Repositorios HTTP desacoplados
- SignalR para actualizaciones de estado de votacion

## Estructura

```text
src/app/
  core/
    auth/
    guards/
    http/
    pwa/
  resident/
    domain/
    application/
    infrastructure/
    presentation/
```

## Experiencia inicial

- Login de residente
- Inicio con resumen de votaciones
- Lista de procesos abiertos y cerrados
- Emision de voto desde el detalle
- Perfil con cambio de PIN
- CTA de instalacion como app

## Desarrollo local

La app usa `proxy.conf.json` para reenviar `/api` y `/hubs` hacia la API en `https://localhost:7001`.

```bash
npm install
npm start
```

Por defecto arranca en:

```text
http://localhost:4301
```

## Credenciales seed

- Cedula: `1000000001`
- PIN: `1234`
