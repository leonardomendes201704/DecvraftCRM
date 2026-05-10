# Fluxo do Web Installer

O Web Installer expoe os endpoints iniciais para verificar status, testar banco e executar a instalacao.

O fluxo completo tambem pode ser executado pela interface em `GET /install`.

## Endpoints

### GET /install/status

Retorna o estado atual da instalacao.

Resposta quando ainda nao instalado:

```json
{
  "isInstalled": false,
  "version": null,
  "installedAt": null
}
```

### POST /install/test-database

Valida se o SQL Server informado esta acessivel.

Request:

```json
{
  "host": "mssql",
  "port": 1433,
  "databaseName": "WhiteLabelErp",
  "username": "sa",
  "password": "YourStrong!Passw0rd",
  "trustServerCertificate": true
}
```

Resposta de sucesso:

```json
{
  "succeeded": true,
  "error": null
}
```

### POST /install/run

Executa o provisionamento inicial completo.

Na UI, a execucao equivalente acontece em `POST /install/review`, apos a tela de revisao do wizard.

Request de exemplo:

```text
docs/examples/install-request.json
```

Fluxo interno:

1. Validar request.
2. Testar conexao com SQL Server.
3. Criar banco se necessario.
4. Trocar o contexto de persistencia para o banco alvo informado.
5. Verificar se o instalador ja esta bloqueado no banco alvo.
6. Executar migrations.
7. Executar seeds globais.
8. Criar tenant.
9. Criar branding.
10. Criar admin.
11. Instalar modulos.
12. Bloquear instalador.

Para criar outra instalacao no mesmo host SQL Server, use outro `databaseName` no wizard. O host pode ser o mesmo; o banco alvo precisa ser novo ou ainda nao instalado.

## Observacao de bootstrap

A connection string do host/container e permitida apenas como configuracao minima de bootstrap. Configuracoes definitivas da plataforma devem ser persistidas em entidades de banco, conforme `agent.md`.

Ao executar o installer dentro do Docker Compose, use `mssql` como host do banco. Ao executar localmente fora do container, use `localhost`.

Para startup multiplo no Visual Studio, configure `ConnectionStrings:DefaultConnection` via User Secrets nos projetos `Platform.Api` e `Platform.WebInstaller`. O passo a passo fica em [local-development.md](local-development.md).
