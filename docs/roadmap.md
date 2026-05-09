# Roadmap de Desenvolvimento

Este roadmap define a ordem de execucao recomendada. Mudancas de ordem devem ser registradas em [decisions.md](decisions.md).

## Status

Legenda:

- `Nao iniciado`
- `Em andamento`
- `Bloqueado`
- `Concluido`

## Fase 1 - Fundacao da Plataforma

Status: `Em andamento`

Objetivo: criar a solution, projetos, referencias, pacotes, Docker Compose e API/Installer basicos.

Entregas:

- solution `WhiteLabelErpCrm.sln`;
- projetos `Platform.*`;
- projetos de testes;
- referencias entre projetos;
- pacotes NuGet principais;
- Docker Compose com SQL Server, API e Installer;
- health check inicial;
- `.env.example`;
- Dockerfiles da API e Installer.

## Fase 2 - Core Domain e Persistencia

Status: `Em andamento`

Objetivo: criar as entidades core, `AppDbContext`, configuracoes EF e primeira migration.

Entregas:

- entidades core;
- configuracoes `IEntityTypeConfiguration<T>`;
- indices unicos;
- filtro global por `TenantId` onde aplicavel;
- primeira migration `InitialCreate`;
- seeds iniciais para roles, permissions e modulos.

## Fase 3 - Provisioning Engine

Status: `Em andamento`

Objetivo: implementar os contratos e servicos responsaveis pela instalacao automatica.

Entregas:

- DTOs de instalacao;
- contratos do provisioning;
- `DatabaseProvisioner`;
- `MigrationRunner`;
- `SeedRunner`;
- `TenantProvisioner`;
- `ModuleInstaller`;
- `InstallerLockService`;
- `ProvisioningService`;
- fluxo idempotente onde fizer sentido.

## Fase 4 - Web Installer

Status: `Concluido`

Objetivo: expor endpoints para status, teste de banco e execucao da instalacao.

Entregas:

- `GET /install/status`;
- `POST /install/test-database`;
- `POST /install/run`;
- validacoes de entrada;
- bloqueio contra reinstalacao;
- respostas padronizadas.

## Fase 5 - Seguranca e Autenticacao

Status: `Nao iniciado`

Objetivo: implementar hash de senha, login JWT, roles, permissions e endpoint do usuario atual.

Entregas:

- hash seguro de senha;
- login JWT;
- claims com tenant e permissoes;
- `POST /api/auth/login`;
- `GET /api/me`;
- protecao basica de endpoints;
- CORS restritivo preparado.

## Fase 6 - Modulos Base

Status: `Nao iniciado`

Objetivo: registrar e ativar modulos por tenant.

Entregas:

- modulo `core`;
- modulo `crm`;
- modulo `finance`;
- `ModuleDefinition`;
- permissoes por modulo;
- `GET /api/modules`.

## Fase 7 - CRM Basico

Status: `Nao iniciado`

Objetivo: criar funcionalidades essenciais de CRM com isolamento por tenant.

Entregas:

- entidade `Customer`;
- entidade `Contact`;
- entidade `Opportunity`;
- CRUD basico;
- permissoes CRM;
- testes principais.

## Fase 8 - Financeiro Basico

Status: `Nao iniciado`

Objetivo: criar funcionalidades financeiras iniciais com isolamento por tenant.

Entregas:

- entidade `FinancialAccount`;
- entidade `FinancialTransaction`;
- CRUD basico;
- permissoes financeiras;
- testes principais.

## Fase 9 - Testes e Qualidade

Status: `Nao iniciado`

Objetivo: garantir que a fundacao funcione com testes unitarios, integracao, provisioning, migrations e arquitetura.

Entregas:

- testes unitarios;
- testes de integracao com SQL Server via Testcontainers;
- testes do Provisioning Engine;
- testes de migrations;
- testes de arquitetura com NetArchTest;
- `dotnet test` passando.

## Fase 10 - Documentacao Tecnica

Status: `Em andamento`

Objetivo: manter a documentacao operacional e tecnica alinhada com a implementacao.

Entregas:

- README principal;
- docs de arquitetura;
- docs de installer;
- docs de modulos;
- docs de banco;
- docs de testes;
- docs de Docker;
- docs de seguranca;
- roadmap atualizado.
