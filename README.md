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
- **Dapper** para acesso a dados
- **SQL Server LocalDB**
- **MediatR** para CQRS
- **FluentValidation** para validação
- **Swagger/OpenAPI** para documentação
- **xUnit** para testes unitários
- **Moq.AutoMock** para mocks em testes
- **FluentAssertions** para assertions mais legíveis

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
- SQL Server LocalDB (incluído no Visual Studio)
- Visual Studio 2022 ou VS Code

## Como Executar

### 1. Configurar o Banco de Dados

Execute o script SQL localizado em `ProposalService.Infrastructure/Scripts/CreateTables.sql` no SQL Server Management Studio ou execute via comando:

```bash
sqlcmd -S "(localdb)\mssqllocaldb" -i "ProposalService.Infrastructure/Scripts/CreateTables.sql"
```

### 2. Executar os Microserviços

#### ProposalService
```bash
cd ProposalService.API
dotnet run
```
- Swagger: https://localhost:7001/swagger

#### ContractingService
```bash
cd ContractingService.API
dotnet run --urls "https://localhost:7002;http://localhost:5002"
```
- Swagger: https://localhost:7002/swagger

### 3. Executar Testes

```bash
# Testes do ProposalService
cd ProposalService.Tests
dotnet test

# Testes do ContractingService
cd ContractingService.Tests
dotnet test
```

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

### Connection Strings
Ambos os serviços usam a mesma connection string no `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=InsurancePlatform;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

### Comunicação entre Microserviços

**NOTA IMPORTANTE:** O sistema implementa **DUAS abordagens de comunicação** para fins de **demonstração e comparação**:

#### 1. **HTTP REST** (Abordagem Principal)
O ContractingService se comunica com o ProposalService via HTTP:
```json
{
  "ProposalService": {
    "BaseUrl": "https://localhost:7001"
  }
}
```

#### 2. **Mensageria** (Exemplo Demonstrativo)
Comunicação assíncrona via RabbitMQ usando MassTransit:
- **ProposalStatusRequest** → Solicita verificação de status
- **ProposalStatusResponse** → Retorna dados da proposta
- **Configuração:** RabbitMQ rodando em `localhost:5672`

**⚠️ AVISO:** A implementação de mensageria é mantida **em paralelo** ao HTTP REST para **fins de teste e demonstração** de diferentes padrões arquiteturais. Em um ambiente de produção, recomenda-se escolher uma abordagem principal.

## Exemplos de Teste

O sistema inclui testes unitários abrangentes usando Moq.AutoMock:

```csharp
[Fact]
public async Task Handle_WithValidApprovedProposal_ShouldCreateContract()
{
    // Arrange
    var proposalId = Guid.NewGuid();
    var command = new CreateContractCommand(proposalId);
    
    // Mock setup using AutoMocker
    _mocker.GetMock<IProposalServiceClient>()
        .Setup(x => x.GetProposalAsync(proposalId))
        .ReturnsAsync(mockProposal);

    // Act
    var result = await _handler.Handle(command, CancellationToken.None);

    // Assert
    result.Should().NotBeNull();
    result.ProposalId.Should().Be(proposalId);
}
```

## Funcionalidades Implementadas

✅ Arquitetura Hexagonal  
✅ Microserviços com APIs REST  
✅ Banco de dados SQL Server com Dapper  
✅ CQRS com MediatR  
✅ Validação com FluentValidation  
✅ Testes unitários com Moq.AutoMock  
✅ Comunicação HTTP entre microserviços  
✅ **Mensageria com MassTransit/RabbitMQ** (exemplo demonstrativo)  
✅ Swagger/OpenAPI documentation  
✅ Clean Code e SOLID principles  
✅ Records imutáveis para DTOs  
✅ Padrões C# 12 modernos  

## Próximos Passos (Melhorias Futuras)

- [ ] Docker containers
- [ ] Mensageria (RabbitMQ/Azure Service Bus)
- [ ] Testes de integração
- [ ] Logging estruturado (Serilog)
- [ ] Health checks
- [ ] API Gateway
- [ ] Autenticação/Autorização
- [ ] Circuit Breaker pattern
- [ ] Retry policies
