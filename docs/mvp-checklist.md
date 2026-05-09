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
