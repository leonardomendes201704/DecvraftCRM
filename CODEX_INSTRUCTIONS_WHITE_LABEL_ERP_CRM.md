# Instruções para o Codex — White-label Modular ERP/CRM com Web Installer e Auto Provisioning

## 1. Objetivo do projeto

Construir uma plataforma **ERP/CRM white-label, modular, auto instalável, versionável e escalável**, usando:

- **.NET 8**
- **ASP.NET Core Web API**
- **Docker**
- **SQL Server em Docker**
- **Entity Framework Core**
- **EF Core Migrations**
- **Testes automatizados**
- **Arquitetura limpa/modular**
- **Web Installer estilo WordPress**
- **Auto Provisioning de banco, tenant, usuário admin, módulos e branding**

A plataforma deve permitir que um cliente suba o sistema em um servidor, acesse `/install` ou `/setup`, informe os dados do banco e da empresa, e o sistema prepare tudo automaticamente.

---

## 2. Nome conceitual da arquitetura

Nome técnico sugerido:

> **Modular White-label ERP/CRM Provisioning Architecture**

Nome em português:

> **Arquitetura Modular Auto Provisionável para ERP/CRM White-label**

Nome interno do produto/plataforma:

> **ERP Bootstrap Platform**

Nome do motor principal de instalação:

> **Provisioning Engine**

---

## 3. Comportamento esperado do sistema

O sistema deve funcionar assim:

1. O usuário sobe a aplicação via Docker.
2. O usuário acessa `/install` ou `/setup`.
3. O sistema verifica se já foi instalado.
4. Se não foi instalado, exibe o assistente de instalação.
5. O usuário informa:
   - host do SQL Server;
   - porta;
   - nome do banco;
   - usuário do banco;
   - senha do banco;
   - nome da empresa;
   - slug/subdomínio;
   - domínio customizado opcional;
   - nome do primeiro administrador;
   - e-mail do primeiro administrador;
   - senha do primeiro administrador;
   - módulos iniciais desejados.
6. O sistema testa a conexão com o banco.
7. O sistema cria o banco, se necessário.
8. O sistema executa as migrations.
9. O sistema executa os seeds iniciais.
10. O sistema cria o tenant/empresa.
11. O sistema cria o usuário administrador.
12. O sistema instala os módulos base.
13. O sistema aplica configurações white-label iniciais.
14. O sistema grava o estado da instalação.
15. O sistema bloqueia o instalador.
16. O usuário é redirecionado para `/login`.

---

## 4. Requisitos técnicos principais

### Backend

- ASP.NET Core 8 Web API.
- C# 12.
- Entity Framework Core.
- SQL Server.
- Autenticação JWT.
- Autorização baseada em papéis/permissões.
- Multi-tenancy.
- Logs estruturados.
- Health checks.
- Versionamento de migrations.
- Testes automatizados.

### Infraestrutura

- Docker.
- Docker Compose.
- SQL Server containerizado.
- Volume persistente para o banco.
- Possibilidade futura de Redis.
- Possibilidade futura de Worker Service.
- Possibilidade futura de filas/eventos.

### Testes

- xUnit.
- FluentAssertions.
- Testes de unidade.
- Testes de integração.
- Testes do fluxo de provisionamento.
- Testes de migrations.
- Testes de isolamento por tenant.
- Testes de arquitetura.

---

## 5. Decisões arquiteturais iniciais

### 5.1 Tipo de arquitetura

Usar uma arquitetura modular baseada em camadas:

- **Domain**
- **Application**
- **Infrastructure**
- **Persistence**
- **Provisioning**
- **Modules**
- **Api**
- **WebInstaller**
- **Worker**, opcional futuramente

Evitar colocar regra de negócio diretamente nos controllers.

---

### 5.2 Multi-tenancy

Começar com o modelo:

> **Banco único com TenantId em todas as tabelas de negócio.**

Preparar a arquitetura para evoluir futuramente para:

> **Master Database + Tenant Database por cliente enterprise.**

Na primeira fase, todas as entidades de negócio devem ter `TenantId`.

Exemplos:

- Clientes
- Contatos
- Produtos
- Pedidos
- Oportunidades
- Lançamentos financeiros
- Usuários vinculados à empresa
- Configurações de módulo

---

### 5.3 Instalação inicial

O instalador deve ser tratado como um módulo controlado, não como uma tela comum.

Após a instalação, o sistema deve bloquear o acesso ao instalador usando pelo menos dois mecanismos:

1. Registro em banco na tabela `SystemInstallations`.
2. Arquivo físico ou lógico de lock, por exemplo `installer.lock`.

---

### 5.4 Migrations

Usar EF Core Migrations para versionar a estrutura do banco.

Em ambiente de desenvolvimento, pode aplicar migrations automaticamente.

Em produção, criar estrutura para permitir:

- geração de script SQL;
- backup antes de update;
- execução controlada;
- registro da versão aplicada;
- rollback planejado quando possível.

---

### 5.5 Modularidade

Cada módulo deve ter metadados próprios.

Módulos iniciais sugeridos:

- Core
- CRM
- Financeiro
- Estoque
- Vendas
- Atendimento
- RH
- Relatórios

Na primeira versão, implementar apenas:

- Core
- CRM básico
- Financeiro básico
- Installer
- Tenant
- Usuários
- Autenticação
- Permissões

---

## 6. Estrutura inicial da solução

Criar a solução com a seguinte estrutura:

```text
WhiteLabelErpCrm/
│
├── src/
│   ├── Platform.Api/
│   │   ├── Controllers/
│   │   ├── Middlewares/
│   │   ├── Extensions/
│   │   ├── Program.cs
│   │   └── appsettings.json
│   │
│   ├── Platform.WebInstaller/
│   │   ├── Controllers/
│   │   ├── Models/
│   │   ├── Services/
│   │   └── Program.cs
│   │
│   ├── Platform.Domain/
│   │   ├── Common/
│   │   ├── Entities/
│   │   ├── ValueObjects/
│   │   ├── Enums/
│   │   ├── Events/
│   │   └── Interfaces/
│   │
│   ├── Platform.Application/
│   │   ├── Abstractions/
│   │   ├── DTOs/
│   │   ├── UseCases/
│   │   ├── Services/
│   │   ├── Validators/
│   │   └── Mapping/
│   │
│   ├── Platform.Infrastructure/
│   │   ├── Auth/
│   │   ├── Email/
│   │   ├── Files/
│   │   ├── Licensing/
│   │   ├── Logging/
│   │   └── Time/
│   │
│   ├── Platform.Persistence/
│   │   ├── AppDbContext.cs
│   │   ├── Configurations/
│   │   ├── Migrations/
│   │   ├── Seeders/
│   │   └── Repositories/
│   │
│   ├── Platform.Provisioning/
│   │   ├── Abstractions/
│   │   ├── Models/
│   │   ├── Services/
│   │   ├── DatabaseProvisioner.cs
│   │   ├── MigrationRunner.cs
│   │   ├── SeedRunner.cs
│   │   ├── TenantProvisioner.cs
│   │   ├── ModuleInstaller.cs
│   │   └── InstallerLockService.cs
│   │
│   ├── Platform.Modules.Core/
│   │   ├── ModuleDefinition.cs
│   │   └── Permissions.cs
│   │
│   ├── Platform.Modules.Crm/
│   │   ├── Entities/
│   │   ├── UseCases/
│   │   ├── Controllers/
│   │   ├── ModuleDefinition.cs
│   │   └── Permissions.cs
│   │
│   ├── Platform.Modules.Finance/
│   │   ├── Entities/
│   │   ├── UseCases/
│   │   ├── Controllers/
│   │   ├── ModuleDefinition.cs
│   │   └── Permissions.cs
│   │
│   └── Platform.Worker/
│       ├── Jobs/
│       └── Program.cs
│
├── tests/
│   ├── Platform.UnitTests/
│   ├── Platform.IntegrationTests/
│   ├── Platform.ProvisioningTests/
│   ├── Platform.MigrationTests/
│   └── Platform.ArchitectureTests/
│
├── docker/
│   ├── Dockerfile.api
│   ├── Dockerfile.installer
│   └── init/
│
├── docs/
│   ├── architecture.md
│   ├── modules.md
│   ├── installer-flow.md
│   └── roadmap.md
│
├── docker-compose.yml
├── .env.example
├── .gitignore
├── README.md
└── WhiteLabelErpCrm.sln
```

---

## 7. Comandos iniciais para criar a solução

Executar comandos equivalentes aos abaixo:

```bash
dotnet new sln -n WhiteLabelErpCrm

mkdir src
mkdir tests
mkdir docker
mkdir docs

dotnet new webapi -n Platform.Api -o src/Platform.Api
dotnet new webapi -n Platform.WebInstaller -o src/Platform.WebInstaller
dotnet new classlib -n Platform.Domain -o src/Platform.Domain
dotnet new classlib -n Platform.Application -o src/Platform.Application
dotnet new classlib -n Platform.Infrastructure -o src/Platform.Infrastructure
dotnet new classlib -n Platform.Persistence -o src/Platform.Persistence
dotnet new classlib -n Platform.Provisioning -o src/Platform.Provisioning
dotnet new classlib -n Platform.Modules.Core -o src/Platform.Modules.Core
dotnet new classlib -n Platform.Modules.Crm -o src/Platform.Modules.Crm
dotnet new classlib -n Platform.Modules.Finance -o src/Platform.Modules.Finance
dotnet new worker -n Platform.Worker -o src/Platform.Worker

dotnet new xunit -n Platform.UnitTests -o tests/Platform.UnitTests
dotnet new xunit -n Platform.IntegrationTests -o tests/Platform.IntegrationTests
dotnet new xunit -n Platform.ProvisioningTests -o tests/Platform.ProvisioningTests
dotnet new xunit -n Platform.MigrationTests -o tests/Platform.MigrationTests
dotnet new xunit -n Platform.ArchitectureTests -o tests/Platform.ArchitectureTests

dotnet sln add src/Platform.Api/Platform.Api.csproj
dotnet sln add src/Platform.WebInstaller/Platform.WebInstaller.csproj
dotnet sln add src/Platform.Domain/Platform.Domain.csproj
dotnet sln add src/Platform.Application/Platform.Application.csproj
dotnet sln add src/Platform.Infrastructure/Platform.Infrastructure.csproj
dotnet sln add src/Platform.Persistence/Platform.Persistence.csproj
dotnet sln add src/Platform.Provisioning/Platform.Provisioning.csproj
dotnet sln add src/Platform.Modules.Core/Platform.Modules.Core.csproj
dotnet sln add src/Platform.Modules.Crm/Platform.Modules.Crm.csproj
dotnet sln add src/Platform.Modules.Finance/Platform.Modules.Finance.csproj
dotnet sln add src/Platform.Worker/Platform.Worker.csproj

dotnet sln add tests/Platform.UnitTests/Platform.UnitTests.csproj
dotnet sln add tests/Platform.IntegrationTests/Platform.IntegrationTests.csproj
dotnet sln add tests/Platform.ProvisioningTests/Platform.ProvisioningTests.csproj
dotnet sln add tests/Platform.MigrationTests/Platform.MigrationTests.csproj
dotnet sln add tests/Platform.ArchitectureTests/Platform.ArchitectureTests.csproj
```

---

## 8. Referências entre projetos

Configurar referências aproximadamente assim:

```bash
dotnet add src/Platform.Application/Platform.Application.csproj reference src/Platform.Domain/Platform.Domain.csproj

dotnet add src/Platform.Persistence/Platform.Persistence.csproj reference src/Platform.Domain/Platform.Domain.csproj
dotnet add src/Platform.Persistence/Platform.Persistence.csproj reference src/Platform.Application/Platform.Application.csproj

dotnet add src/Platform.Infrastructure/Platform.Infrastructure.csproj reference src/Platform.Application/Platform.Application.csproj
dotnet add src/Platform.Infrastructure/Platform.Infrastructure.csproj reference src/Platform.Domain/Platform.Domain.csproj

dotnet add src/Platform.Provisioning/Platform.Provisioning.csproj reference src/Platform.Domain/Platform.Domain.csproj
dotnet add src/Platform.Provisioning/Platform.Provisioning.csproj reference src/Platform.Application/Platform.Application.csproj
dotnet add src/Platform.Provisioning/Platform.Provisioning.csproj reference src/Platform.Persistence/Platform.Persistence.csproj
dotnet add src/Platform.Provisioning/Platform.Provisioning.csproj reference src/Platform.Infrastructure/Platform.Infrastructure.csproj

dotnet add src/Platform.Api/Platform.Api.csproj reference src/Platform.Application/Platform.Application.csproj
dotnet add src/Platform.Api/Platform.Api.csproj reference src/Platform.Infrastructure/Platform.Infrastructure.csproj
dotnet add src/Platform.Api/Platform.Api.csproj reference src/Platform.Persistence/Platform.Persistence.csproj
dotnet add src/Platform.Api/Platform.Api.csproj reference src/Platform.Provisioning/Platform.Provisioning.csproj
dotnet add src/Platform.Api/Platform.Api.csproj reference src/Platform.Modules.Core/Platform.Modules.Core.csproj
dotnet add src/Platform.Api/Platform.Api.csproj reference src/Platform.Modules.Crm/Platform.Modules.Crm.csproj
dotnet add src/Platform.Api/Platform.Api.csproj reference src/Platform.Modules.Finance/Platform.Modules.Finance.csproj

dotnet add src/Platform.WebInstaller/Platform.WebInstaller.csproj reference src/Platform.Provisioning/Platform.Provisioning.csproj
dotnet add src/Platform.WebInstaller/Platform.WebInstaller.csproj reference src/Platform.Persistence/Platform.Persistence.csproj
dotnet add src/Platform.WebInstaller/Platform.WebInstaller.csproj reference src/Platform.Infrastructure/Platform.Infrastructure.csproj
```

---

## 9. Pacotes NuGet sugeridos

Adicionar pacotes conforme necessário:

```bash
dotnet add src/Platform.Persistence package Microsoft.EntityFrameworkCore.SqlServer
dotnet add src/Platform.Persistence package Microsoft.EntityFrameworkCore.Design
dotnet add src/Platform.Persistence package Microsoft.EntityFrameworkCore.Tools

dotnet add src/Platform.Api package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add src/Platform.Api package Microsoft.AspNetCore.OpenApi
dotnet add src/Platform.Api package Swashbuckle.AspNetCore

dotnet add src/Platform.Application package FluentValidation
dotnet add src/Platform.Application package AutoMapper

dotnet add src/Platform.Infrastructure package Serilog.AspNetCore
dotnet add src/Platform.Infrastructure package Serilog.Sinks.Console
dotnet add src/Platform.Infrastructure package Serilog.Sinks.File

dotnet add tests/Platform.UnitTests package FluentAssertions
dotnet add tests/Platform.IntegrationTests package FluentAssertions
dotnet add tests/Platform.IntegrationTests package Testcontainers.MsSql
dotnet add tests/Platform.ProvisioningTests package FluentAssertions
dotnet add tests/Platform.ProvisioningTests package Testcontainers.MsSql
dotnet add tests/Platform.ArchitectureTests package NetArchTest.Rules
```

---

## 10. Entidades iniciais obrigatórias

Criar inicialmente as entidades abaixo.

### 10.1 SystemInstallation

Representa o estado da instalação da plataforma.

Campos sugeridos:

```csharp
public sealed class SystemInstallation
{
    public Guid Id { get; private set; }
    public string InstallationKey { get; private set; } = string.Empty;
    public string InstalledVersion { get; private set; } = string.Empty;
    public bool IsInstalled { get; private set; }
    public DateTimeOffset? InstalledAt { get; private set; }
    public string? InstalledBy { get; private set; }
    public string Environment { get; private set; } = string.Empty;
}
```

---

### 10.2 Tenant

Representa uma empresa/cliente.

Campos sugeridos:

```csharp
public sealed class Tenant
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Slug { get; private set; } = string.Empty;
    public string? CustomDomain { get; private set; }
    public bool IsActive { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
}
```

---

### 10.3 TenantBranding

Representa a identidade visual white-label.

Campos sugeridos:

```csharp
public sealed class TenantBranding
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public string SystemName { get; private set; } = string.Empty;
    public string? LogoUrl { get; private set; }
    public string? FaviconUrl { get; private set; }
    public string PrimaryColor { get; private set; } = "#2563EB";
    public string SecondaryColor { get; private set; } = "#111827";
    public string? LoginBackgroundUrl { get; private set; }
    public string? EmailSenderName { get; private set; }
}
```

---

### 10.4 Module

Representa um módulo instalável.

Campos sugeridos:

```csharp
public sealed class Module
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Slug { get; private set; } = string.Empty;
    public string Version { get; private set; } = string.Empty;
    public bool IsCore { get; private set; }
    public bool IsEnabled { get; private set; }
}
```

---

### 10.5 TenantModule

Representa quais módulos estão ativos para cada tenant.

```csharp
public sealed class TenantModule
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public Guid ModuleId { get; private set; }
    public bool IsEnabled { get; private set; }
    public DateTimeOffset InstalledAt { get; private set; }
}
```

---

### 10.6 ApplicationUser

Representa usuário da plataforma.

Campos mínimos:

```csharp
public sealed class ApplicationUser
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public bool IsActive { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
}
```

---

### 10.7 Role e Permission

Criar estrutura para RBAC.

```csharp
public sealed class Role
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public bool IsSystemRole { get; private set; }
}
```

```csharp
public sealed class Permission
{
    public Guid Id { get; private set; }
    public string Key { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string ModuleSlug { get; private set; } = string.Empty;
}
```

---

## 11. Entidades iniciais do CRM

Criar o módulo CRM básico com as entidades:

### Customer

```csharp
public sealed class Customer
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Document { get; private set; }
    public string? Email { get; private set; }
    public string? Phone { get; private set; }
    public bool IsActive { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
}
```

### Contact

```csharp
public sealed class Contact
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public Guid CustomerId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Email { get; private set; }
    public string? Phone { get; private set; }
    public string? Position { get; private set; }
}
```

### Opportunity

```csharp
public sealed class Opportunity
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public Guid CustomerId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public decimal EstimatedValue { get; private set; }
    public string Status { get; private set; } = "Open";
    public DateTimeOffset CreatedAt { get; private set; }
}
```

---

## 12. Entidades iniciais do Financeiro

Criar o módulo financeiro básico com as entidades:

### FinancialAccount

```csharp
public sealed class FinancialAccount
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Type { get; private set; } = string.Empty;
    public bool IsActive { get; private set; }
}
```

### FinancialTransaction

```csharp
public sealed class FinancialTransaction
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public Guid AccountId { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public decimal Amount { get; private set; }
    public string Type { get; private set; } = string.Empty;
    public DateOnly DueDate { get; private set; }
    public DateOnly? PaidAt { get; private set; }
    public string Status { get; private set; } = "Pending";
}
```

---

## 13. AppDbContext

Criar `AppDbContext` em `Platform.Persistence`.

Requisitos:

- Mapear todas as entidades iniciais.
- Aplicar configurações via `IEntityTypeConfiguration<T>`.
- Adicionar filtros globais por `TenantId` nas entidades multi-tenant.
- Usar `DateTimeOffset` para auditoria.
- Usar `Guid` como identificador inicial.
- Configurar índices únicos importantes:
  - `Tenant.Slug`;
  - `ApplicationUser.Email + TenantId`;
  - `Module.Slug`;
  - `TenantModule.TenantId + ModuleId`.

---

## 14. Contratos do Provisioning Engine

Criar as seguintes interfaces:

```csharp
public interface IDatabaseProvisioner
{
    Task TestConnectionAsync(DatabaseSetupOptions options, CancellationToken cancellationToken = default);
    Task EnsureDatabaseCreatedAsync(DatabaseSetupOptions options, CancellationToken cancellationToken = default);
}
```

```csharp
public interface IMigrationRunner
{
    Task RunAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<string>> GetPendingMigrationsAsync(CancellationToken cancellationToken = default);
}
```

```csharp
public interface ISeedRunner
{
    Task RunAsync(CancellationToken cancellationToken = default);
}
```

```csharp
public interface ITenantProvisioner
{
    Task<Guid> CreateTenantAsync(TenantSetupOptions options, CancellationToken cancellationToken = default);
    Task<Guid> CreateAdminUserAsync(Guid tenantId, AdminUserSetupOptions options, CancellationToken cancellationToken = default);
}
```

```csharp
public interface IModuleInstaller
{
    Task InstallBaseModulesAsync(Guid tenantId, IReadOnlyCollection<string> moduleSlugs, CancellationToken cancellationToken = default);
}
```

```csharp
public interface IInstallerLockService
{
    Task<bool> IsInstalledAsync(CancellationToken cancellationToken = default);
    Task LockAsync(string installedVersion, string installedBy, CancellationToken cancellationToken = default);
}
```

---

## 15. DTO principal de instalação

Criar algo similar a:

```csharp
public sealed class InstallRequest
{
    public DatabaseSetupOptions Database { get; set; } = new();
    public TenantSetupOptions Tenant { get; set; } = new();
    public AdminUserSetupOptions AdminUser { get; set; } = new();
    public BrandingSetupOptions Branding { get; set; } = new();
    public List<string> Modules { get; set; } = new();
}
```

```csharp
public sealed class DatabaseSetupOptions
{
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; } = 1433;
    public string DatabaseName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool TrustServerCertificate { get; set; } = true;
}
```

```csharp
public sealed class TenantSetupOptions
{
    public string CompanyName { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? CustomDomain { get; set; }
}
```

```csharp
public sealed class AdminUserSetupOptions
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
```

```csharp
public sealed class BrandingSetupOptions
{
    public string SystemName { get; set; } = string.Empty;
    public string PrimaryColor { get; set; } = "#2563EB";
    public string SecondaryColor { get; set; } = "#111827";
}
```

---

## 16. Fluxo esperado do serviço de instalação

Implementar um serviço principal:

```csharp
public interface IProvisioningService
{
    Task<ProvisioningResult> InstallAsync(InstallRequest request, CancellationToken cancellationToken = default);
}
```

Fluxo obrigatório:

```text
1. Validar request.
2. Verificar se já está instalado.
3. Testar conexão com banco.
4. Criar banco se necessário.
5. Executar migrations.
6. Executar seeds globais.
7. Criar tenant.
8. Criar branding.
9. Criar usuário admin.
10. Instalar módulos.
11. Criar papéis e permissões padrão.
12. Registrar versão instalada.
13. Bloquear instalador.
14. Retornar sucesso.
```

O processo deve ser idempotente onde fizer sentido, ou seja, se algo já existir e estiver coerente, não deve quebrar indevidamente.

---

## 17. Endpoints iniciais

### API principal

Criar endpoints mínimos:

```text
GET /health
GET /api/system/status
POST /api/auth/login
GET /api/modules
GET /api/me
```

### Installer

Criar endpoints mínimos:

```text
GET /install/status
POST /install/test-database
POST /install/run
```

`GET /install/status` deve retornar se o sistema já foi instalado.

Exemplo:

```json
{
  "isInstalled": false,
  "version": null
}
```

`POST /install/test-database` deve validar se é possível conectar.

`POST /install/run` deve executar a instalação completa.

---

## 18. Docker Compose

Criar `docker-compose.yml` na raiz.

Versão inicial sugerida:

```yaml
services:
  mssql:
    image: mcr.microsoft.com/mssql/server:2022-latest
    container_name: whitelabel-mssql
    environment:
      ACCEPT_EULA: "Y"
      MSSQL_SA_PASSWORD: "${MSSQL_SA_PASSWORD}"
      MSSQL_PID: "Developer"
    ports:
      - "1433:1433"
    volumes:
      - mssql_data:/var/opt/mssql
    networks:
      - whitelabel-network

  api:
    build:
      context: .
      dockerfile: docker/Dockerfile.api
    container_name: whitelabel-api
    environment:
      ASPNETCORE_ENVIRONMENT: "Development"
      ConnectionStrings__DefaultConnection: "Server=mssql,1433;Database=${MSSQL_DATABASE};User Id=sa;Password=${MSSQL_SA_PASSWORD};TrustServerCertificate=True;"
    ports:
      - "8080:8080"
    depends_on:
      - mssql
    networks:
      - whitelabel-network

  installer:
    build:
      context: .
      dockerfile: docker/Dockerfile.installer
    container_name: whitelabel-installer
    environment:
      ASPNETCORE_ENVIRONMENT: "Development"
    ports:
      - "8081:8080"
    depends_on:
      - mssql
    networks:
      - whitelabel-network

volumes:
  mssql_data:

networks:
  whitelabel-network:
    driver: bridge
```

---

## 19. Arquivo .env.example

Criar `.env.example`:

```env
MSSQL_SA_PASSWORD=YourStrong!Passw0rd
MSSQL_DATABASE=WhiteLabelErp
ASPNETCORE_ENVIRONMENT=Development
JWT_SECRET=CHANGE_ME_SUPER_SECRET_KEY
JWT_ISSUER=WhiteLabelErpCrm
JWT_AUDIENCE=WhiteLabelErpCrmUsers
```

Nunca commitar `.env` real.

---

## 20. Dockerfile da API

Criar `docker/Dockerfile.api`:

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .
RUN dotnet restore
RUN dotnet publish src/Platform.Api/Platform.Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "Platform.Api.dll"]
```

---

## 21. Dockerfile do Installer

Criar `docker/Dockerfile.installer`:

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .
RUN dotnet restore
RUN dotnet publish src/Platform.WebInstaller/Platform.WebInstaller.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "Platform.WebInstaller.dll"]
```

---

## 22. Estratégia de migrations

Criar a primeira migration com:

```bash
dotnet ef migrations add InitialCreate \
  --project src/Platform.Persistence \
  --startup-project src/Platform.Api
```

Aplicar em desenvolvimento com:

```bash
dotnet ef database update \
  --project src/Platform.Persistence \
  --startup-project src/Platform.Api
```

Gerar script para produção com:

```bash
dotnet ef migrations script \
  --project src/Platform.Persistence \
  --startup-project src/Platform.Api \
  --output ./artifacts/sql/migration.sql
```

Criar pasta:

```text
artifacts/sql/
```

Não commitar scripts com dados sensíveis.

---

## 23. Seeds obrigatórios

Criar seeds para:

### Roles

- SystemAdmin
- TenantAdmin
- Manager
- User

### Permissions

Core:

- `core.system.view`
- `core.system.manage`
- `core.users.view`
- `core.users.manage`
- `core.modules.view`
- `core.modules.manage`

CRM:

- `crm.customers.view`
- `crm.customers.create`
- `crm.customers.update`
- `crm.customers.delete`
- `crm.opportunities.view`
- `crm.opportunities.manage`

Financeiro:

- `finance.accounts.view`
- `finance.accounts.manage`
- `finance.transactions.view`
- `finance.transactions.manage`

### Modules

- core
- crm
- finance

---

## 24. Segurança obrigatória

Implementar desde o início:

- Hash de senha seguro.
- Nunca salvar senha do banco em texto puro.
- Não logar secrets.
- JWT com expiração.
- Refresh token futuramente.
- Isolamento por tenant.
- Filtro global por `TenantId`.
- Bloqueio do instalador após instalação.
- Proteção contra reinstalação acidental.
- Logs de auditoria para ações críticas.
- Validação forte dos inputs do instalador.
- CORS configurado de forma restritiva.
- HTTPS preparado para produção.

---

## 25. Logs

Usar Serilog.

Eventos importantes para log:

- início da instalação;
- teste de conexão com banco;
- criação de banco;
- execução de migration;
- criação de tenant;
- criação de admin;
- instalação de módulo;
- bloqueio do instalador;
- erro no provisioning;
- login;
- falha de login;
- troca de configurações do tenant;
- alteração de módulo;
- erro crítico da aplicação.

---

## 26. Health checks

Criar endpoints:

```text
GET /health
GET /health/database
GET /health/modules
GET /health/migrations
```

Exemplo de resposta esperada:

```json
{
  "status": "Healthy",
  "database": "Healthy",
  "migrations": "UpToDate",
  "modules": [
    {
      "slug": "core",
      "status": "Enabled",
      "version": "1.0.0"
    },
    {
      "slug": "crm",
      "status": "Enabled",
      "version": "1.0.0"
    }
  ]
}
```

---

## 27. Testes obrigatórios

### 27.1 Testes de unidade

Testar regras de:

- criação de tenant;
- validação de slug;
- criação de módulo;
- validação de instalação;
- criação de usuário;
- regras de senha;
- permissões.

---

### 27.2 Testes de integração

Usar SQL Server real com Testcontainers.

Testar:

- conexão com banco;
- criação de schema;
- execução de migrations;
- persistência de tenant;
- persistência de módulos;
- login.

---

### 27.3 Testes do Provisioning Engine

Criar cenário:

```text
Dado um SQL Server vazio
Quando o InstallRequest válido for enviado
Então o sistema deve:
  - testar a conexão;
  - criar estrutura;
  - aplicar migrations;
  - criar tenant;
  - criar admin;
  - instalar módulos;
  - bloquear instalador;
  - permitir login do admin.
```

---

### 27.4 Testes de migrations

Criar teste que sobe banco limpo e aplica todas as migrations.

Critério de sucesso:

```text
Todas as migrations devem executar do zero sem erro.
```

---

### 27.5 Testes de arquitetura

Usar NetArchTest ou alternativa.

Regras:

- Domain não pode depender de Application.
- Domain não pode depender de Infrastructure.
- Application não pode depender de Api.
- Application não pode depender de Persistence concreta, apenas abstrações.
- Infrastructure pode depender de Application e Domain.
- Api pode depender das demais camadas.
- Modules não devem acessar diretamente infraestrutura não permitida.

---

## 28. Critérios de aceite da primeira entrega

A primeira entrega será considerada válida quando:

1. A solution compilar com `dotnet build`.
2. O Docker Compose subir SQL Server, API e Installer.
3. `GET /health` responder com sucesso.
4. `GET /install/status` retornar `isInstalled = false` antes da instalação.
5. `POST /install/test-database` validar a conexão.
6. `POST /install/run` executar a instalação completa.
7. As tabelas iniciais forem criadas.
8. O tenant inicial for criado.
9. O usuário admin for criado.
10. Os módulos `core`, `crm` e `finance` forem registrados.
11. O instalador for bloqueado após a instalação.
12. `GET /install/status` retornar `isInstalled = true` após a instalação.
13. `POST /api/auth/login` autenticar o admin.
14. Os testes principais passarem com `dotnet test`.

---

## 29. Roadmap técnico inicial

### Fase 1 — Fundação

- Criar solution.
- Criar projetos.
- Configurar referências.
- Adicionar pacotes.
- Criar Docker Compose.
- Criar SQL Server em Docker.
- Criar API base.
- Criar Health Check.
- Criar AppDbContext.
- Criar entidades core.
- Criar primeira migration.

---

### Fase 2 — Provisioning Engine

- Criar contratos.
- Criar DTOs.
- Criar DatabaseProvisioner.
- Criar MigrationRunner.
- Criar SeedRunner.
- Criar TenantProvisioner.
- Criar ModuleInstaller.
- Criar InstallerLockService.
- Criar ProvisioningService.
- Criar endpoints do Installer.

---

### Fase 3 — Segurança e autenticação

- Implementar hash de senha.
- Implementar login JWT.
- Criar roles.
- Criar permissions.
- Criar autorização básica.
- Criar endpoint `/api/me`.

---

### Fase 4 — Módulo CRM

- Criar Customer.
- Criar Contact.
- Criar Opportunity.
- Criar endpoints CRUD básicos.
- Aplicar filtro por tenant.
- Criar permissões de CRM.

---

### Fase 5 — Módulo Financeiro

- Criar FinancialAccount.
- Criar FinancialTransaction.
- Criar endpoints CRUD básicos.
- Aplicar filtro por tenant.
- Criar permissões financeiras.

---

### Fase 6 — Testes

- Criar testes unitários.
- Criar testes de integração.
- Criar testes de provisioning.
- Criar testes de migration.
- Criar testes de arquitetura.

---

### Fase 7 — Documentação

- Criar README principal.
- Criar docs/architecture.md.
- Criar docs/installer-flow.md.
- Criar docs/modules.md.
- Criar docs/roadmap.md.

---

## 30. Padrões de código

Adotar os padrões abaixo:

- Usar `sealed class` quando não houver necessidade de herança.
- Usar `CancellationToken` em operações assíncronas.
- Usar `async/await`.
- Evitar lógica de negócio em controllers.
- Usar DTOs para entrada e saída.
- Usar FluentValidation para validações relevantes.
- Usar `Result` ou padrão equivalente para retornos previsíveis.
- Evitar exceptions para fluxo esperado.
- Separar configuração EF em classes `IEntityTypeConfiguration<T>`.
- Criar extensões para registro de dependências:
  - `AddApplication()`
  - `AddInfrastructure()`
  - `AddPersistence()`
  - `AddProvisioning()`
  - `AddModules()`

---

## 31. Convenções de nomenclatura

### Projetos

Usar prefixo:

```text
Platform.*
```

### Entidades

Usar nomes no singular:

```text
Tenant
Module
Customer
Opportunity
FinancialTransaction
```

### Tabelas

Usar nomes no plural:

```text
Tenants
Modules
Customers
Opportunities
FinancialTransactions
```

### Permissões

Usar padrão:

```text
modulo.recurso.acao
```

Exemplos:

```text
crm.customers.view
crm.customers.create
finance.transactions.manage
core.modules.manage
```

---

## 32. Regras importantes para o Codex

Ao implementar:

1. Não simplificar a arquitetura a ponto de misturar tudo no projeto API.
2. Não salvar senha em texto puro.
3. Não deixar `/install` aberto após instalação.
4. Não remover o conceito de módulos.
5. Não remover o conceito de tenant.
6. Não criar tudo em uma única camada.
7. Não ignorar testes.
8. Não usar banco em memória para testes de integração do SQL Server.
9. Não depender de recursos pagos.
10. Manter compatibilidade com Docker.
11. Priorizar código compilável.
12. Criar implementações pequenas, incrementais e testáveis.
13. Sempre que criar uma entidade, criar também configuração EF correspondente.
14. Sempre que criar uma regra crítica, criar teste.
15. Sempre que criar endpoint, documentar exemplo de request/response.

---

## 33. Primeiro objetivo prático do Codex

O primeiro objetivo é entregar um MVP técnico com:

```text
- Solution .NET criada
- Projetos organizados
- Docker Compose funcionando
- SQL Server em container
- API subindo
- Installer subindo
- Health check funcionando
- AppDbContext configurado
- Entidades core criadas
- Primeira migration criada
- ProvisioningService inicial implementado
- Endpoint POST /install/run funcional
- Teste de instalação básica passando
```

---

## 34. Exemplo de InstallRequest

Criar exemplo em `docs/examples/install-request.json`:

```json
{
  "database": {
    "host": "mssql",
    "port": 1433,
    "databaseName": "WhiteLabelErp",
    "username": "sa",
    "password": "YourStrong!Passw0rd",
    "trustServerCertificate": true
  },
  "tenant": {
    "companyName": "Empresa Demo",
    "slug": "empresa-demo",
    "customDomain": null
  },
  "adminUser": {
    "name": "Administrador",
    "email": "admin@demo.com",
    "password": "Admin@123456"
  },
  "branding": {
    "systemName": "ERP Demo",
    "primaryColor": "#2563EB",
    "secondaryColor": "#111827"
  },
  "modules": [
    "core",
    "crm",
    "finance"
  ]
}
```

---

## 35. Comandos de execução esperados

Subir ambiente:

```bash
docker compose up -d --build
```

Ver logs:

```bash
docker compose logs -f api
docker compose logs -f installer
docker compose logs -f mssql
```

Rodar build:

```bash
dotnet build
```

Rodar testes:

```bash
dotnet test
```

Criar migration:

```bash
dotnet ef migrations add InitialCreate \
  --project src/Platform.Persistence \
  --startup-project src/Platform.Api
```

Aplicar migration:

```bash
dotnet ef database update \
  --project src/Platform.Persistence \
  --startup-project src/Platform.Api
```

---

## 36. Documentação técnica a criar

Criar os arquivos:

```text
docs/architecture.md
docs/installer-flow.md
docs/modules.md
docs/database.md
docs/testing.md
docs/docker.md
docs/security.md
docs/roadmap.md
```

Cada documento deve ser simples, objetivo e atualizado conforme a implementação.

---

## 37. Referências técnicas oficiais

Consultar quando necessário:

- .NET support policy:
  - https://dotnet.microsoft.com/en-us/platform/support/policy
- .NET lifecycle:
  - https://learn.microsoft.com/en-us/lifecycle/products/microsoft-net-and-net-core
- Official .NET Docker images:
  - https://learn.microsoft.com/en-us/dotnet/architecture/microservices/net-core-net-framework-containers/official-net-docker-images
- SQL Server Docker:
  - https://learn.microsoft.com/en-us/sql/linux/quickstart-install-connect-docker
- SQL Server Docker persistence/configuration:
  - https://learn.microsoft.com/en-us/sql/linux/sql-server-linux-docker-container-configure
- EF Core Migrations:
  - https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/
- Applying EF Core Migrations:
  - https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/applying

---

## 38. Resultado esperado final

Ao final da primeira grande etapa, o projeto deve permitir:

1. Subir tudo via Docker.
2. Acessar o instalador.
3. Informar banco, empresa, admin e módulos.
4. Criar automaticamente a estrutura do banco.
5. Executar migrations.
6. Criar tenant.
7. Criar admin.
8. Instalar módulos.
9. Bloquear instalador.
10. Fazer login.
11. Consultar módulos ativos.
12. Criar clientes no CRM.
13. Criar lançamentos financeiros básicos.
14. Rodar testes automatizados.

---

## 39. Observação estratégica

Este projeto não deve ser tratado apenas como um CRUD.

Ele deve ser tratado como uma **plataforma base para múltiplos produtos white-label**, com capacidade futura para:

- marketplace de módulos;
- planos comerciais;
- licenciamento;
- atualizações automáticas;
- domínio customizado por cliente;
- múltiplos bancos;
- separação por tenant;
- relatórios;
- API pública;
- integrações;
- app mobile;
- deploy self-hosted;
- deploy SaaS gerenciado.

A fundação precisa nascer limpa, modular e testável.
