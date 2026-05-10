# Financeiro Basico

Este documento controla o escopo inicial do modulo financeiro.

## Entidades iniciais

- `FinancialAccount`: conta financeira do tenant.
- `FinancialTransaction`: lancamento financeiro associado a uma conta.

## Valores fechados

- `FinancialAccountStatus`: `Active`, `Inactive`.
- `FinancialTransactionType`: `Credit`, `Debit`.
- `FinancialTransactionStatus`: `Posted`, `Voided`.

Todos os valores fechados sao enums valorados para evitar strings ou numeros soltos.

## Persistencia

- As entidades financeiras sao multi-tenant e implementam `ITenantEntity`.
- `FinancialTransaction` usa FK composta `{TenantId, AccountId}` para impedir vinculo cruzado entre tenants.
- A migration inicial do financeiro e `AddFinanceEntities`.

## Contas financeiras

Endpoints iniciais:

- `GET /api/financial/accounts`: lista contas do tenant autenticado. Permissao: `finance.accounts.view`.
- `GET /api/financial/accounts/{accountId}`: retorna uma conta do tenant autenticado. Permissao: `finance.accounts.view`.
- `POST /api/financial/accounts`: cria conta. Permissao: `finance.accounts.manage`.
- `PUT /api/financial/accounts/{accountId}`: atualiza conta. Permissao: `finance.accounts.manage`.
- `DELETE /api/financial/accounts/{accountId}`: desativa conta. Permissao: `finance.accounts.manage`.

Request de criacao:

```json
{
  "name": "Caixa",
  "openingBalance": 1000.00
}
```

Request de atualizacao:

```json
{
  "name": "Banco Principal"
}
```

Nome duplicado dentro do mesmo tenant retorna `409 Conflict`.

## Lancamentos financeiros

Endpoints iniciais:

- `GET /api/financial/accounts/{accountId}/transactions`: lista lancamentos da conta. Permissao: `finance.transactions.view`.
- `GET /api/financial/transactions/{transactionId}`: retorna um lancamento. Permissao: `finance.transactions.view`.
- `POST /api/financial/accounts/{accountId}/transactions`: cria lancamento. Permissao: `finance.transactions.manage`.
- `PUT /api/financial/transactions/{transactionId}`: atualiza lancamento. Permissao: `finance.transactions.manage`.
- `POST /api/financial/transactions/{transactionId}/void`: estorna lancamento. Permissao: `finance.transactions.manage`.

Request de criacao/atualizacao:

```json
{
  "description": "Recebimento de cliente",
  "amount": 1500.00,
  "type": 1,
  "occurredOn": "2026-05-10"
}
```

Valores de `type`:

- `1`: `Credit`.
- `2`: `Debit`.

Criar, atualizar ou estornar lancamentos recalcula o saldo atual da conta.
