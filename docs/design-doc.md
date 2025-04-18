# Serviço de fluxo de caixa diário

Um comerciante precisa controlar o seu fluxo de caixa diário com os lançamentos
(débitos e créditos), também precisa de um relatório que disponibilize o saldo
diário consolidado

# Solução

Um aplicativo web será construído para que o usuário possa visualizar e manter
seu fluxo de caixa através de lançamentos de débito e crédito, além de poder
solicitar e visualizar relatórios de consolidação de saldo diário.

Todo registro será feito por uma API Web que garantirá consistência de regras
de negócio do lado do servidor e manterá os dados em um banco não relacional.

Tanto o aplicativo Web quanto a API Web estarão protegidos por um serviço de
identidade centralizado que usa o protocolo de autenticação/autorização
[OpenID Connect][OPENID_CONNECT].

O cálculo dos dados para o relatório consolidado será feito por um serviço
executando em segundo e de forma independente da API Web. A comunicação
entre esses componentes será feita através de um mecanismo assíncrono de
mensagens.

Caso o serviço de cálculo de dados para os relatórios pare de funcionar, a
aplicação web continuará funcionando, porém sem a possibilidade de geração de
relatórios consolidados de saldo diário.

Abaixo temos uma ideia geral dos componentes da solução:

![](images/diagrama-componentes.png)


# Decisões de design

Quanto a tecnologias:

- Usaremos [Keycloak][KEYCLOAK] como serviço de identidade por
se tratar de um componente de mercado já pronto para produção e que implementa
de forma certificada o padrão [OpenID Connect][OPENID_CONNECT] como precisamos
- Usaremos o [MongoDB][MONGODB] como solução de banco de dados não relacional
por ser uma ferramenta pronta para produção e bastante conhecida no mercado
- Usaremos o [RabbitMQ][RABBITMQ] como [*Message broker*][MESSAGE_BROKER] que
gerenciará nossa fila de mensagem para comunicação entre os serviços

# Detalhes do design

> TODO

# Anotações gerais

> TODO


[OPENID_CONNECT]: https://openid.net/developers/how-connect-works
[KEYCLOAK]: https://www.keycloak.org
[MONGODB]: https://www.mongodb.com
[RABBITMQ]: https://www.rabbitmq.com
[MESSAGE_BROKER]: https://en.wikipedia.org/wiki/Message_broker