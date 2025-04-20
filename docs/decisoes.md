## Decisões de design

Quanto a tecnologias:

- Usaremos [Keycloak][KEYCLOAK] como serviço de identidade por se tratar de um
  componente de mercado já pronto para produção e que implementa de forma
  certificada o padrão [OpenID Connect][OPENID_CONNECT] como precisamos
- Usaremos o [MongoDB][MONGODB] como solução de banco de dados não relacional
  por ser uma ferramenta pronta para produção e bastante conhecida no mercado
- Usaremos o [RabbitMQ][RABBITMQ] como [*Message broker*][MESSAGE_BROKER] que
  gerenciará nossa fila de mensagem para comunicação entre os serviços.
  - Porque não usamos [Apache Kafka]? Entendemos que o Kafka atende a cenários
    mais complexos, que a aplicação não exige atualmente. Quanto a volume, ambas
    as ferramentas atendem ao que se pretende. É razoável considerar também a
    forma como as ferramentas ordenam as mensagens, e como RabbitMQ garante por
    padrão que as mensagens sejam entregues na ordem que foram criadas (mesmo
    que configurações permitam mudar esse comportamento), isso se encaixa com o
    que precisamos para a solução, pois é adequado que quem solicitou um
    consolidado primeiro seja atendido primeiro na fila.
- Para atender aos requisitos de proteção e performance, optamos por soluções de
  nuvem ao invés de implementá-las nós mesmos na própria aplicação por se tratar
  de um requisito crítico. As ferramentas de nuvem já estão prontas para
  produção além de serem certificadas constantemente e estarem distribuídas em
  alta escala. Os custos para implemtnar nós mesmos não justificariam a escolha
  ante um mecanismo já disponível.
  - O AWS CloudFront nos entrega a criptografia de comunicação TLS/HTTPS 
    obrigatoriamente, além de cache e WAF para nos garantir a segurança
  - O AWS Elastic Load Balancer junto a grupos de autoscaling nos entregam a alta
    disponibilidade

<!-- links -->
[OPENID_CONNECT]: https://openid.net/developers/how-connect-works
[KEYCLOAK]: https://www.keycloak.org
[MONGODB]: https://www.mongodb.com
[RABBITMQ]: https://www.rabbitmq.com
[MESSAGE_BROKER]: https://en.wikipedia.org/wiki/Message_broker