# Visao do Projeto

## Produto

Plataforma ERP/CRM white-label, modular, auto instalavel, versionavel e escalavel, baseada em .NET 8, ASP.NET Core, Docker, SQL Server, Entity Framework Core e arquitetura limpa/modular.

Nome conceitual da arquitetura:

> Modular White-label ERP/CRM Provisioning Architecture

Nome interno da plataforma:

> ERP Bootstrap Platform

Nome do motor de instalacao:

> Provisioning Engine

## Objetivo principal

Permitir que um cliente suba a aplicacao em um servidor, acesse `/install` ou `/setup`, informe dados de banco, empresa, usuario administrador, modulos e branding, e o sistema prepare automaticamente a estrutura inicial.

## Resultado esperado do MVP tecnico

Ao final da primeira entrega, o projeto deve permitir:

- subir API, Installer e SQL Server via Docker Compose;
- consultar health check;
- consultar status da instalacao;
- testar conexao com banco;
- executar instalacao inicial;
- criar banco e aplicar migrations;
- criar tenant inicial;
- criar usuario administrador;
- registrar modulos `core`, `crm` e `finance`;
- bloquear o instalador apos instalacao;
- autenticar o administrador via JWT;
- executar testes principais.

## Escopo da primeira versao

Incluido:

- Foundation tecnica da solution .NET;
- multi-tenancy com banco unico e `TenantId`;
- entidades core;
- Provisioning Engine;
- Web Installer;
- autenticacao JWT;
- RBAC inicial;
- modulos Core, CRM basico e Financeiro basico;
- testes automatizados principais;
- documentacao tecnica minima.

Fora do escopo inicial:

- marketplace de modulos;
- licenciamento comercial completo;
- app mobile;
- multi-banco por tenant;
- Redis;
- filas/eventos;
- worker jobs obrigatorios;
- UI administrativa completa;
- billing e planos comerciais.

## Principios do projeto

- A plataforma nao deve virar apenas um CRUD.
- O installer e o provisioning sao parte central da arquitetura.
- Todo dado de negocio deve respeitar isolamento por tenant.
- Modulos devem ser tratados como capacidade instalavel e versionavel.
- A arquitetura deve nascer modular, testavel e compativel com Docker.
- Toda entrega deve compilar e preservar o caminho para deploy self-hosted.
