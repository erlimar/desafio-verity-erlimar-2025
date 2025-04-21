# Decisões de design

Aqui temos mais detalhes sobre algumas decisões tomadas e seus porquês. Elas 
podem ser revistas a qualquer momento, mas precisam ser conhecidas para entender
o impacto na solução final.

## Quanto a tecnologias:

- Optamos por algumas soluções _open source_ de mercado pela flexibilidade de
  ter um ambiente de desenvolvimento relativamente simples e pronto para uso,
  quanto também ter a possibilidade de implantá-los em produção em uma
  configuração que permita escalabilidade já contida nos próprios produtos,
  além de serem certificadas no mercado em ambientes de produção real que
  atendem aos nossos requisitos se segurança. Por fim, também consideramos
  ferramentas que já tenham equivalentes prontos para uso em escala global nas
  nuvens públicas. 
  - Usaremos [Keycloak][KEYCLOAK] como serviço de identidade certificada no
    padrão [OpenID Connect][OPENID_CONNECT]
  - Usaremos o [MongoDB][MONGODB] como solução de banco de dados não relacional.
    A escalabilidade será entregue por sua própria nuvem pública
    [MongoDB Atlas][MONGODB_ATLAS]
  - Usaremos o [RabbitMQ][RABBITMQ] como [*Message broker*][MESSAGE_BROKER] que
    gerenciará nossa fila de mensagem para comunicação entre os serviços. A
    escalabilidade será entregue pelo serviço de nuvem pública equivalente
     _AWS MQ_.

## Porque escolhemos RabbitMQ ao invés de Apache Kafka na mensageria

O [Apache Kafka][APACHE_KAFKA] também atenderia nossas necessidade, porém seria
mais indicado a cenários mais complexos do que o exigido atualmente.

Quanto a volume, ambas as ferramentas atendem ao que se pretende, mas é razoável
considerar a forma como as ferramentas ordenam as mensagens, justamente devido
sua natureza de aplicação. Enquanto por padrão o [Apache Kafka][APACHE_KAFKA]
entrega alta velocidade e volume, permitindo reprocessamento até, faz isso sem
garantia de relação entre ordem de envio e recebimento das mensagens, enquanto
queo RabbitMQ garante por padrão que as mensagens sejam entregues na ordem que
foram criadas. Isso se encaixa com o que precisamos para a solução, pois é
adequado que quem solicitou um consolidado primeiro seja atendido primeiro.

Esse comportamento pode ser configurado, mas entendemos que
[Apache Kafka][APACHE_KAFKA] não seria a melhor opção aqui.

## Nem todos os reauisitos de proteção e performance estarão na própria aplicação

Para atender aos requisitos de proteção e performance, optamos por soluções de
nuvem ao invés de implementá-las nós mesmos na própria aplicação por se tratar
de um requisito crítico. As ferramentas de nuvem já estão prontas para produção
além de serem certificadas constantemente e estarem distribuídas em alta escala.
Os custos para implementar nós mesmos não justificariam a escolha ante um
mecanismo já disponível.

Alguns exemplos:

- O _AWS CloudFront_ junto ao _AWS WAF_ e _AWS Certificate Manager_ nos entrega
  a criptografia de comunicação TLS/HTTPS que precisamos, além de cache e
  proteção contra ataques altamente configuráveis
- O _AWS Elastic Load Balancer_ junto a _AWS Container Service_ nos dá o
  _autoscaling_ horizontal para alta performance e alta disponibilidade
- Os serviços gerenciados para fila (_Amazon MQ_), bancos relacionais
  (_Amazon RDS_) e não relacionais (_MongoDB Atlas_) nos entregam a segurança
  e alta disponibilidade, além de redundância e failover nativos

## Devido ao prazo optamos por implementações mais simples

Essas servem como anotações para possíveis melhorias a serem implementadas na
solução.

- Não implementamos um mecanismo de registro de usuários devido ao tempo
  disponível para implementação da solução, portanto está fora do escopo.
  Mas em uma situação real ou a aplicação permitiria o *auto-registro* de
  usuários, ou um outro serviço faria essa gestão (*back office* por exemplo).
  Por hora faremos o registro de usuários diretamente no console do
  [Keycloak][KEYCLOAK].
- A implementação atual não entrega artefatos nem guias para implantação em
  ambiente de produção devido ao tempo disponível para concepção da solução.
  Mas uma ideia de como seria a infraestrutura de produção é apresentada para
  que se tenha a visão de que alguns requisitos da aplicação são atendidos
  através de outros componentes que não estão presentes em código de aplicativo.
  Tais como WAF para proteção contra ataques, balanceamento de carga e múltiplas
  regiões de implantação para alta disponibilidade.
- Usamos um banco [MongoDB][MONGODB] compartilhado entre a Web API e o _worker_
  Consolidado por questões de prazo, mas em uma situação ideal cada serviço usaria
  sua própria base e fariamos a sincronização através de mensagens com serviços
  lado a lado (_side car_).
- Por questões de prazo não otimizamos a _interface do usuário_ para experiência.
  O aguardar a geração do relatório pode exigir atualização explícita de tela, o
  que não é bom para o usuário em um caso real. Nessas situações usaríamos o
  protocolo [_Web Socket_][WEBSOCKET] para atualizar a _interface do usuário_
  em tempo real, de acordo com que o relatório esteja sendo gerado, a aqui
  uma exibição de progresso cairia muito bem.
- No exemplo da infraestrutura da solução na [AWS][AWS] não distinguimos recursos
  específicos para aplicação de _frontend_, mas seria uma das melhorias a se
  fazer, ou seja, servir os _frontends_ em serviços com suporte [_CDN_][CDN]
  como _CloudFront_ ao invés de _containers_ regulares.
- No exemplo da infraestrutura da solução na [AWS][AWS] também optamos pelo
  serviço de gerenciamento de containers _ECS_ ao invés da plataforma de
  orquestração [Kubernetes][KUBERNETES] pela simplicidade da aplicação, mas de
  acordo com que a aplicação cresca talvez seja necessário migrar.

<!-- links -->
[OPENID_CONNECT]: https://openid.net/developers/how-connect-works
[WEBSOCKET]: https://developer.mozilla.org/pt-BR/docs/Web/API/WebSockets_API
[KEYCLOAK]: https://www.keycloak.org
[MONGODB]: https://www.mongodb.com
[RABBITMQ]: https://www.rabbitmq.com
[MESSAGE_BROKER]: https://en.wikipedia.org/wiki/Message_broker
[MONGODB_ATLAS]: https://www.mongodb.com/atlas
[APACHE_KAFKA]: https://kafka.apache.org/
[AWS]:
  <https://aws.amazon.com/pt>
  "Amazon Web Services"
[CDN]: https://en.wikipedia.org/wiki/Content_delivery_network
[KUBERNETES]: https://kubernetes.io/pt-br/