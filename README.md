# Desafio Verity 2025

Este repositório contém os artefatos de desenvolvimento para desafio proposto
por [Verity](https://www.verity.com.br) em vaga de processo seletivo destinado
a função de _Arquiteto de Software_.

Você pode saber mais sobre o desafio em [DESCRICAO_DESAFIO.md](DESCRICAO_DESAFIO.md),
e conhecer a solução proposta em [docs/design-doc.md](docs/design-doc.md), bem
como saber sobre as decisões de design em [docs/decisoes.md](docs/decisoes.md).

# Ambiente de desenvolvimento local

Você vai precisar de:

- [.NET SDK 9.0](https://dotnet.microsoft.com/pt-br/download/dotnet/9.0)
- [Docker (com Compose)](https://docs.docker.com/compose/)
- [Node.js 22.0 (Com NPM 10.9)](https://nodejs.org/pt)
- [Angular CLI 19](https://angular.dev/tools/cli)
  - `npm install -g @angular/cli`

## Passo a passo para preparar o ambiente

1) Crie o arquivo `.env` com variáveis de ambiente para seus acessos:
```ini
KC_BOOTSTRAP_ADMIN_USERNAME="Nome do usuário adminisadmintrador do Keycloak"
KC_BOOTSTRAP_ADMIN_PASSWORD="Senha do usuário admin do Keycloak"
KEYCLOAK_DB="Nome do banco do Keycloak"
POSTGRES_USER="Nome do usuário Postgres para Keycloak"
POSTGRES_PASSWORD="Senha do usuário Postgres pra Keycloak"
MONGODB_USERNAME="Nome do usuário do MongoDB"
MONGODB_PASSWORD="Senha do usuário do MongoDB"
RABBITMQ_USERNAME="Nome do usuário do RabbitMQ"
RABBITMQ_PASSWORD="Senha do usuário do RabbitMQ"
DESAFIO_VERITY_WEBAPI_SECRET="Segredo da API da solução"
```

Exemplo:
```ini
KC_BOOTSTRAP_ADMIN_USERNAME=keycloak-admin-user
KC_BOOTSTRAP_ADMIN_PASSWORD=12f8bcc7b009318133ba6ddd
KEYCLOAK_DB=keycloak
POSTGRES_USER=postgres-user
POSTGRES_PASSWORD=3889a314b7402aba80bc335a
MONGODB_USERNAME=mongodb-user
MONGODB_PASSWORD=ea526ff955a5e21d7d6b9057
RABBITMQ_USERNAME=rabbitmq-user
RABBITMQ_PASSWORD=4fea21a05b99f27f0414d642
DESAFIO_VERITY_WEBAPI_SECRET=63018ff4e991c64c2fc0157f732ae722d1da0f382bf6acea
```

> Gere uma senha aleatória com ajuda do OpenSSL: Ex: 32 caracteres em hexadecimal
> - `openssl rand -hex 32`

Você usará esses dados para acessar o Keycloak, MongoDB, Rabbiq MQ e terá o
segredo da API no Keycloak via OpenID Connect.

Quando tudo estiver disponível (pode levar alguns minutos) você terá:

- Interface de administração do Keycloak: http://localhost:8080
- Interface de administração do RabbitMQ: http://localhost:15672
- Banco de dados Postgres do Keycloak em `localhost:5432`
- Banco de dados MongoDB em `localhost:27017`

> Se você tiver problemas com as portas usadas, porque já as usa em outras
> em outras aplicações e não pode se desfazer delas, então altere as configurações
> de portas dos serviços no arquivo `docker-compose.yuml`.

2) Levante os serviços necessários como requisitos
```sh
docker compose up -d
```

Mesmo quando os serviços já estiverem iniciados, demora um tempo até que tudo
esteja pronto para uso. Então verifique os logs até que veja as mensagems informando
que os serviços já estão prontos.

```sh
# Escutando todos os logs
docker compose logs -f

# Escutando os logs de um único serviço
docker compose logs [nome-servico] -f
```

O serviço que mais demora para ficar pronto normalemente é o _Keycloak_, e você
saberá que ele já está pronto quando ver uma linha de log parecida com essa:

```log
keycloak-1 [...] Listening on: http://0.0.0.0:8080 [...]
```

> O parâmetro `-f` ao final trava a tela para mostrar os logs em tempo real,
> então use um `Ctrl + C` para encerrar o processo, ou não use o parâmetro
> `-f` nos comandods.

3) Atualize os segredos do usuário para os projetos 

Agora você deve usar os valores no arquivo `.env` para preencher algumas
variáveis nos arquivos de configuração específicos de cada projeto.
Para isso temos o script PowerShell `eng/update-user-secrets.ps1` pra te
ajudar com essa tarefa.

```powershell
.\eng\update-user-secrets.ps1
```

> Pronto! Agora você já pode abris os projetos e codificar como preferir

- `components/service` - Solução .NET | Visual Studio (VSCode) com backend
- `components/ui/fluxo-caixa` - Aplicação Angular Node.js

Recomendamos abrir cada uma separadamente em seu editor favorito para editar
e depurar, bem como executar de forma independente cada componente. Assim
você pode simular situações onde alguns componentes ficam indisponíveis.