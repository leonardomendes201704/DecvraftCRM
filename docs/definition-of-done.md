# Definition of Done

Uma task so pode ser marcada como `Done` quando os criterios abaixo forem atendidos, quando aplicaveis.

## Criterios gerais

- Codigo compila.
- A implementacao esta ligada a uma task documentada.
- Nao ha regra de negocio nova diretamente em controller.
- Nao ha segredo real versionado.
- Nao ha senha salva em texto puro.
- Chaves, credenciais e configuracoes definitivas sao gerenciadas por entidades em banco, nao por `.env`, `appsettings*.json` ou hardcoded.
- Nao ha quebra consciente de isolamento por tenant.
- Documentacao afetada foi atualizada.

## Para entidades

- Entidade criada no projeto correto.
- Configuracao EF criada.
- Tabela nomeada conforme convencao.
- Indices necessarios configurados.
- `TenantId` incluido quando for entidade de negocio multi-tenant.
- Testes adicionados quando houver regra critica.

## Para endpoints

- Request e response usam DTOs.
- Validacoes relevantes existem.
- Erros esperados retornam resposta previsivel.
- Endpoint respeita autenticacao/autorizacao quando aplicavel.
- Exemplo de uso foi documentado.

## Para provisioning

- Fluxo valida input antes de executar mudancas.
- Nao loga secrets.
- Pode ser testado de forma automatizada.
- Bloqueia reinstalacao indevida.
- Registra estado final da instalacao.

## Para Docker

- `docker compose up -d --build` funciona ou limitacao esta documentada.
- Variaveis sensiveis ficam em `.env`, nao no repositorio.
- `.env.example` contem apenas valores de exemplo.

## Para testes

- Teste novo falha pelo motivo certo antes da correcao, quando praticavel.
- Teste passa apos implementacao.
- Teste de integracao usa SQL Server real quando o comportamento depende do banco.
- `dotnet test` passa para a entrega final da fase.
