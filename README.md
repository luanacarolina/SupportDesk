🛠️ SupportDesk

Sistema de gerenciamento de chamados técnicos.

 Descrição do Projeto

O SupportDesk é uma aplicação fullstack que permite o cadastro, consulta, filtragem e atualização de chamados técnicos.

A solução também conta com processamento assíncrono utilizando mensageria(RabbitMq), onde chamados finalizados são enviados para uma fila e processados por um Worker, que registra notificações em um banco NoSQL(MongoDB).

🚀 Tecnologias Utilizadas
Backend
.NET 8 (ASP.NET Core Web API)
Entity Framework Core
PostgreSQL
JWT Authentication
Mensageria
RabbitMQ
Worker
.NET Worker Service
MongoDB
Frontend
React
Infraestrutura
Docker / Docker Compose

⚙️ Como Executar o Projeto
1. Subir infraestrutura (bancos e mensageria)
docker-compose up -d
2. Executar API
cd src/SupportDesk.Api
dotnet run

Swagger:
http://localhost:5221/swagger

3. Executar Worker
cd src/SupportDesk.Worker
dotnet run
4. Executar Frontend
cd frontend
npm install
npm start

Aplicação:
http://localhost:3000

🔐 Autenticação

A aplicação utiliza autenticação JWT.

Usuário de teste:

{
  "username": "admin",
  "password": "123456"
}


Estrutura dos Dados
PostgreSQL
Tabela: support_tickets
MongoDB
Coleção: ticket_notifications

Comunicação Assíncrona
Fila: support-ticket-completed
Producer: API
Consumer: Worker

Explicações Técnicas

O projeto foi estruturado seguindo boas práticas de arquitetura em camadas:

API → Exposição de endpoints RESTful utilizando corretamente métodos HTTP e organização por recursos
Application → Regras de negócio e serviços
Domain → Entidades do sistema
Infrastructure → Acesso a dados e integrações externas
Padrões utilizados
Repository Pattern
Injeção de Dependência
Separação de responsabilidades (SRP)
Comunicação assíncrona via mensageria

Princípios SOLID
SRP (Single Responsibility Principle)

Cada classe possui uma única responsabilidade, como serviços de negócio, repositórios e publishers de mensageria.

DIP (Dependency Inversion Principle)

As dependências são baseadas em interfaces (ex: ISupportTicketRepository, ITicketCompletedPublisher), permitindo baixo acoplamento e facilidade de troca de implementação.

Organização da camada de API

A camada de API foi mantida enxuta e focada exclusivamente na exposição de endpoints RESTful e no tratamento de requisições HTTP.

Algumas classes auxiliares foram mantidas nesta camada por estarem diretamente relacionadas à borda da aplicação, como:

AuthService: responsável pela validação de credenciais de autenticação
JwtTokenService: responsável pela geração de tokens JWT
AuthUser: modelo utilizado para representar usuários configurados na aplicação

Esses componentes não contêm regras de negócio do domínio, sendo utilizados apenas para suporte à autenticação e integração com o pipeline HTTP.

As regras de negócio principais estão isoladas na camada Application, enquanto o acesso a dados está concentrado na camada Infrastructure, respeitando a separação de responsabilidades.

Fluxo Completo da Aplicação
1. Autenticação

O usuário realiza login pelo frontend em React.
A API valida as credenciais e retorna um token JWT.
Esse token é armazenado no frontend e enviado nas próximas requisições autenticadas.

2. Cadastro e consulta de chamados

Após autenticado, o usuário pode:

cadastrar chamados
listar chamados
filtrar por cliente, prioridade e status
atualizar o status diretamente pela interface

Os dados principais do sistema são persistidos no PostgreSQL.

3. Atualização de status
Quando o status de um chamado é alterado para Finalizado, a API:
atualiza o registro no PostgreSQL
publica um evento na fila support-ticket-completed no RabbitMQ

4. Processamento assíncrono
O SupportDesk.Worker fica escutando essa fila.
Quando recebe a mensagem, ele processa o evento e registra uma notificação no MongoDB.

5. Resultado final
Com isso, o fluxo fica dividido em:
processamento síncrono para operações principais da API
processamento assíncrono para eventos de finalização de chamados