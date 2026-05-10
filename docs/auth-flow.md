# Fluxo de Autenticacao

Este documento descreve os endpoints iniciais de autenticacao da API.

## `POST /api/auth/login`

Autentica um usuario ativo dentro de um tenant.

Request:

```json
{
  "tenantSlug": "empresa-demo",
  "email": "admin@demo.com",
  "password": "Admin@123456"
}
```

Response `200`:

```json
{
  "accessToken": "<jwt>",
  "expiresAt": "2026-05-09T18:00:00+00:00",
  "user": {
    "userId": "00000000-0000-0000-0000-000000000000",
    "tenantId": "00000000-0000-0000-0000-000000000000",
    "tenantSlug": "empresa-demo",
    "name": "Administrador",
    "email": "admin@demo.com",
    "roles": ["TenantAdmin"],
    "permissions": ["crm.customers.view"]
  }
}
```

Response `401`: credenciais invalidas, tenant inativo ou usuario inativo.

## `GET /api/me`

Retorna o usuario autenticado a partir do bearer token.

Headers:

```http
Authorization: Bearer <jwt>
```

Response `200`:

```json
{
  "userId": "00000000-0000-0000-0000-000000000000",
  "tenantId": "00000000-0000-0000-0000-000000000000",
  "tenantSlug": "empresa-demo",
  "name": "Administrador",
  "email": "admin@demo.com",
  "roles": ["TenantAdmin"],
  "permissions": ["crm.customers.view"]
}
```

Response `401`: token ausente, invalido ou expirado.

## `GET /api/system/permissions`

Retorna o catalogo de permissoes conhecidas. Este endpoint exige a permissao `core.system.view`.

Headers:

```http
Authorization: Bearer <jwt>
```

Response `200`:

```json
[
  {
    "key": "core.system.view",
    "description": "Visualizar informacoes do sistema",
    "moduleSlug": "core"
  }
]
```

Response `401`: token ausente, invalido ou expirado.

Response `403`: usuario autenticado sem a permissao exigida.

## Login pelo `Platform.Web`

O frontend operacional usa os mesmos contratos da API:

1. Usuario informa endereco da API, tenant, e-mail e senha.
2. `Platform.Web` chama `POST /api/auth/login`.
3. Com o `accessToken`, `Platform.Web` chama `GET /api/me`.
4. O token, tenant, permissoes e dados do usuario ficam em cookie web `HttpOnly`.
5. Logout remove o cookie local.

O endereco da API e informado em runtime na tela de login. Ele nao deve ser gravado em `appsettings`, variaveis de ambiente ou codigo fonte; configuracoes definitivas continuam sendo responsabilidade de entidades em banco.

## `GET /api/modules`

Retorna o catalogo de modulos com status de instalacao e ativacao para o tenant autenticado. Este endpoint exige a permissao `core.modules.view`.

Headers:

```http
Authorization: Bearer <jwt>
```

Response `200`:

```json
[
  {
    "id": "00000000-0000-0000-0000-000000000000",
    "name": "Core",
    "slug": "core",
    "version": "1.0.0",
    "isCore": true,
    "isEnabled": true,
    "isInstalled": true,
    "isActiveForTenant": true
  }
]
```

Response `401`: token ausente, invalido ou expirado.

Response `403`: usuario autenticado sem a permissao exigida.
