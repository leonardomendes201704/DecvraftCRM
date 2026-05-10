# Desenvolvimento Local

Este documento registra os ajustes necessarios para executar a API e o Web Installer fora do Docker Compose, por exemplo usando startup multiplo no Visual Studio.

## Connection string de bootstrap

Os hosts `Platform.Api` e `Platform.WebInstaller` usam a connection string de bootstrap chamada `DefaultConnection`.

Essa connection string e usada somente para abrir o banco inicial da plataforma. Configuracoes definitivas da aplicacao continuam sendo persistidas em entidades de banco, conforme a diretriz do projeto.

Nao grave credenciais reais em `appsettings`, `launchSettings` ou codigo fonte.

## Visual Studio com startup multiplo

Antes de iniciar `Platform.Api` e `Platform.WebInstaller` juntos pelo Visual Studio, configure User Secrets nos dois projetos:

```powershell
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost,1433;Database=WhiteLabelErp;User Id=sa;Password=SUA_SENHA_LOCAL;TrustServerCertificate=True;" --project src/Platform.Api/Platform.Api.csproj
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost,1433;Database=WhiteLabelErp;User Id=sa;Password=SUA_SENHA_LOCAL;TrustServerCertificate=True;" --project src/Platform.WebInstaller/Platform.WebInstaller.csproj
```

Use `localhost` quando executar os projetos fora do Docker. Use `mssql` apenas quando a aplicacao estiver rodando dentro da rede do Docker Compose.

## Compatibilidade

O nome antigo `Default` ainda e aceito como fallback para evitar quebra de ambientes locais antigos, mas o nome padrao do projeto e `DefaultConnection`.
