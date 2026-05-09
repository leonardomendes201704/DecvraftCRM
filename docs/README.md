# Documentacao de Controle do Projeto

Este diretorio e a fonte de verdade para acompanhamento do desenvolvimento da plataforma White-label ERP/CRM.

O objetivo desta documentacao e manter o projeto no trilho desde a fundacao tecnica ate as entregas funcionais, evitando decisoes soltas, escopo implicito e implementacoes fora da arquitetura planejada.

## Como usar estes documentos

Antes de implementar qualquer mudanca relevante:

1. Consulte o roadmap em [roadmap.md](roadmap.md).
2. Localize o epico e a UE correspondente em [backlog.md](backlog.md).
3. Confira os criterios de aceite da entrega em [mvp-checklist.md](mvp-checklist.md).
4. Registre decisoes arquiteturais em [decisions.md](decisions.md).
5. Atualize o status da task conforme ela avancar.

## Documentos principais

- [project-vision.md](project-vision.md): visao do produto, objetivos e limites iniciais.
- [roadmap.md](roadmap.md): fases de entrega e ordem recomendada de execucao.
- [backlog.md](backlog.md): epicos, user stories e tasks tecnicas.
- [mvp-checklist.md](mvp-checklist.md): checklist objetivo da primeira entrega valida.
- [development-process.md](development-process.md): regras de acompanhamento e fluxo de trabalho.
- [decisions.md](decisions.md): registro de decisoes tecnicas e arquiteturais.
- [definition-of-done.md](definition-of-done.md): criterios minimos para considerar uma task concluida.
- [installer-flow.md](installer-flow.md): endpoints e fluxo inicial do Web Installer.
- [auth-flow.md](auth-flow.md): endpoints e exemplos do fluxo de autenticacao.
- [security.md](security.md): autenticacao, JWT, senhas e configuracoes sensiveis.
- [modules.md](modules.md): catalogo inicial de modulos e ativacao por tenant.

## Regra de ouro

Se uma implementacao nao estiver ligada a um epico, UE ou decisao documentada, ela deve ser pausada ate que o documento seja atualizado.
