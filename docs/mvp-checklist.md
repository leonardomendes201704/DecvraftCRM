# Checklist do MVP Tecnico

Este checklist define a primeira entrega valida do projeto.

Uma entrega so deve ser considerada concluida quando todos os itens P0 aplicaveis estiverem marcados.

## Fundacao

- `[x]` A solution `WhiteLabelErpCrm.sln` existe.
- `[x]` Todos os projetos `Platform.*` foram criados.
- `[x]` Todos os projetos de teste foram criados.
- `[x]` As referencias entre projetos respeitam a arquitetura definida.
- `[x]` `dotnet build` executa com sucesso.

## Docker

- `[x]` `docker-compose.yml` existe.
- `[x]` SQL Server sobe em container.
- `[x]` API sobe em container.
- `[x]` Installer sobe em container.
- `[x]` Volume persistente do SQL Server esta configurado.
- `[x]` `.env.example` existe e nao contem segredo real.

## API e Health Checks

- `[ ]` `GET /health` responde com sucesso.
- `[ ]` `GET /health/database` responde corretamente.
- `[ ]` `GET /health/modules` responde corretamente.
- `[ ]` `GET /health/migrations` responde corretamente.

## Persistencia

- `[x]` `AppDbContext` existe.
- `[x]` Entidades core foram criadas.
- `[x]` Configuracoes EF foram criadas para entidades core.
- `[x]` Indices unicos foram configurados.
- `[x]` Filtro global por `TenantId` foi aplicado onde necessario.
- `[x]` Migration `InitialCreate` foi criada.
- `[x]` Migration executa em banco SQL Server limpo.

## Installer

- `[x]` `GET /install/status` retorna `isInstalled = false` antes da instalacao.
- `[x]` Motor interno de teste de conexao com SQL Server foi implementado e validado.
- `[x]` `POST /install/test-database` valida conexao via Web Installer.
- `[x]` `POST /install/run` executa instalacao completa.
- `[x]` Tenant inicial e criado.
- `[x]` Branding inicial e criado.
- `[x]` Usuario admin e criado com senha hasheada.
- `[x]` Modulos `core`, `crm` e `finance` sao registrados.
- `[x]` Modulos selecionados sao ativados para o tenant.
- `[x]` Roles e permissions iniciais sao criadas.
- `[x]` Instalador e bloqueado apos instalacao.
- `[x]` `GET /install/status` retorna `isInstalled = true` apos instalacao.

## Autenticacao

- `[x]` `POST /api/auth/login` autentica usuario ativo por tenant.
- `[x]` JWT contem dados minimos esperados.
- `[x]` `GET /api/me` retorna dados do usuario autenticado.
- `[x]` Senha nao e salva em texto puro.
- `[x]` Endpoints podem exigir permissao centralizada.

## Modulos

- `[x]` Metadados dos modulos `core`, `crm` e `finance` existem.
- `[x]` `GET /api/modules` retorna modulos globais com status do tenant.
- `[x]` Ativacao por tenant e considerada na resposta.
- `[x]` Consulta de modulos exige permissao `core.modules.view`.

## CRM

- `[x]` Entidade `Customer` existe.
- `[x]` Entidade `Contact` existe.
- `[x]` Entidade `Opportunity` existe.
- `[x]` Entidades CRM usam enums valorados para status/tipos fechados.
- `[x]` Entidades CRM possuem configuracoes EF.
- `[x]` Entidades CRM respeitam filtro global por tenant.
- `[x]` CRUD basico de clientes existe.
- `[x]` CRUD basico de contatos existe.
- `[x]` CRUD basico de oportunidades existe.

## Financeiro

- `[x]` Entidade `FinancialAccount` existe.
- `[x]` Entidade `FinancialTransaction` existe.
- `[x]` Entidades financeiras usam enums valorados para status/tipos fechados.
- `[x]` Entidades financeiras possuem configuracoes EF.
- `[x]` Entidades financeiras respeitam filtro global por tenant.
- `[x]` CRUD basico de contas financeiras existe.
- `[x]` CRUD basico de lancamentos financeiros existe.

## Testes

- `[x]` Testes unitarios principais passam.
- `[ ]` Testes de integracao com SQL Server passam.
- `[x]` Teste do fluxo de provisioning passa.
- `[x]` Teste de migrations passa.
- `[x]` Testes de arquitetura passam.
- `[x]` `dotnet test` executa com sucesso.

## Documentacao

- `[ ]` README principal existe.
- `[ ]` Documentacao de arquitetura existe.
- `[ ]` Documentacao do fluxo do installer existe.
- `[ ]` Documentacao de modulos existe.
- `[ ]` Documentacao de banco existe.
- `[ ]` Documentacao de testes existe.
- `[ ]` Documentacao de Docker existe.
- `[x]` Documentacao de seguranca existe.
