## Decisões de design

Quanto a tecnologias:

- Usaremos [Keycloak][KEYCLOAK] como serviço de identidade por
se tratar de um componente de mercado já pronto para produção e que implementa
de forma certificada o padrão [OpenID Connect][OPENID_CONNECT] como precisamos
- Usaremos o [MongoDB][MONGODB] como solução de banco de dados não relacional
por ser uma ferramenta pronta para produção e bastante conhecida no mercado
- Usaremos o [RabbitMQ][RABBITMQ] como [*Message broker*][MESSAGE_BROKER] que
gerenciará nossa fila de mensagem para comunicação entre os serviços