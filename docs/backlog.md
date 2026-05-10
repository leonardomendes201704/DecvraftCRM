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

Status: `Done`
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

- `[Done]` Implementar teste de conexao com banco.
- `[Done]` Implementar criacao de banco quando necessario.
- `[Done]` Implementar execucao de migrations.
- `[Done]` Implementar seeds globais.
- `[Done]` Implementar criacao de tenant.
- `[Done]` Implementar criacao de branding.
- `[Done]` Implementar criacao de admin com senha hasheada.
- `[Done]` Implementar instalacao de modulos.
- `[Done]` Implementar criacao de roles e permissions padrao.
- `[Done]` Implementar lock do instalador.
- `[Done]` Centralizar valores conhecidos de modulos, roles, permissoes e installer.
- `[Done]` Registrar DI inicial do Provisioning Engine.
- `[Done]` Cobrir servicos concretos iniciais com testes automatizados.
- `[Done]` Implementar orquestrador `ProvisioningService`.
- `[Done]` Cobrir orquestracao do provisioning com testes usando fakes.
- `[Done]` Implementar `DatabaseProvisioner` real com estrategia segura de bootstrap.
- `[Done]` Validar `DatabaseProvisioner` contra SQL Server real em Docker.

## EP-04 - Web Installer

Status: `Done`
Prioridade: `P0`

Objetivo: disponibilizar endpoints de instalacao controlados.

### UE-04.01 - Expor endpoints do installer

Tasks:

- `[Done]` Criar `GET /install/status`.
- `[Done]` Criar `POST /install/test-database`.
- `[Done]` Criar `POST /install/run`.
- `[Done]` Padronizar respostas iniciais de erro e sucesso.
- `[Done]` Validar bloqueio apos instalacao via endpoint.
- `[Done]` Validar fluxo completo via Docker.

## EP-05 - Autenticacao e Autorizacao

Status: `Doing`
Prioridade: `P0`

Objetivo: permitir login seguro e controle inicial por roles/permissoes.

### UE-05.01 - Login JWT

Tasks:

- `[Done]` Implementar hash seguro de senha.
- `[Done]` Criar servico de autenticacao.
- `[Done]` Configurar JWT via `SystemConfigurations`.
- `[Done]` Criar `POST /api/auth/login`.
- `[Done]` Criar `GET /api/me`.
- `[Done]` Incluir `TenantId` nas claims.

### UE-05.02 - RBAC inicial

Tasks:

- `[Done]` Criar seeds de roles.
- `[Done]` Criar seeds de permissions.
- `[Done]` Relacionar admin inicial ao papel `TenantAdmin`.
- `[Done]` Preparar policies basicas.

## EP-06 - Modulos Base

Status: `Done`
Prioridade: `P0`

Objetivo: registrar modulos e controlar ativacao por tenant.

### UE-06.01 - Registro dos modulos iniciais

Tasks:

- `[Done]` Criar metadados do modulo `core`.
- `[Done]` Criar metadados do modulo `crm`.
- `[Done]` Criar metadados do modulo `finance`.
- `[Done]` Criar `GET /api/modules`.
- `[Done]` Garantir ativacao por tenant.

## EP-07 - CRM Basico

Status: `Doing`
Prioridade: `P1`

Objetivo: entregar funcionalidades CRM iniciais.

### UE-07.01 - Clientes, contatos e oportunidades

Tasks:

- `[Done]` Criar entidade `Customer`.
- `[Done]` Criar entidade `Contact`.
- `[Done]` Criar entidade `Opportunity`.
- `[Done]` Criar configuracoes EF.
- `[Done]` Criar CRUD basico de clientes.
- `[Done]` Criar CRUD basico de contatos.
- `[Done]` Criar CRUD basico de oportunidades.
- `[Done]` Aplicar filtro por tenant.
- `[Done]` Criar migration inicial do CRM.
- `[Done]` Criar testes principais de dominio, modelo e filtro por tenant.

## EP-08 - Financeiro Basico

Status: `Done`
Prioridade: `P1`

Objetivo: entregar funcionalidades financeiras iniciais.

### UE-08.01 - Contas e lancamentos

Tasks:

- `[Done]` Criar entidade `FinancialAccount`.
- `[Done]` Criar entidade `FinancialTransaction`.
- `[Done]` Criar configuracoes EF.
- `[Done]` Criar migration inicial do financeiro.
- `[Done]` Criar CRUD basico de contas financeiras.
- `[Done]` Criar CRUD basico de lancamentos financeiros.
- `[Done]` Aplicar filtro por tenant.
- `[Done]` Criar testes principais.

## EP-09 - Testes Automatizados

Status: `Todo`
Prioridade: `P0`

Objetivo: garantir qualidade tecnica desde a fundacao.

### UE-09.01 - Testes principais do MVP

Tasks:

- `[Todo]` Testar criacao de tenant.
- `[Todo]` Testar validacao de slug.
- `[Todo]` Testar criacao de usuario.
- `[Done]` Testar regras de senha.
- `[Todo]` Testar migrations do zero.
- `[Todo]` Testar fluxo completo de provisioning.
- `[Done]` Testar login do admin apos instalacao em nivel de servico.
- `[Done]` Testar autorizacao basica por permissao.
- `[Done]` Testar consulta de modulos por tenant.
- `[Done]` Testar regras de arquitetura.

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

## EP-11 - Refatoracao Arquitetural da API

Status: `Done`
Prioridade: `P0`

Objetivo: evoluir a API de Minimal API concentrada no `Program.cs` para uma arquitetura hexagonal mais explicita, com separacao clara entre entrada HTTP, casos de uso, modelos, servicos e infraestrutura.

### UE-11.01 - Modularizar endpoints HTTP

Tasks:

- `[Done]` Remover concentracao de endpoints do `Program.cs`.
- `[Done]` Criar organizacao por grupos/modulos de endpoints.
- `[Done]` Manter `Program.cs` apenas para bootstrap, DI, middleware e mapeamento de modulos.
- `[Done]` Garantir que autorizacao por permissao continue aplicada por endpoint.

### UE-11.02 - Introduzir MediatR e casos de uso

Tasks:

- `[Done]` Adicionar MediatR ao projeto de aplicacao.
- `[Done]` Criar commands/queries iniciais para autenticacao e modulos.
- `[Done]` Criar commands/queries para clientes do CRM.
- `[Done]` Criar commands/queries para contatos do CRM.
- `[Done]` Criar commands/queries para oportunidades do CRM.
- `[Done]` Criar commands/queries para financeiro.
- `[Done]` Migrar regras de orquestracao de services diretos para handlers.
- `[Done]` Padronizar responses/resultados de handlers.
- `[Done]` Criar behaviors para logging e tratamento de erros.

### UE-11.03 - Reforcar arquitetura hexagonal

Tasks:

- `[Done]` Definir portas de entrada e saida na camada Application.
- `[Done]` Manter infraestrutura como adapters externos.
- `[Done]` Isolar controllers/endpoints como adapters HTTP.
- `[Done]` Revisar dependencias entre projetos para evitar acoplamento indevido.
- `[Done]` Adicionar testes de arquitetura para validar as regras.

## EP-12 - Evolucao Comercial do CRM

Status: `Done`
Prioridade: `P1`

Objetivo: ampliar oportunidades para um fluxo comercial acompanhavel, com pipeline, atividades e historico.

### UE-12.01 - Pipeline de oportunidades

Tasks:

- `[Done]` Criar entidade `OpportunityStage`.
- `[Done]` Permitir listar, criar, atualizar e desativar etapas por tenant.
- `[Done]` Permitir associar oportunidade a uma etapa.
- `[Done]` Permitir mover oportunidade entre etapas.
- `[Done]` Criar migration de pipeline comercial.

### UE-12.02 - Atividades e historico de oportunidades

Tasks:

- `[Done]` Criar entidade `OpportunityActivity`.
- `[Done]` Criar entidade `OpportunityHistoryEntry`.
- `[Done]` Permitir listar, criar, atualizar, concluir e cancelar atividades.
- `[Done]` Registrar historico automatico para criacao, atualizacao, mudanca de etapa, status e atividades.
- `[Done]` Expor leitura do historico por oportunidade.
- `[Done]` Cobrir valores fechados do CRM com testes de dominio.

### UE-12.03 - Visao operacional do pipeline

Tasks:

- `[Done]` Listar oportunidades por etapa do pipeline.
- `[Done]` Listar atividades vencidas do tenant.
- `[Done]` Listar proximas atividades do tenant com janela configuravel.
- `[Done]` Manter consultas agregadas via handlers MediatR e portas da Application.

## EP-13 - Organizacao, Pessoas e Responsaveis

Status: `Done`
Prioridade: `P1`

Objetivo: criar a base organizacional para relacionar usuarios, funcionarios, cargos, departamentos, hierarquias e responsaveis de negocio.

### UE-13.01 - Estrutura organizacional base

Tasks:

- `[Done]` Criar entidade `Department`.
- `[Done]` Criar entidade `JobTitle`.
- `[Done]` Criar entidade `Employee`.
- `[Done]` Criar enum valorado `EmployeeStatus`.
- `[Done]` Configurar indices unicos por tenant para codigos, emails e vinculo usuario-funcionario.
- `[Done]` Configurar hierarquia direta por `ManagerEmployeeId`.
- `[Done]` Criar migration da estrutura organizacional.

### UE-13.02 - Casos de uso e endpoints Core

Tasks:

- `[Done]` Criar portas, commands/queries e handlers para departamentos.
- `[Done]` Criar portas, commands/queries e handlers para cargos.
- `[Done]` Criar portas, commands/queries e handlers para funcionarios.
- `[Done]` Criar endpoints HTTP usando MediatR.
- `[Done]` Criar permissoes centralizadas para departamentos, cargos, funcionarios e hierarquia.
- `[Done]` Cobrir regras principais com testes automatizados.

### UE-13.03 - Vinculo usuario-funcionario

Tasks:

- `[Done]` Permitir vincular um `ApplicationUser` a um `Employee`.
- `[Done]` Permitir desvincular usuario e funcionario sem apagar historico.
- `[Done]` Validar unicidade do vinculo por tenant.
- `[Done]` Atualizar `GET /api/me` para retornar dados do funcionario vinculado quando existir.

### UE-13.04 - Responsaveis no CRM

Tasks:

- `[Done]` Adicionar `OwnerEmployeeId` em `Opportunity`.
- `[Done]` Adicionar `OwnerEmployeeId` em `OpportunityActivity`.
- `[Done]` Permitir alterar responsavel da oportunidade.
- `[Done]` Permitir alterar responsavel da atividade.
- `[Done]` Registrar historico quando responsavel for alterado.
- `[Done]` Atualizar listagens de oportunidades e atividades com filtro por responsavel.

### UE-13.05 - Visoes gerenciais por responsavel

Tasks:

- `[Done]` Criar resumo de carteira por responsavel.
- `[Done]` Criar total de oportunidades por responsavel.
- `[Done]` Criar valor estimado de oportunidades abertas por responsavel.
- `[Done]` Criar atividades vencidas agrupadas por responsavel.
- `[Done]` Criar proximas atividades agrupadas por responsavel.
- `[Done]` Criar funil comercial por equipe/departamento.
- `[Done]` Criar endpoints agregados para dashboard comercial.
- `[Done]` Atualizar documentacao de CRM e organizacao com os endpoints agregados.

## EP-14 - Frontend Web Installer

Status: `Done`
Prioridade: `P0`

Objetivo: criar um wizard visual em Razor Pages para executar a instalacao inicial da plataforma pelo navegador.

### UE-14.01 - Estrutura visual do installer

Tasks:

- `[Done]` Criar layout base do Web Installer.
- `[Done]` Criar navegacao por etapas.
- `[Done]` Criar componente de indicador de progresso.
- `[Done]` Criar estado temporario do wizard.

### UE-14.02 - Etapas do wizard

Tasks:

- `[Done]` Criar etapa de boas-vindas/status.
- `[Done]` Criar etapa de banco de dados com teste de conexao.
- `[Done]` Criar etapa de tenant inicial.
- `[Done]` Criar etapa de administrador.
- `[Done]` Criar etapa de branding.
- `[Done]` Criar etapa de modulos.
- `[Done]` Criar etapa de revisao.
- `[Done]` Criar etapa de conclusao.

### UE-14.03 - Execucao da instalacao pela UI

Tasks:

- `[Done]` Integrar wizard ao ProvisioningService.
- `[Done]` Exibir erros de validacao por etapa.
- `[Done]` Exibir resultado final da instalacao.
- `[Done]` Bloquear wizard quando instalacao ja estiver concluida.
- `[Done]` Limpar estado temporario apos instalacao.

## EP-15 - Frontend Web MVC/Razor

Status: `Todo`
Prioridade: `P1`

Objetivo: criar a aplicacao web autenticada do ERP/CRM usando ASP.NET Core MVC + Razor Pages, componentes reutilizaveis, services e API clients.

### UE-15.01 - Estrutura base do Platform.Web

Tasks:

- `[Todo]` Criar projeto `Platform.Web`.
- `[Todo]` Configurar referencias e Docker quando necessario.
- `[Todo]` Criar layout autenticado.
- `[Todo]` Criar estrutura de Controllers, Pages, Views, ViewComponents, Services, Clients e ViewModels.

### UE-15.02 - Autenticacao web

Tasks:

- `[Todo]` Criar tela de login.
- `[Todo]` Criar logout.
- `[Todo]` Consumir `POST /api/auth/login`.
- `[Todo]` Consumir `GET /api/me`.
- `[Todo]` Manter sessao web via cookie seguro.
- `[Todo]` Criar contexto web do usuario autenticado.

### UE-15.03 - Dashboard inicial

Tasks:

- `[Todo]` Criar dashboard autenticado.
- `[Todo]` Exibir atividades vencidas.
- `[Todo]` Exibir proximas atividades.
- `[Todo]` Exibir oportunidades por etapa.
- `[Todo]` Exibir filtros por responsavel quando disponivel.

### UE-15.04 - Telas operacionais iniciais

Tasks:

- `[Todo]` Criar telas de organizacao.
- `[Todo]` Criar telas de CRM.
- `[Todo]` Criar telas de financeiro.
- `[Todo]` Criar componentes reutilizaveis de tabela, filtros, cards e acoes.
