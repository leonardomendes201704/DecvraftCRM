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
- `[ ]` SQL Server sobe em container.
- `[ ]` API sobe em container.
- `[ ]` Installer sobe em container.
- `[x]` Volume persistente do SQL Server esta configurado.
- `[x]` `.env.example` existe e nao contem segredo real.

## API e Health Checks

- `[ ]` `GET /health` responde com sucesso.
- `[ ]` `GET /health/database` responde corretamente.
- `[ ]` `GET /health/modules` responde corretamente.
- `[ ]` `GET /health/migrations` responde corretamente.

## Persistencia

- `[ ]` `AppDbContext` existe.
- `[ ]` Entidades core foram criadas.
- `[ ]` Configuracoes EF foram criadas para entidades core.
- `[ ]` Indices unicos foram configurados.
- `[ ]` Filtro global por `TenantId` foi aplicado onde necessario.
- `[ ]` Migration `InitialCreate` foi criada.
- `[ ]` Migration executa em banco SQL Server limpo.

## Installer

- `[ ]` `GET /install/status` retorna `isInstalled = false` antes da instalacao.
- `[ ]` `POST /install/test-database` valida conexao com SQL Server.
- `[ ]` `POST /install/run` executa instalacao completa.
- `[ ]` Tenant inicial e criado.
- `[ ]` Branding inicial e criado.
- `[ ]` Usuario admin e criado com senha hasheada.
- `[ ]` Modulos `core`, `crm` e `finance` sao registrados.
- `[ ]` Modulos selecionados sao ativados para o tenant.
- `[ ]` Roles e permissions iniciais sao criadas.
- `[ ]` Instalador e bloqueado apos instalacao.
- `[ ]` `GET /install/status` retorna `isInstalled = true` apos instalacao.

## Autenticacao

- `[ ]` `POST /api/auth/login` autentica o admin criado.
- `[ ]` JWT contem dados minimos esperados.
- `[ ]` `GET /api/me` retorna dados do usuario autenticado.
- `[ ]` Senha nao e salva em texto puro.

## Testes

- `[ ]` Testes unitarios principais passam.
- `[ ]` Testes de integracao com SQL Server passam.
- `[ ]` Teste do fluxo de provisioning passa.
- `[ ]` Teste de migrations passa.
- `[ ]` Testes de arquitetura passam.
- `[ ]` `dotnet test` executa com sucesso.

## Documentacao

- `[ ]` README principal existe.
- `[ ]` Documentacao de arquitetura existe.
- `[ ]` Documentacao do fluxo do installer existe.
- `[ ]` Documentacao de modulos existe.
- `[ ]` Documentacao de banco existe.
- `[ ]` Documentacao de testes existe.
- `[ ]` Documentacao de Docker existe.
- `[ ]` Documentacao de seguranca existe.
