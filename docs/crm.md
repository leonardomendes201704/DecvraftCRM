# CRM Basico

Este documento controla o escopo inicial do modulo CRM.

## Entidades iniciais

- `Customer`: cliente ou empresa relacionada ao tenant.
- `Contact`: contato associado a um cliente.
- `Opportunity`: oportunidade comercial associada a um cliente.

## Valores fechados

- `CustomerType`: `Company`, `Individual`.
- `CustomerStatus`: `Active`, `Inactive`.
- `OpportunityStatus`: `Open`, `Won`, `Lost`, `Canceled`.

Todos os valores fechados sao enums valorados para evitar strings ou numeros soltos.

## Persistencia

- As entidades CRM sao multi-tenant e implementam `ITenantEntity`.
- O filtro global do `AppDbContext` isola consultas por tenant quando houver tenant corrente.
- `Contact` e `Opportunity` usam FK composta `{TenantId, CustomerId}` para impedir vinculo cruzado entre tenants.
- A migration inicial do CRM e `AddCrmEntities`.

## Clientes

Endpoints iniciais:

- `GET /api/customers`: lista clientes do tenant autenticado. Permissao: `crm.customers.view`.
- `GET /api/customers/{customerId}`: retorna um cliente do tenant autenticado. Permissao: `crm.customers.view`.
- `POST /api/customers`: cria cliente. Permissao: `crm.customers.create`.
- `PUT /api/customers/{customerId}`: atualiza cliente. Permissao: `crm.customers.update`.
- `DELETE /api/customers/{customerId}`: desativa cliente. Permissao: `crm.customers.delete`.

Request de criacao/atualizacao:

```json
{
  "name": "Acme Ltda",
  "document": "12345678000190",
  "type": 1
}
```

Valores de `type`:

- `1`: `Company`.
- `2`: `Individual`.

Documento duplicado dentro do mesmo tenant retorna `409 Conflict`.

## Contatos

Endpoints iniciais:

- `GET /api/customers/{customerId}/contacts`: lista contatos de um cliente do tenant autenticado. Permissao: `crm.customers.view`.
- `GET /api/contacts/{contactId}`: retorna um contato do tenant autenticado. Permissao: `crm.customers.view`.
- `POST /api/customers/{customerId}/contacts`: cria contato para um cliente. Permissao: `crm.customers.create`.
- `PUT /api/contacts/{contactId}`: atualiza contato. Permissao: `crm.customers.update`.
- `DELETE /api/contacts/{contactId}`: remove contato. Permissao: `crm.customers.delete`.

Request de criacao/atualizacao:

```json
{
  "name": "Maria Silva",
  "email": "maria@acme.test",
  "phone": "11999990000",
  "role": "Compras",
  "isPrimary": true
}
```

O contato sempre deve pertencer a um cliente do mesmo tenant. Email duplicado dentro do mesmo cliente retorna `409 Conflict`.

## Proximo bloco

O proximo bloco do CRM deve criar CRUD basico para oportunidades, protegendo os endpoints com permissoes CRM.
