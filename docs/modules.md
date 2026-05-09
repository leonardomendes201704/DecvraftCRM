# Modulos

Este documento controla o catalogo inicial de modulos da plataforma.

## Catalogo inicial

- `core`: funcionalidades essenciais da plataforma.
- `crm`: funcionalidades comerciais e relacionamento com clientes.
- `finance`: funcionalidades financeiras iniciais.

Os metadados conhecidos ficam centralizados em `KnownModules`.

## Ativacao por tenant

- `Modules` representa o catalogo global.
- `TenantModules` representa instalacao e ativacao por tenant.
- Um modulo pode existir globalmente e ainda nao estar instalado para um tenant.
- A resposta da API diferencia `isInstalled` de `isActiveForTenant`.

## Endpoint

`GET /api/modules` retorna os modulos globais com flags calculadas para o tenant autenticado.

Permissao exigida: `core.modules.view`.
