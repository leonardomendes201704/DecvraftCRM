# CRM Basico

Este documento controla o escopo inicial do modulo CRM.

## Entidades iniciais

- `Customer`: cliente ou empresa relacionada ao tenant.
- `Contact`: contato associado a um cliente.
- `Opportunity`: oportunidade comercial associada a um cliente.
- `OpportunityStage`: etapa do pipeline comercial do tenant.
- `OpportunityActivity`: atividade planejada ou executada em uma oportunidade.
- `OpportunityHistoryEntry`: registro historico automatico de eventos da oportunidade.

## Valores fechados

- `CustomerType`: `Company`, `Individual`.
- `CustomerStatus`: `Active`, `Inactive`.
- `OpportunityStatus`: `Open`, `Won`, `Lost`, `Canceled`.
- `OpportunityActivityType`: `Call`, `Email`, `Meeting`, `Task`, `Proposal`, `FollowUp`.
- `OpportunityActivityStatus`: `Scheduled`, `Completed`, `Canceled`.
- `OpportunityHistoryEventType`: `Created`, `Updated`, `StageChanged`, `Won`, `Lost`, `Canceled`, `ActivityCreated`, `ActivityUpdated`, `ActivityCompleted`, `ActivityCanceled`.

Todos os valores fechados sao enums valorados para evitar strings ou numeros soltos.

## Persistencia

- As entidades CRM sao multi-tenant e implementam `ITenantEntity`.
- O filtro global do `AppDbContext` isola consultas por tenant quando houver tenant corrente.
- `Contact` e `Opportunity` usam FK composta `{TenantId, CustomerId}` para impedir vinculo cruzado entre tenants.
- Oportunidades, atividades e historico usam chaves compostas com `TenantId` para impedir vinculos cruzados entre tenants.
- A migration inicial do CRM e `AddCrmEntities`.
- A migration de pipeline, atividades e historico e `AddOpportunityPipelineActivities`.

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
Cada cliente pode ter apenas um contato principal por tenant. Ao criar ou atualizar um contato como principal, os demais contatos principais do mesmo cliente sao desmarcados e a persistencia reforca essa regra com indice unico filtrado.

## Oportunidades

Endpoints iniciais:

- `GET /api/customers/{customerId}/opportunities`: lista oportunidades de um cliente do tenant autenticado. Permissao: `crm.opportunities.view`.
- `GET /api/opportunities/{opportunityId}`: retorna uma oportunidade do tenant autenticado. Permissao: `crm.opportunities.view`.
- `POST /api/customers/{customerId}/opportunities`: cria oportunidade para um cliente. Permissao: `crm.opportunities.manage`.
- `PUT /api/opportunities/{opportunityId}`: atualiza oportunidade. Permissao: `crm.opportunities.manage`.
- `POST /api/opportunities/{opportunityId}/won`: marca oportunidade como ganha. Permissao: `crm.opportunities.manage`.
- `POST /api/opportunities/{opportunityId}/lost`: marca oportunidade como perdida. Permissao: `crm.opportunities.manage`.
- `POST /api/opportunities/{opportunityId}/canceled`: cancela oportunidade. Permissao: `crm.opportunities.manage`.

Request de criacao/atualizacao:

```json
{
  "title": "Projeto ERP",
  "estimatedValue": 15000.00,
  "expectedCloseDate": "2026-07-31"
}
```

A oportunidade sempre deve pertencer a um cliente do mesmo tenant. Valor estimado negativo retorna `400 Bad Request`.

## Pipeline de oportunidades

Endpoints iniciais:

- `GET /api/opportunity-stages`: lista etapas do tenant autenticado. Permissao: `crm.opportunities.view`.
- `POST /api/opportunity-stages`: cria etapa. Permissao: `crm.opportunities.manage`.
- `PUT /api/opportunity-stages/{stageId}`: atualiza etapa. Permissao: `crm.opportunities.manage`.
- `DELETE /api/opportunity-stages/{stageId}`: desativa etapa. Permissao: `crm.opportunities.manage`.
- `GET /api/opportunity-stages/{stageId}/opportunities`: lista oportunidades vinculadas a uma etapa. Permissao: `crm.opportunities.view`.
- `PUT /api/opportunities/{opportunityId}/stage`: move oportunidade para uma etapa ativa. Permissao: `crm.opportunities.manage`.

Request de criacao/atualizacao de etapa:

```json
{
  "name": "Proposta enviada",
  "position": 3
}
```

Request de movimentacao:

```json
{
  "stageId": "00000000-0000-0000-0000-000000000000"
}
```

## Atividades e historico

Endpoints iniciais:

- `GET /api/opportunities/{opportunityId}/activities`: lista atividades da oportunidade. Permissao: `crm.opportunities.view`.
- `POST /api/opportunities/{opportunityId}/activities`: cria atividade. Permissao: `crm.opportunities.manage`.
- `PUT /api/opportunity-activities/{activityId}`: atualiza atividade. Permissao: `crm.opportunities.manage`.
- `POST /api/opportunity-activities/{activityId}/complete`: conclui atividade. Permissao: `crm.opportunities.manage`.
- `POST /api/opportunity-activities/{activityId}/cancel`: cancela atividade. Permissao: `crm.opportunities.manage`.
- `GET /api/opportunity-activities/overdue`: lista atividades vencidas ainda agendadas. Permissao: `crm.opportunities.view`.
- `GET /api/opportunity-activities/upcoming?days=7`: lista proximas atividades agendadas. Permissao: `crm.opportunities.view`.
- `GET /api/opportunities/{opportunityId}/history`: lista historico da oportunidade. Permissao: `crm.opportunities.view`.

Request de criacao/atualizacao de atividade:

```json
{
  "type": 3,
  "title": "Reuniao de apresentacao",
  "notes": "Apresentar proposta comercial",
  "dueAt": "2026-06-01T14:00:00-03:00"
}
```

Valores de `type`:

- `1`: `Call`.
- `2`: `Email`.
- `3`: `Meeting`.
- `4`: `Task`.
- `5`: `Proposal`.
- `6`: `FollowUp`.

Eventos de criacao, atualizacao, mudanca de etapa, ganho, perda, cancelamento e alteracoes de atividades sao registrados automaticamente em `OpportunityHistoryEntry`.

## Proximo bloco

O proximo bloco recomendado e modelar responsaveis comerciais em oportunidades e atividades para permitir agenda por usuario, distribuicao de carteira e filtros por responsavel.
