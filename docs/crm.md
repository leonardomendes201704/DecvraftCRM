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

## Proximo bloco

O proximo bloco do CRM deve criar os contratos, servicos e endpoints CRUD basicos para clientes, contatos e oportunidades, protegidos por permissoes CRM.
