# Processo de Desenvolvimento

Este processo existe para manter o projeto alinhado com a documentacao do inicio ate a entrega.

## Fluxo padrao de trabalho

1. Escolher uma task em [backlog.md](backlog.md).
2. Confirmar que ela pertence a uma UE e epico.
3. Atualizar o status da task para `Doing`.
4. Implementar a menor entrega coerente possivel.
5. Rodar verificacoes aplicaveis.
6. Atualizar checklist ou docs afetados.
7. Mover a task para `Review` ou `Done`.
8. Registrar na comunicacao final qual e o proximo passo recomendado.

## Regras de controle

- Nenhuma funcionalidade relevante deve ser implementada sem task correspondente.
- Toda decisao arquitetural relevante deve ser registrada em [decisions.md](decisions.md).
- Toda entidade nova deve ter configuracao EF correspondente.
- Toda regra critica deve ter teste.
- Todo endpoint novo deve ter exemplo documentado.
- Toda chave, credencial ou configuracao definitiva deve ser modelada para persistencia em banco.
- Valores fechados de dominio devem usar enums valorados, value objects ou constantes centralizadas, nao valores hardcoded espalhados.
- Toda entrega P0 deve manter `dotnet build` funcionando.
- O escopo do MVP tecnico tem prioridade sobre funcionalidades P1 e P2.

## Criterios para abrir nova task

Criar nova task quando:

- uma implementacao exige mais de um commit logico;
- uma decisao muda o escopo original;
- surge um risco tecnico que precisa ser tratado separadamente;
- uma UE fica grande demais para acompanhar;
- um bug bloqueia criterio de aceite.

## Criterios para bloquear uma task

Marcar como `Blocked` quando:

- falta decisao tecnica;
- falta dependencia externa;
- a task conflita com a arquitetura documentada;
- a implementacao exige mudanca de escopo;
- testes essenciais nao podem rodar por limitacao conhecida.

## Cadencia sugerida

A cada ciclo de desenvolvimento:

- revisar tasks `Doing`;
- concluir ou desbloquear tasks abertas;
- atualizar checklist do MVP;
- registrar decisoes novas;
- validar build/testes quando aplicavel.

## Ordem de prioridade

1. Compilacao da solution.
2. Docker e infraestrutura local.
3. Persistencia e migrations.
4. Provisioning Engine.
5. Installer.
6. Autenticacao.
7. Testes P0.
8. CRM e Financeiro basicos.
