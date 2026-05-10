# Fluxo do Web Installer

O Web Installer expoe os endpoints iniciais para verificar status, testar banco e executar a instalacao.

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

Request de exemplo:

```text
docs/examples/install-request.json
```

Fluxo interno:

1. Validar request.
2. Verificar se o instalador ja esta bloqueado.
3. Testar conexao com SQL Server.
4. Criar banco se necessario.
5. Executar migrations.
6. Executar seeds globais.
7. Criar tenant.
8. Criar branding.
9. Criar admin.
10. Instalar modulos.
11. Bloquear instalador.

## Observacao de bootstrap

A connection string do host/container e permitida apenas como configuracao minima de bootstrap. Configuracoes definitivas da plataforma devem ser persistidas em entidades de banco, conforme `agent.md`.

Ao executar o installer dentro do Docker Compose, use `mssql` como host do banco. Ao executar localmente fora do container, use `localhost`.

Para startup multiplo no Visual Studio, configure `ConnectionStrings:DefaultConnection` via User Secrets nos projetos `Platform.Api` e `Platform.WebInstaller`. O passo a passo fica em [local-development.md](local-development.md).
