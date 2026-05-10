# Frontend Web MVC/Razor

Este documento define a arquitetura do frontend autenticado da plataforma ERP/CRM.

## Objetivo

Criar uma aplicacao web server-side em ASP.NET Core MVC + Razor Pages, com componentes reutilizaveis, services de tela e consumo da API.

O frontend deve ser a experiencia operacional do usuario final apos a instalacao.

## Projeto

Projeto alvo sugerido:

- `src/Platform.Web`

Estrutura recomendada:

```text
Platform.Web/
  Controllers/
    DashboardController.cs
    CrmController.cs
    FinanceController.cs
    OrganizationController.cs
  Pages/
    Account/
      Login.cshtml
      Logout.cshtml
  Views/
    Shared/
      _Layout.cshtml
      _ValidationScriptsPartial.cshtml
    Dashboard/
    Crm/
    Finance/
    Organization/
  ViewComponents/
    SidebarNavigationViewComponent.cs
    TopbarViewComponent.cs
    PermissionGateViewComponent.cs
    DataTableViewComponent.cs
    KpiCardViewComponent.cs
  Services/
    AuthWebService.cs
    CrmWebService.cs
    FinanceWebService.cs
    OrganizationWebService.cs
  Clients/
    PlatformApiClient.cs
    AuthApiClient.cs
    CrmApiClient.cs
    FinanceApiClient.cs
    OrganizationApiClient.cs
  ViewModels/
    Account/
    Dashboard/
    Crm/
    Finance/
    Organization/
  Security/
    WebUserContext.cs
    PermissionViewPolicy.cs
```

## Padrao Arquitetural

Fluxo recomendado:

```text
Controller ou Razor Page
  -> Web Service
    -> API Client tipado
      -> Platform.Api
        -> MediatR
          -> Domain/Infrastructure
```

Regras:

- `Platform.Web` nao deve acessar banco diretamente.
- Regras de negocio permanecem na API/Application/Domain.
- Controllers e Pages devem lidar com fluxo de tela, validacao de entrada visual e composicao de ViewModels.
- Services do frontend orquestram chamadas para API e preparam ViewModels.
- API Clients encapsulam HTTP, serializacao e tratamento de status code.
- ViewModels sao especificos de tela e nao devem expor DTO cru sem necessidade.

## MVC x Razor Pages

Usar MVC Controllers para areas com navegacao funcional:

- Dashboard.
- CRM.
- Financeiro.
- Organizacao.
- Relatorios.

Usar Razor Pages para fluxos simples e autocontidos:

- Login.
- Logout.
- Recuperacao de senha futura.
- Preferencias do usuario futura.

## Componentes Reutilizaveis

Componentes iniciais:

- Menu lateral por permissao.
- Topbar com usuario/tenant.
- Cards de KPI.
- Tabela padrao com acoes.
- Breadcrumb.
- Empty state.
- Alertas de erro/sucesso.
- Kanban de oportunidades.
- Filtros de periodo/responsavel.
- Select de funcionario/responsavel.

## Autenticacao Web

Modelo recomendado:

- Login no `Platform.Web` chama `POST /api/auth/login`.
- Web App guarda a sessao em cookie seguro.
- Token de API deve ser protegido no servidor, evitando exposicao desnecessaria no browser.
- Requests dos API Clients enviam bearer token para `Platform.Api`.
- `GET /api/me` carrega usuario atual, permissoes e funcionario vinculado.
- A tela de login recebe o endereco da API em runtime para evitar configuracao em arquivo.
- O endereco da API fica limitado a sessao autenticada e nao e persistido em `appsettings`, env ou codigo fonte.

Regras:

- Nao armazenar credenciais em appsettings/env.
- Configuracoes definitivas continuam em banco.
- UI deve esconder acoes sem permissao, mas a API continua sendo a barreira real.

## Areas Funcionais Iniciais

### Account

- Login.
- Logout.
- Exibir erro de credenciais invalidas.

### Dashboard

- KPIs basicos.
- Atividades vencidas.
- Proximas atividades.
- Oportunidades por etapa.

### Organizacao

- Departamentos.
- Cargos.
- Funcionarios.
- Vinculo usuario-funcionario.
- Subordinados diretos.

### CRM

- Clientes.
- Contatos.
- Oportunidades.
- Pipeline por etapa.
- Atividades.
- Historico.
- Responsaveis.

### Financeiro

- Contas financeiras.
- Lancamentos.
- Saldos.

## Design

Direcao visual:

- Aplicacao operacional, densa e clara.
- Evitar visual de landing page.
- Priorizar leitura, filtros, tabelas, formularios e fluxo de trabalho.
- Usar componentes consistentes.
- Cards apenas para itens repetidos, KPIs ou areas realmente destacadas.
- Navegacao previsivel.

## Criterios de Aceite

- Usuario consegue logar pela UI.
- Layout autenticado exibe tenant, usuario e navegacao.
- Menus respeitam permissoes.
- Telas consomem a API, sem acesso direto ao banco.
- Erros da API sao traduzidos para mensagens de tela.
- ViewModels separados dos DTOs de API quando houver composicao de tela.
- `dotnet build` e `dotnet test` continuam passando.

## Ordem Recomendada

1. Criar projeto `Platform.Web`. `[Done]`
2. Configurar layout base autenticado. `[Done]`
3. Implementar login/logout.
4. Criar API clients tipados.
5. Criar dashboard inicial.
6. Criar telas de Organizacao.
7. Criar telas de CRM.
8. Criar telas de Financeiro.

## Status Atual

- UE-15.01 concluida com estrutura base MVC/Razor em `src/Platform.Web`.
- UE-15.02 concluida com login real via `POST /api/auth/login`, validacao via `GET /api/me`, cookie de sessao e logout real.
- Projeto adicionado a solution principal.
- Layout operacional inicial criado com sidebar, topbar, componentes de KPI e tabela.
- Controllers, Razor Pages, Services, Clients e ViewModels foram criados como base para as proximas UEs.
- Controllers operacionais exigem usuario autenticado.

## Fora do Primeiro Bloco

- SPA frontend.
- Blazor.
- Mobile app.
- Tema white-label completo em runtime.
- Editor visual de layout.
- Relatorios avancados.
