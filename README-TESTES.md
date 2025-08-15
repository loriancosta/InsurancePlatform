# Insurance Platform - Guia de Testes

## 🚀 Como Executar as APIs

### Pré-requisitos
- .NET 8.0 SDK instalado
- Nenhum banco de dados necessário (dados em memória)

### Executando os Serviços

#### 1. ProposalService (Porta 7001)
```bash
cd src/ProposalService/ProposalService.API
dotnet run
```

#### 2. ContractingService (Porta 7002)
```bash
cd src/ContractingService/ContractingService.API
dotnet run
```

## 📊 Dados Pré-carregados

### Propostas (ProposalService)
O sistema inicia com 5 propostas de exemplo:

| ID | Cliente | Tipo de Seguro | Valor | Status |
|---|---|---|---|---|
| `11111111-1111-1111-1111-111111111111` | João Silva | Seguro Auto | R$ 1.500,00 | Em Análise |
| `22222222-2222-2222-2222-222222222222` | Maria Santos | Seguro Residencial | R$ 2.500,00 | **Aprovada** |
| `33333333-3333-3333-3333-333333333333` | Pedro Oliveira | Seguro Vida | R$ 800,00 | Rejeitada |
| `44444444-4444-4444-4444-444444444444` | Ana Costa | Seguro Auto | R$ 1.800,00 | **Aprovada** |
| `55555555-5555-5555-5555-555555555555` | Carlos Ferreira | Seguro Empresarial | R$ 5.000,00 | Em Análise |

### Contratos (ContractingService)
O sistema inicia com 2 contratos já criados:

| ID | Proposta ID | Cliente | Tipo de Seguro | Valor |
|---|---|---|---|---|
| `aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa` | `22222222-2222-2222-2222-222222222222` | Maria Santos | Seguro Residencial | R$ 2.500,00 |
| `bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb` | `44444444-4444-4444-4444-444444444444` | Ana Costa | Seguro Auto | R$ 1.800,00 |

## 🧪 Testes das APIs

### ProposalService (https://localhost:7001)

#### 1. Listar todas as propostas
```bash
GET https://localhost:7001/api/proposals
```

#### 2. Buscar proposta específica
```bash
GET https://localhost:7001/api/proposals/11111111-1111-1111-1111-111111111111
```

#### 3. Criar nova proposta
```bash
POST https://localhost:7001/api/proposals
Content-Type: application/json

{
  "customerName": "Fernanda Lima",
  "insuranceType": "Seguro Saúde",
  "value": 1200.00
}
```

#### 4. Alterar status da proposta
```bash
PUT https://localhost:7001/api/proposals/11111111-1111-1111-1111-111111111111/status
Content-Type: application/json

{
  "status": 2
}
```

**Status disponíveis:**
- `1` = Em Análise (UnderAnalysis)
- `2` = Aprovada (Approved)
- `3` = Rejeitada (Rejected)

**Nota:** Todos os retornos incluem o campo `statusDescription` com a tradução em português do status para facilitar o entendimento.

### ContractingService (https://localhost:7002)

#### 1. Listar todos os contratos
```bash
GET https://localhost:7002/api/contracts
```

#### 2. Criar contrato (apenas para propostas aprovadas)
```bash
POST https://localhost:7002/api/contracts
Content-Type: application/json

{
  "proposalId": "11111111-1111-1111-1111-111111111111"
}
```

**Nota:** Este exemplo falhará porque a proposta está "Em Análise". Use uma proposta com status "Aprovada" ou aprove uma proposta primeiro.

## 🔄 Fluxo de Teste Completo

### Cenário 1: Aprovar proposta e criar contrato
1. **Listar propostas** para ver status atual
2. **Aprovar proposta** do João Silva:
   ```bash
   PUT https://localhost:7001/api/proposals/11111111-1111-1111-1111-111111111111/status
   {
     "status": 2
   }
   ```
3. **Criar contrato** para a proposta aprovada:
   ```bash
   POST https://localhost:7002/api/contracts
   {
     "proposalId": "11111111-1111-1111-1111-111111111111"
   }
   ```
4. **Verificar contratos** criados

### Cenário 2: Tentar contratar proposta rejeitada (deve falhar)
1. **Tentar criar contrato** para proposta rejeitada:
   ```bash
   POST https://localhost:7002/api/contracts
   {
     "proposalId": "33333333-3333-3333-3333-333333333333"
   }
   ```
2. **Resultado esperado:** Erro 400 - Proposta não aprovada

## 🔧 Comunicação via Mensageria

O sistema usa **MassTransit com InMemory transport** para comunicação entre serviços:

- **ContractingService** envia mensagem `ProposalStatusRequest`
- **ProposalService** responde com `ProposalStatusResponse`
- **Fila:** `mq-proposal-status-request`

### ✅ Como Validar se a Fila Funciona

#### Método 1: Teste Rápido (Deve dar erro específico)
```bash
POST https://localhost:7002/api/contracts
Content-Type: application/json

{
  "proposalId": "22222222-2222-2222-2222-222222222222"
}
```

**Resultado esperado:** 
- ✅ **Se funciona:** Erro 500 "Contract for proposal already exists" 
- ❌ **Se não funciona:** Timeout ou erro de conexão

#### Método 2: Fluxo Completo de Validação
1. **Aprove uma proposta:**
   ```bash
   PUT https://localhost:7001/api/proposals/11111111-1111-1111-1111-111111111111/status
   Content-Type: application/json
   
   {
     "status": 2
   }
   ```

2. **Crie o contrato:**
   ```bash
   POST https://localhost:7002/api/contracts
   Content-Type: application/json
   
   {
     "proposalId": "11111111-1111-1111-1111-111111111111"
   }
   ```

**Resultado esperado:** Contrato criado com sucesso (Status 201)

#### Validação pelos Logs
**No Terminal do ProposalService, procure por:**
```
Consuming ProposalStatusRequest for proposal: [ID]
```

**No Terminal do ContractingService, procure por:**
```
Sending ProposalStatusRequest via MassTransit
```

### 🎯 Confirmação de Funcionamento
- **Resposta rápida** (não timeout) = Fila OK ✅
- **Erro específico sobre status** = Comunicação funcionando ✅
- **Contrato criado após aprovação** = Fluxo completo OK ✅

## 📝 Observações

- **Dados em memória:** Todos os dados são perdidos quando as APIs são reiniciadas
- **Singleton repositories:** Os dados são compartilhados entre todas as requisições
- **Sem banco de dados:** Não é necessário instalar SQL Server ou qualquer outro banco
- **Mensageria InMemory:** Funciona apenas enquanto ambas as APIs estão rodando

## 🎯 Funcionalidades Implementadas

✅ **ProposalService:**
- Criar proposta de seguro
- Listar propostas
- Buscar proposta por ID
- Alterar status da proposta
- API REST completa

✅ **ContractingService:**
- Contratar proposta (apenas se aprovada)
- Listar contratos
- Comunicação via mensageria com ProposalService
- Validação de status da proposta

✅ **Arquitetura:**
- Hexagonal (Ports & Adapters)
- Microserviços independentes
- Mensageria com MassTransit
- Clean Code, DDD, SOLID
- Dados mocados em memória
