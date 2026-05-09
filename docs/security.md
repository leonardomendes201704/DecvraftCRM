# Seguranca

Este documento controla as decisoes e validacoes de seguranca implementadas.

## Autenticacao JWT

- O login inicial e feito por `POST /api/auth/login`.
- A consulta do usuario autenticado e feita por `GET /api/me`.
- O login exige `tenantSlug`, `email` e `password`.
- O token JWT inclui identificacao do usuario, tenant, roles e permissoes.
- As chaves de claims customizadas ficam centralizadas em `KnownAuthClaimTypes`.

## Autorizacao por permissoes

- Endpoints podem exigir permissao com `RequirePermission()`.
- A autorizacao retorna estados valorados por `PermissionAuthorizationStatus`.
- Permissoes conhecidas ficam centralizadas em `KnownPermissions`.
- O primeiro endpoint protegido por permissao e `GET /api/system/permissions`, que exige `core.system.view`.

## Configuracoes

- Configuracoes definitivas de JWT sao persistidas em `SystemConfigurations`.
- O seed do installer cria as configuracoes JWT e o servico de token tambem cria registros ausentes de forma idempotente para ambientes ja instalados.
- As chaves conhecidas ficam centralizadas em `KnownSystemConfigurationKeys`.
- Valores padrao de emissor, audiencia, expiracao e tamanho do segredo ficam centralizados em `JwtConfigurationDefaults`.
- Arquivos de configuracao e variaveis de ambiente podem existir apenas como bootstrap operacional, nunca como fonte definitiva de chaves ou credenciais da aplicacao.

## Senhas

- Senhas sao armazenadas como hash PBKDF2-SHA256.
- Parametros de hashing ficam centralizados em `PasswordHashingDefaults`.
- O formato atual e `PBKDF2-SHA256$iterations$saltBase64$hashBase64`.

## Validacao

- `dotnet test WhiteLabelErpCrm.sln` deve passar.
- Testes unitarios cobrem verificacao de senha, login, retorno do usuario atual via token e autorizacao por permissao.
