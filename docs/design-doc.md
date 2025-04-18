# Serviço de fluxo de caixa diário

Um comerciante precisa controlar o seu fluxo de caixa diário com os lançamentos (débitos e créditos), também precisa de um relatório que disponibilize o saldo diário consolidado

# Solução

Um aplicativo web será construído para que o usuário possa visualizar e manter
seu fluxo de caixa através de lançamentos de débito e crédito. Ele também poderá
solicitar e visualizar relatórios de consolidação de saldo diário.

Todo registro será feito por uma API Web que garantirá consistẽncia de regras
de negócio do lado do servidor e manterá os dados em um banco não relacional.

Tanto o aplicativo Web quanto a API Web estarão protegidos por um serviço de
identidade centralizado que usa o protocolo de autenticação/autorização
[OpenID Connect](https://openid.net/developers/how-connect-works).

O cálculo dos dados para relatório consolidado será feito por um serviço à parte
que será executado em segundo plano e independente da API Web, e a comunicação
entre esses componentes será feita através de mensagens enfileiradas. Este
serviço não é crucial, de forma que caso pare de funcionar, a aplicação web
continuará funcionando, porém sem a possibilidade de geração de relatórios
consolidados de saldos diários.

Abaixo temos uma ideia geral dos componentes da solução:

![](images/diagrama-componentes.png)


# Decisões de design

> TODO

# Detalhes do design

> TODO

# Anotações gerais

> TODO