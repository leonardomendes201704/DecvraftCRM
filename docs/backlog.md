# Backlog Controlado

Este backlog organiza o desenvolvimento por Epicos, UEs e Tasks.

Status permitidos:

- `Todo`
- `Doing`
- `Review`
- `Done`
- `Blocked`

Prioridade:

- `P0`: essencial para o MVP tecnico;
- `P1`: importante para primeira versao funcional;
- `P2`: melhoria ou expansao posterior.

## EP-01 - Fundacao da Plataforma

Status: `Doing`
Prioridade: `P0`

Objetivo: criar a base tecnica compilavel e executavel via Docker.

### UE-01.01 - Criar solution modular

Como desenvolvedor, quero uma solution .NET organizada por camadas para que a plataforma nasca modular e testavel.

Tasks:

- `[Done]` Criar `WhiteLabelErpCrm.sln`.
- `[Done]` Fixar SDK .NET 8 com `global.json`.
- `[Done]` Criar pasta `src`.
- `[Done]` Criar pasta `tests`.
- `[Done]` Criar projetos `Platform.Api`, `Platform.WebInstaller`, `Platform.Domain`, `Platform.Application`, `Platform.Infrastructure`, `Platform.Persistence`, `Platform.Provisioning`, `Platform.Modules.Core`, `Platform.Modules.Crm`, `Platform.Modules.Finance` e `Platform.Worker`.
- `[Done]` Criar projetos de teste `Platform.UnitTests`, `Platform.IntegrationTests`, `Platform.ProvisioningTests`, `Platform.MigrationTests` e `Platform.ArchitectureTests`.
- `[Done]` Adicionar todos os projetos na solution.

### UE-01.02 - Configurar referencias entre projetos

Como desenvolvedor, quero referencias coerentes entre camadas para preservar a arquitetura limpa/modular.

Tasks:

- `[Done]` Referenciar `Domain` em `Application`.
- `[Done]` Referenciar `Domain` e `Application` em `Persistence`.
- `[Done]` Referenciar `Domain` e `Application` em `Infrastructure`.
- `[Done]` Referenciar `Domain`, `Application`, `Persistence` e `Infrastructure` em `Provisioning`.
- `[Done]` Referenciar camadas necessarias na `Api`.
- `[Done]` Referenciar dependencias necessarias no `WebInstaller`.

### UE-01.03 - Configurar Docker inicial

Como operador, quero subir SQL Server, API e Installer via Docker Compose para validar o ambiente self-hosted.

Tasks:

- `[Done]` Criar `docker-compose.yml`.
- `[Done]` Criar `docker/Dockerfile.api`.
- `[Done]` Criar `docker/Dockerfile.installer`.
- `[Done]` Criar volume persistente para SQL Server.
- `[Done]` Criar `.env.example`.
- `[Done]` Criar `.dockerignore`.
- `[Done]` Validar build das imagens Docker localmente.
- `[Done]` Validar subida dos containers SQL Server, API e Installer.

## EP-02 - Core Domain e Persistencia

Status: `Doing`
Prioridade: `P0`

Objetivo: implementar entidades centrais, DbContext, mapeamentos EF e migration inicial.

### UE-02.01 - Criar entidades core

Tasks:

- `[Done]` Criar `SystemInstallation`.
- `[Done]` Criar `Tenant`.
- `[Done]` Criar `TenantBranding`.
- `[Done]` Criar `Module`.
- `[Done]` Criar `TenantModule`.
- `[Done]` Criar `ApplicationUser`.
- `[Done]` Criar `Role`.
- `[Done]` Criar `Permission`.
- `[Done]` Criar entidades de relacionamento de usuarios, roles e permissions quando necessario.
- `[Done]` Criar entidades `SystemConfiguration` e `TenantConfiguration` para governanca de configuracoes.

### UE-02.02 - Configurar persistencia

Tasks:

- `[Done]` Criar `AppDbContext`.
- `[Done]` Criar configuracoes EF por entidade.
- `[Done]` Configurar tabelas no plural.
- `[Done]` Configurar indices unicos.
- `[Done]` Configurar filtro global por `TenantId` para entidades multi-tenant.
- `[Done]` Criar extensao `AddPersistence()`.

### UE-02.03 - Criar migration inicial

Tasks:

- `[Done]` Instalar pacotes EF Core.
- `[Done]` Criar migration `InitialCreate`.
- `[Done]` Validar `dotnet build`.
- `[Done]` Validar aplicacao da migration em banco SQL Server.

## EP-03 - Provisioning Engine

Status: `Doing`
Prioridade: `P0`

Objetivo: implementar o motor de instalacao automatica.

### UE-03.01 - Criar contratos e DTOs

Tasks:

- `[Done]` Criar `InstallRequest`.
- `[Done]` Criar `DatabaseSetupOptions`.
- `[Done]` Criar `TenantSetupOptions`.
- `[Done]` Criar `AdminUserSetupOptions`.
- `[Done]` Criar `BrandingSetupOptions`.
- `[Done]` Criar `ProvisioningResult`.
- `[Done]` Criar `InstallStatusResult`.
- `[Done]` Criar interfaces do provisioning.
- `[Done]` Criar teste de contrato dos modelos iniciais.

### UE-03.02 - Implementar fluxo de instalacao

Tasks:

- `[Todo]` Implementar teste de conexao com banco.
- `[Todo]` Implementar criacao de banco quando necessario.
- `[Todo]` Implementar execucao de migrations.
- `[Todo]` Implementar seeds globais.
- `[Todo]` Implementar criacao de tenant.
- `[Todo]` Implementar criacao de branding.
- `[Todo]` Implementar criacao de admin.
- `[Todo]` Implementar instalacao de modulos.
- `[Todo]` Implementar criacao de roles e permissions padrao.
- `[Todo]` Implementar lock do instalador.

## EP-04 - Web Installer

Status: `Todo`
Prioridade: `P0`

Objetivo: disponibilizar endpoints de instalacao controlados.

### UE-04.01 - Expor endpoints do installer

Tasks:

- `[Todo]` Criar `GET /install/status`.
- `[Todo]` Criar `POST /install/test-database`.
- `[Todo]` Criar `POST /install/run`.
- `[Todo]` Validar bloqueio apos instalacao.
- `[Todo]` Padronizar respostas de erro e sucesso.

## EP-05 - Autenticacao e Autorizacao

Status: `Todo`
Prioridade: `P0`

Objetivo: permitir login seguro e controle inicial por roles/permissoes.

### UE-05.01 - Login JWT

Tasks:

- `[Todo]` Implementar hash seguro de senha.
- `[Todo]` Criar servico de autenticacao.
- `[Todo]` Configurar JWT.
- `[Todo]` Criar `POST /api/auth/login`.
- `[Todo]` Criar `GET /api/me`.
- `[Todo]` Incluir `TenantId` nas claims.

### UE-05.02 - RBAC inicial

Tasks:

- `[Todo]` Criar seeds de roles.
- `[Todo]` Criar seeds de permissions.
- `[Todo]` Relacionar admin inicial ao papel `TenantAdmin`.
- `[Todo]` Preparar policies basicas.

## EP-06 - Modulos Base

Status: `Todo`
Prioridade: `P0`

Objetivo: registrar modulos e controlar ativacao por tenant.

### UE-06.01 - Registro dos modulos iniciais

Tasks:

- `[Todo]` Criar metadados do modulo `core`.
- `[Todo]` Criar metadados do modulo `crm`.
- `[Todo]` Criar metadados do modulo `finance`.
- `[Todo]` Criar `GET /api/modules`.
- `[Todo]` Garantir ativacao por tenant.

## EP-07 - CRM Basico

Status: `Todo`
Prioridade: `P1`

Objetivo: entregar funcionalidades CRM iniciais.

### UE-07.01 - Clientes, contatos e oportunidades

Tasks:

- `[Todo]` Criar entidade `Customer`.
- `[Todo]` Criar entidade `Contact`.
- `[Todo]` Criar entidade `Opportunity`.
- `[Todo]` Criar configuracoes EF.
- `[Todo]` Criar CRUD basico.
- `[Todo]` Aplicar filtro por tenant.
- `[Todo]` Criar testes principais.

## EP-08 - Financeiro Basico

Status: `Todo`
Prioridade: `P1`

Objetivo: entregar funcionalidades financeiras iniciais.

### UE-08.01 - Contas e lancamentos

Tasks:

- `[Todo]` Criar entidade `FinancialAccount`.
- `[Todo]` Criar entidade `FinancialTransaction`.
- `[Todo]` Criar configuracoes EF.
- `[Todo]` Criar CRUD basico.
- `[Todo]` Aplicar filtro por tenant.
- `[Todo]` Criar testes principais.

## EP-09 - Testes Automatizados

Status: `Todo`
Prioridade: `P0`

Objetivo: garantir qualidade tecnica desde a fundacao.

### UE-09.01 - Testes principais do MVP

Tasks:

- `[Todo]` Testar criacao de tenant.
- `[Todo]` Testar validacao de slug.
- `[Todo]` Testar criacao de usuario.
- `[Todo]` Testar regras de senha.
- `[Todo]` Testar migrations do zero.
- `[Todo]` Testar fluxo completo de provisioning.
- `[Todo]` Testar login do admin apos instalacao.
- `[Todo]` Testar regras de arquitetura.

## EP-10 - Documentacao Tecnica

Status: `Done`
Prioridade: `P0`

Objetivo: manter desenvolvimento guiado por documentacao.

### UE-10.01 - Controle de projeto

Tasks:

- `[Done]` Criar indice da documentacao.
- `[Done]` Criar visao do projeto.
- `[Done]` Criar roadmap.
- `[Done]` Criar backlog controlado.
- `[Done]` Criar checklist do MVP.
- `[Done]` Criar processo de desenvolvimento.
- `[Done]` Criar registro de decisoes.
- `[Done]` Criar definition of done.
