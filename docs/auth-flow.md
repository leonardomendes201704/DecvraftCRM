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
    "permissions": ["crm.customers.read"]
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
  "permissions": ["crm.customers.read"]
}
```

Response `401`: token ausente, invalido ou expirado.
