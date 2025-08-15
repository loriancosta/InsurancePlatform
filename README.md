# Insurance Platform

Sistema de gerenciamento de propostas de seguro e contratação, desenvolvido com arquitetura hexagonal e microserviços.

## Arquitetura

O sistema é composto por dois microserviços principais:

### 1. ProposalService
- **Porta**: 7001 (HTTPS) / 5001 (HTTP)
- **Responsabilidades**:
  - Criar proposta de seguro
  - Listar propostas
  - Alterar status da proposta (Em Análise, Aprovada, Rejeitada)
  - Expor API REST

### 2. ContractingService
- **Porta**: 7002 (HTTPS) / 5002 (HTTP)
- **Responsabilidades**:
  - Contratar uma proposta (somente se Aprovada)
  - Armazenar informações da contratação
  - Comunicar-se com o ProposalService via HTTP REST
  - Expor API REST

## Tecnologias Utilizadas

- **.NET 8**
- **C# 12** com padrões modernos (collection expressions, primary constructors, file-scoped namespaces)
- **MediatR** para CQRS
- **FluentValidation** para validação
- **Swagger/OpenAPI** para documentação
- **xUnit** para testes unitários
- **Moq.AutoMock** para mocks em testes
- **FluentAssertions** para assertions mais legíveis
- **Dapper** para acesso a dados (Não funcional, apenas para efeito de avaliação)
- **SQL Server LocalDB** (Não funcional, apenas para efeito de avaliação)

## Padrões Implementados

- **Arquitetura Hexagonal (Ports & Adapters)**
- **Clean Architecture**
- **Domain-Driven Design (DDD)**
- **CQRS** com MediatR
- **SOLID Principles**
- **Repository Pattern**
- **Dependency Injection**

## Pré-requisitos

- .NET 8 SDK
- SQL Server LocalDB (Não funcional, apenas para efeito de avaliação)

## Como Executar

### 1. Configurar o Banco de Dados

O banco de dados foi criado em memória para fins de exemplificar o funcionamento
Portanto, não há necessidade de criarbanco de dados

### 2. Executar os Microserviços

#### ProposalService
- Swagger: https://localhost:7002/swagger

#### ContractingService
- Swagger: https://localhost:7001/swagger


## Fluxo de Uso

### 1. Criar uma Proposta
```http
POST https://localhost:7001/api/proposals
Content-Type: application/json

{
  "customerName": "João Silva",
  "insuranceType": "Seguro Auto",
  "value": 1500.00
}
```

### 2. Listar Propostas
```http
GET https://localhost:7001/api/proposals
```

### 3. Aprovar uma Proposta
```http
PUT https://localhost:7001/api/proposals/{id}/status
Content-Type: application/json

{
  "status": 2
}
```
Status: 1 = Em Análise, 2 = Aprovada, 3 = Rejeitada

### 4. Contratar uma Proposta Aprovada
```http
POST https://localhost:7002/api/contracts
Content-Type: application/json

{
  "proposalId": "guid-da-proposta-aprovada"
}
```

### 5. Listar Contratos
```http
GET https://localhost:7002/api/contracts
```

## Estrutura do Projeto

```
InsurancePlatform/
├── .editorconfig
├── README.md
├── InsurancePlatform.sln
├── src/
│   ├── ProposalService/
│   │   ├── ProposalService.API/          # Controllers, Program.cs
│   │   ├── ProposalService.Application/  # Commands, Queries, Handlers, DTOs
│   │   ├── ProposalService.Domain/       # Entities, Enums, Interfaces
│   │   └── ProposalService.Infrastructure/ # Repositories, Database
│   └── ContractingService/
│       ├── ContractingService.API/       # Controllers, Program.cs
│       ├── ContractingService.Application/ # Commands, Queries, Handlers, DTOs
│       ├── ContractingService.Domain/    # Entities, Interfaces
│       └── ContractingService.Infrastructure/ # Repositories, HTTP Clients
└── tests/
    ├── ProposalService.Tests/        # Testes unitários
    └── ContractingService.Tests/     # Testes unitários
```

## Configurações

#### 2. **Mensageria** (Exemplo Demonstrativo)
Comunicação assíncrona via MSMQ usando MassTransit:


## Exemplos de Teste

O sistema inclui testes unitários abrangentes usando Moq.AutoMock: