# Serviço de fluxo de caixa diário

Um comerciante precisa controlar o seu fluxo de caixa diário com os lançamentos
(débitos e créditos), também precisa de um relatório que disponibilize o saldo
diário consolidado.

## Requisitos de negócio

- Serviço que faça o controle de lançamentos
- Serviço do consolidado diário

## Solução

Será construída uma solução para *Fluxo de Caixa* que contenha:

- Um aplicativo web para que o usuário possa visualizar e manter seu fluxo de
  caixa através de lançamentos de débito e crédito, além de poder solicitar e
  visualizar relatórios de consolidação de saldo diário.

- Todo registro será feito por uma API Web que garantirá consistência de regras
  de negócio do lado do servidor e manterá os dados em um banco não relacional.

- O cálculo dos dados para o relatório consolidado será feito por um serviço
  executando em segundo plano e de forma independente. A comunicação
  entre esses componentes será feita através de um mecanismo assíncrono de
  mensagens.

- Tanto o aplicativo Web quanto a API Web estarão protegidos por um serviço de
  identidade centralizado que usa o protocolo de autenticação/autorização
  [OpenID Connect][OPENID_CONNECT].

> Caso o serviço de consolidado pare de funcionar, a aplicação web continuará
> funcionando, porém sem a possibilidade de geração de relatórios consolidados
> de saldo diário.

Abaixo temos uma ideia geral dos componentes da solução e a interação entre eles:

![](images/diagrama-componentes.png)

## Detalhes da solução

Devem ser implementados os seguintes casos de uso:

![](images/casos-de-uso.png)

### Autenticar usuário

![](images/caso-de-uso-autenticar-usuario.png)

O usuário não se autenticará diretamente na aplicação web, ao invés disso a
mesma irá redirecionar o usuário para se autenticar no serviço de identidade
[Keycloak][KEYCLOAK], que cuidará de toda a parte de segurança para validar
corretamente a identidade do usuário, e ao final redirecioná-lo para a aplicação.

### Registrar lançamento

![](images/caso-de-uso-registrar-lancamento.png)

O usuário autenticado poderá registrar um lançamento informando seus dados:

- Data e hora do lançamento
- Descrição do lançamento
- Tipo: _Crédito_ ou _Débito_
- Valor do lançamento

Esses serão registrados por uma Web API para garantir a integridade da
informação, considerando:

- O registro deve estar sempre vinculado ao usuário dono. E um usuário não pode
  enxergar os dados de outros usuários.

- Não deve ser permitido o registro de mais de um lançamento com mesmo tipo,
  descrição, data e hora

- Caso hajam cosolidações já registradas à partir do dia do lançamento, essas
  devem ser invalidadas, e mensagens correspondentes para o serviço
  **Consolidado** devem ser emitidas para que o cálculo seja refeito.

### Visualizar lançamentos

![](images/caso-de-uso-visualizar-lancamentos.png)

O usuário autenticado poderá visualizar os lançamentos que já fez. Esses serão
servidos pela API, obrigatoriamente entre um período determinado de até 90
dias. O resultado deve ser dinamicamente paginado com até 5 registros por
página de resultado, além de serem ordenados por data/hora. Essas regras visam
evitar sobrecarga do serviço durante as pesquisar.

### Calcular consolidação diária

![](images/caso-de-uso-calcular-consolidacao-diaria.png)

O usuário autenticado poderá solicitar a visualização do consolidado diário em
um dia específico. Não iremos fazer o cálculo do saldo consolidado em tempo de
consulta por questões de _performance_. Ao invés disso, o usuário deve informar
o dia que deseja ver o saldo consolidado, e uma entrada será criada para indicar
isso, porém o saldo permanecerá pendente até que seja calculado de fato por um
serviço em segundo plano.

O serviço saberá o que calcular através de mensagens recebidas da Web API
via fila em um [_broker de mensagens_][MESSAGE_BROKER].

### Visualizar consolidação diária

![](images/caso-de-uso-visualizar-consolidacao-diaria.png)

O usuário autenticado poderá visualizar o saldo consolidado diário que já foi
calculado anteriormente. Para isso precisará selecionar um dia que já tenha
solicitado o cálculo, e então visualizar os dados consolidados. Caso acesse um
dia cujo os dados ainda não tenham sido calculados, ele será informado dessa
situação para que aguarde e tente visualizar novamente mais tarde.

> O dado pode não estar disponível caso nunca tenha sido calculado anteriormente,
> ou caso o serviço de consolidação esteja indisponível, ou ainda se o dado já
> tenha sido calculado, mas por uma modificação nos registros daquele dia o
> saldo tenha sido invalidado, o que o coloca na fila para recalculo
> automaticamente.

### Diagrama de infraestrutura na AWS

Usaremos os recursos da núven pública da [AWS][AWS] para atender os requisitos
de segurança, escalabilidade e redudância que precisamos. No diagrama abaixo
temos um exemplo de como se daria aessa implantação.

Vale ressaltar que escolhemos [AWS][AWS] por conveniência, mas o mesmo se aplica
em outras nuvens como [GCP][GCP], [Azure][AZURE] ou [OCI][OCI] com algumas
poucas mudanças.

![](images/infraestrutura-aws.png)

No diagrama podemos ver principalmente como aplicamos as restrições de segurança
quanto ao tráfego de entrada permitido. Combinamosm um _Web Application Firewall_
e o _CloudFront_ para garantir a proteção contra ataques e a segurança do
tráfego criptografado com _TLS/HTTPS_.

Um _Load Balancer_ se comunica com o _Container Service_ em conjunto com grupos
de _auto scalling_ para entregar as demandas requisitadas, e escalar
horizontalmente nossos _containers_ com base na necessidade.

Nossas aplicações reais estão protegidas em redes privadas, e o tráfego ao mundo
externo é controlado através de configurações de NAT e rígidos grupos de
segurança.

Aqui nós substituímos nossos containers de bancos relacionais (Keycloak DB) pelo
serviço gerenciado compatível, e o mesmo ocorre para filas do _RabbitMQ_, e
nossas bases não relacionais do _MongoDB_.

Vale ressaltar que optamos pelo serviço de _cluster gerenciado_ do próprio
_MongoDB_ chamado [_MongoDB Atlas_][MONGODB_ATLAS] porque é totalmente compatível
com as núvens públicas mencionadas, e a segurança é garantida através de
emparelhamento seguro de _VPC_.

Também temos a computação distribuída em duas zonas de disponibilidade (o que
pode ser mais dependendo de atualização nos requisitos) para atender ao requisito
de alta disponibilidade, onde os serviços são configurados para estarem em
execução simultaneamente em pelo menos essas duas zonas.

Também exemplificamos o uso na região de _São Paulo_ caso tenhamos que atender
a requisitos regulatórios de uso de dados em território nacional apenas.

<!-- links -->
[OPENID_CONNECT]: https://openid.net/developers/how-connect-works
[KEYCLOAK]: https://www.keycloak.org
[MONGODB]: https://www.mongodb.com
[MESSAGE_BROKER]: https://en.wikipedia.org/wiki/Message_broker
[WAF]: https://pt.wikipedia.org/wiki/Web_Application_Firewall
[MONGODB_ATLAS]: https://www.mongodb.com/atlas
[AWS]:
  <https://aws.amazon.com/pt>
  "Amazon Web Services"
[GCP]:
  <https://cloud.google.com>
  "Google Cloud Platform"
[OCI]:
  <https://www.oracle.com/br/cloud/compute>
  "Oracle Cloud Infrastructure"
[AZURE]:
  <https://azure.microsoft.com/pt-br/>
  "Serviços de nuvem do Microsoft Azure"  
