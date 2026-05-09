# Agent Guidelines

Estas diretrizes devem ser assumidas por qualquer agente ou desenvolvedor trabalhando neste repositorio.

## Diretriz obrigatoria de configuracao e credenciais

Toda chave, credencial, segredo, parametro sensivel ou configuracao operacional da plataforma deve ser persistida e gerenciada por entidades em banco de dados.

Nao usar arquivos `.env`, `appsettings*.json` ou valores hardcoded como fonte definitiva de configuracao da aplicacao.

Excecoes permitidas apenas para bootstrap minimo:

- dados estritamente necessarios para iniciar o processo de instalacao;
- valores temporarios de desenvolvimento sem segredo real;
- exemplos documentais em `.env.example`;
- configuracao tecnica inevitavel do host/container antes do banco existir.

Mesmo nas excecoes, nenhum segredo real deve ser commitado.

## Implicacoes arquiteturais

- Configuracoes globais devem ser modeladas como entidades de sistema.
- Configuracoes por tenant devem ser modeladas como entidades vinculadas ao tenant.
- Segredos devem ser armazenados de forma protegida, nunca em texto puro.
- O Provisioning Engine deve criar as configuracoes iniciais no banco.
- `appsettings*.json` deve conter apenas configuracao minima de bootstrap e ambiente local.
- `.env.example` deve ser tratado como exemplo de execucao, nao como fonte final de verdade.

## Fluxo de desenvolvimento

Antes de implementar algo, declarar o que sera feito.

Ao final, declarar:

- o que foi feito;
- quais arquivos principais foram alterados;
- como validar;
- qualquer bloqueio ou risco restante;
- qual e o proximo passo recomendado.
