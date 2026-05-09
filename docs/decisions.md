# Registro de Decisoes Tecnicas

Este arquivo registra decisoes que afetam arquitetura, escopo, seguranca, persistencia, deploy ou testes.

Formato:

```text
## ADR-000X - Titulo

Data: YYYY-MM-DD
Status: Proposta | Aceita | Substituida

Contexto:

Decisao:

Consequencias:
```

## ADR-0001 - Arquitetura modular por camadas

Data: 2026-05-09
Status: Aceita

Contexto:

O projeto precisa nascer como plataforma white-label modular, e nao como CRUD monolitico concentrado na API.

Decisao:

Usar uma solution .NET organizada em projetos separados: `Domain`, `Application`, `Infrastructure`, `Persistence`, `Provisioning`, `Modules`, `Api`, `WebInstaller` e `Worker` opcional.

Consequencias:

- Controllers nao devem concentrar regra de negocio.
- Dependencias entre projetos devem respeitar a arquitetura.
- Testes de arquitetura devem validar essas regras.

## ADR-0002 - Multi-tenancy inicial com banco unico e TenantId

Data: 2026-05-09
Status: Aceita

Contexto:

A plataforma precisa suportar multiplos clientes desde o inicio, mas sem complexidade inicial de banco por tenant.

Decisao:

Comecar com banco unico e `TenantId` em todas as tabelas de negocio.

Consequencias:

- Entidades de negocio devem incluir `TenantId`.
- O EF Core deve aplicar filtro global por tenant onde aplicavel.
- Testes devem validar isolamento por tenant.
- A arquitetura deve permitir evolucao futura para banco por tenant.

## ADR-0003 - Installer bloqueado por banco e lock

Data: 2026-05-09
Status: Aceita

Contexto:

O instalador nao pode permanecer aberto apos a configuracao inicial, para evitar reinstalacao acidental ou ataque.

Decisao:

Bloquear instalador usando registro em `SystemInstallations` e arquivo/logica de lock, por exemplo `installer.lock`.

Consequencias:

- `GET /install/status` deve consultar o estado real da instalacao.
- `POST /install/run` deve impedir reinstalacao.
- Testes devem cobrir estado antes e depois da instalacao.

## ADR-0004 - MVP tecnico prioriza Provisioning Engine

Data: 2026-05-09
Status: Aceita

Contexto:

CRM e Financeiro dependem de tenant, usuario, modulos, permissoes, banco e instalacao funcionando.

Decisao:

Priorizar o MVP tecnico de provisioning antes de funcionalidades amplas de negocio.

Consequencias:

- Fases P0 vem antes de CRM/Financeiro completos.
- A primeira entrega valida e instalar, bloquear installer e autenticar admin.
- Funcionalidades P1 so devem avancar apos a fundacao estar estavel.
