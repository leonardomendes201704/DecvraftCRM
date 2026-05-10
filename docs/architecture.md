# Arquitetura

Este documento registra as regras de arquitetura que devem guiar a evolucao do sistema.

## Camadas

- `Platform.Domain`: nucleo de dominio. Nao deve referenciar outros projetos.
- `Platform.Application`: casos de uso, commands/queries, handlers, DTOs e portas. Pode referenciar apenas `Platform.Domain`.
- `Platform.Persistence`: adapter de persistencia. Pode referenciar `Platform.Domain` e `Platform.Application`.
- `Platform.Infrastructure`: adapters tecnicos e implementacoes de portas da Application. Pode referenciar `Platform.Application`, `Platform.Domain` e `Platform.Persistence`.
- `Platform.Api`: adapter HTTP. Deve mapear endpoints, autorizacao e pipeline, delegando casos de uso via MediatR.
- `Platform.Provisioning`: orquestracao de instalacao e bootstrap do sistema.
- `Platform.Modules.*`: metadados e extensoes de modulos. Devem permanecer desacoplados do runtime principal.

## Regras

- Portas de entrada e saida ficam na camada `Application`.
- Implementacoes concretas de infraestrutura ficam fora da `Application`.
- Endpoints HTTP ficam isolados em `Platform.Api/Endpoints`.
- Configuracoes, chaves e credenciais definitivas devem ser entidades persistidas em banco.
- Valores fechados de dominio devem usar enums valorados ou constantes centralizadas.
- Novas dependencias entre projetos devem respeitar os testes de arquitetura em `Platform.ArchitectureTests`.
