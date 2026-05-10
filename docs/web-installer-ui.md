# Frontend do Web Installer

Este documento define a arquitetura e o fluxo visual do instalador web.

## Objetivo

Criar uma interface guiada, em formato wizard, para executar a instalacao inicial da plataforma sem uso manual de endpoints.

O instalador deve ser simples, seguro e objetivo: coletar dados minimos, validar cada etapa, executar o provisioning e bloquear novas instalacoes apos sucesso.

## Projeto

Projeto alvo:

- `src/Platform.WebInstaller`

Arquitetura recomendada:

```text
Platform.WebInstaller/
  Pages/
    Install/
      Index.cshtml
      Database.cshtml
      Tenant.cshtml
      Admin.cshtml
      Branding.cshtml
      Modules.cshtml
      Review.cshtml
      Complete.cshtml
  Services/
    IInstallerWizardStateStore.cs
    SessionInstallerWizardStateStore.cs
  ViewModels/
    InstallWizardState.cs
    DatabaseStepViewModel.cs
    TenantStepViewModel.cs
    AdminStepViewModel.cs
    BrandingStepViewModel.cs
    ModulesStepViewModel.cs
    ReviewStepViewModel.cs
  Components/
    InstallerStepIndicator/
    InstallerValidationSummary/
```

Status da UE-14.01:

- Razor Pages habilitado no `Platform.WebInstaller`.
- Layout compartilhado criado em `Pages/Shared/_Layout.cshtml`.
- Indicador reutilizavel criado em `Pages/Shared/_InstallerStepIndicator.cshtml`.
- Estado temporario criado em `InstallWizardState` e armazenado via session protegida do ASP.NET Core.
- Rotas visuais criadas de `/install` ate `/install/complete`.

Status das UE-14.02 e UE-14.03:

- Etapas do wizard implementadas com validacoes server-side.
- Tela de banco testa conexao usando `IDatabaseProvisioner`.
- Tela de revisao executa o `ProvisioningService`.
- O provisioning troca o `DbContext` para o banco informado no wizard antes de migrations, seeds e criacao do tenant.
- Tela de conclusao exibe tenant, administrador e modulos instalados sem expor senha.
- Dados sensiveis do wizard sao limpos da session apos instalacao bem-sucedida.
- Acesso direto as etapas e bloqueado quando a instalacao ja esta concluida.

## Padrao de UI

- Usar Razor Pages para o wizard.
- Cada etapa deve ter uma PageModel propria.
- Componentes reutilizaveis devem ficar em `Components` ou `ViewComponents`.
- ViewModels de tela nao devem ser os DTOs internos do provisioning.
- A PageModel deve delegar regras para services, nao acessar banco diretamente.
- O service do wizard pode chamar contratos do provisioning diretamente, pois o WebInstaller e o host do fluxo de instalacao.

## Fluxo do Wizard

### 1. Boas-vindas

Rota sugerida:

- `GET /install`

Responsabilidades:

- Consultar status da instalacao.
- Se ja instalado, bloquear wizard e exibir estado instalado.
- Se nao instalado, permitir inicio do fluxo.

### 2. Banco de dados

Rota sugerida:

- `GET /install/database`
- `POST /install/database`

Campos:

- Host.
- Porta.
- Nome do banco.
- Usuario.
- Senha.
- Trust server certificate.

Acoes:

- Testar conexao.
- Criar banco automaticamente durante a instalacao, quando ainda nao existir.
- Persistir dados temporarios no estado do wizard.

Observacao:

- Dados sensiveis do wizard sao temporarios e usados apenas durante o bootstrap.
- Configuracoes definitivas da plataforma devem continuar persistidas como entidades em banco.

### 3. Tenant inicial

Rota sugerida:

- `GET /install/tenant`
- `POST /install/tenant`

Campos:

- Nome da empresa.
- Slug.

Validacoes:

- Nome obrigatorio.
- Slug obrigatorio, normalizado e amigavel para URL.

### 4. Administrador

Rota sugerida:

- `GET /install/admin`
- `POST /install/admin`

Campos:

- Nome.
- Email.
- Senha.
- Confirmacao de senha.

Validacoes:

- Email valido.
- Senha com politica minima definida pelo dominio.
- Confirmacao igual a senha.

### 5. Branding

Rota sugerida:

- `GET /install/branding`
- `POST /install/branding`

Campos:

- Nome comercial.
- Cor primaria.
- Cor secundaria.
- Logo em bloco futuro.

Validacoes:

- Cores em formato valido.
- Nome comercial obrigatorio.

### 6. Modulos

Rota sugerida:

- `GET /install/modules`
- `POST /install/modules`

Comportamento:

- `core` sempre obrigatorio e bloqueado para desmarcar.
- `crm` opcional.
- `finance` opcional.

### 7. Revisao

Rota sugerida:

- `GET /install/review`
- `POST /install/review`

Responsabilidades:

- Mostrar resumo sem exibir senha em claro.
- Confirmar execucao.
- Chamar provisioning.
- Exibir progresso ou estado final.

### 8. Conclusao

Rota sugerida:

- `GET /install/complete`

Responsabilidades:

- Mostrar sucesso.
- Mostrar tenant criado.
- Mostrar link para login.
- Orientar que o instalador foi bloqueado.

## Estado do Wizard

Modelo sugerido:

```text
InstallWizardState
  Database
  Tenant
  Admin
  Branding
  Modules
```

Persistencia do estado:

- Preferir session/cookie temporario protegido para o estado do wizard.
- A UE-14.01 usa `SessionInstallerWizardStateStore` com cookie HTTP-only e essencial.
- Nao persistir senha em arquivo.
- Nao salvar credenciais definitivas em appsettings ou env.
- Limpar estado apos instalacao concluida ou cancelamento.

## Design e Experiencia

- Layout focado em formulario e progresso.
- Step indicator no topo ou lateral.
- Validacao por etapa.
- Botoes: Voltar, Continuar, Testar conexao, Instalar.
- Revisao final antes de executar.
- Estados claros de erro, sucesso e carregamento.
- Nao usar landing page; primeira tela deve ser o inicio do wizard.

## Criterios de Aceite

- Usuario consegue executar instalacao completa pelo navegador.
- Wizard bloqueia acesso quando sistema ja estiver instalado.
- Teste de banco funciona antes da execucao.
- Erros de validacao aparecem na propria etapa.
- Senha do admin nao aparece na revisao.
- Instalacao bem-sucedida redireciona para conclusao.
- `dotnet test` continua passando.

## Fora do Primeiro Bloco

- Upload real de logo.
- Tema visual avancado.
- Barra de progresso em tempo real via SignalR.
- Multi-idioma.
- Recuperacao de wizard abandonado apos reinicio do servidor.

## Proximo Bloco

Iniciar a `EP-15 - Frontend Web MVC/Razor`, criando a aplicacao autenticada operacional.
