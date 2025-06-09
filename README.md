# 🍽️ KitchenService - Microserviço de Cozinha

Este microserviço é responsável por **processar pedidos recebidos**, permitindo que a equipe de cozinha **aceite ou rejeite** os pedidos e envie eventos de status de volta para os outros serviços.

---

## Funcionalidades

- Recebe pedidos via **mensageria (RabbitMQ)** com eventos `OrderCreated`
- Salva os pedidos em **MongoDB**
- Recebe eventos com os pedidos
- Publica eventos de volta:
  - `OrderAcceptedEvent`
  - `OrderRejectedEvent`

---

## Arquitetura

- **Domain-Driven Design (DDD)**: entidades e regras de negócio puras
- **Clean Architecture**: separação clara entre camadas
- **Event-driven** com MassTransit + RabbitMQ
- **Persistência NoSQL** com MongoDB
- **Observabilidade** via logs estruturados

---

## Estrutura do Projeto

