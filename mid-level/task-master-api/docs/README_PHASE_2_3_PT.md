# 🚀 Fase 2.3: Objetos de Transferência (DTOs)

## Índice

- [1. O que foi feito?](#1-o-que-foi-feito)
- [2. Como foi feito?](#2-como-foi-feito)
    - [2.1 Pacotes NuGet Instalados](#21-pacotes-nuget-instalados)
    - [2.2 Vantagens na Implementação](#22-vantagens-na-implementação)
- [3. Por que foi feito assim?](#3-por-que-foi-feito-assim)
- [4. Glossário](#4-glossário)
- [5. Arquivos Gerados/Modificados](#5-arquivos-geradosmodificados)

---

## 1. O que foi feito?
Criamos classes específicas para **transportar dados** entre quem chama a API (Cliente) e nossa aplicação, desacoplando a estrutura interna (Entidades) da estrutura externa (JSON). São os **Data Transfer Objects (DTOs)**.

Implementamos três tipos de contratos de dados:
1.  **Criação:** O que eu preciso receber mínimo para criar uma tarefa? (`CreateTaskDto`)
2.  **Atualização:** O que pode mudar em uma tarefa existente? (`UpdateTaskDto`)
3.  **Visualização:** O que o cliente vê quando consulta uma tarefa? (`TaskResponseDto`)

## 2. Como foi feito?
Criamos a pasta `src/TaskMaster.Core/DTOs` contendo as classes.

### Vantagens na Implementação:
1.  **Validação (`Data Annotations`):** Usamos atributos como `[Required]` direto no DTO. Se faltar o título, a aplicação nem começa a processar a lógica de negócio.
2.  **Factory Method no Response:** Criamos um método estático `FromEntity` para converter de Entidade para DTO, centralizando essa lógica de tradução.
Criamos classes de contrato específicas para entrada e saída de dados, protegendo nossas entidades internas.

### 2.1 Pacotes NuGet Instalados
Estes são os pacotes que dão "superpoderes" às nossas camadas:

| Projeto | Pacote NuGet | Versão | Finalidade |
| :--- | :--- | :--- | :--- |
| **TaskMaster.Api** | `Microsoft.AspNetCore.OpenApi` | 10.0.2 | Suporte ao OpenAPI/Swagger. |
| **TaskMaster.Api** | `Scalar.AspNetCore` | 2.12.27 | Documentação interativa e elegante da API. |
| **TaskMaster.Infrastructure** | `Dapper` | 2.1.66 | Micro-ORM para consultas SQL de alta performance. |
| **TaskMaster.Infrastructure** | `Microsoft.Data.SqlClient` | 6.1.4 | Driver de conexão para SQL Server. |
| **TaskMaster.Infrastructure** | `Microsoft.EntityFrameworkCore` | 10.0.2 | ORM principal para mapeamento objeto-relacional. |
| **TaskMaster.Infrastructure** | `Microsoft.EntityFrameworkCore.SqlServer` | 10.0.2 | Provedor do EF Core para SQL Server. |
| **TaskMaster.Infrastructure** | `Microsoft.EntityFrameworkCore.InMemory` | 10.0.2 | Provedor de banco em memória para testes integrados. |
| **TaskMaster.UnitTests** | `xunit` | 2.9.3 | Framework de testes unitários. |
| **TaskMaster.UnitTests** | `Microsoft.NET.Test.Sdk` | 17.14.1 | SDK necessário para execução de testes .NET. |
| **TaskMaster.UnitTests** | `coverlet.collector` | 6.0.4 | Ferramenta para medição de cobertura de código. |

### 2.2 Vantagens na Implementação:
1.  **Validação (`Data Annotations`):** Usamos atributos como `[Required]` direto no DTO. Se faltar o título, a aplicação nem começa a processar a lógica de negócio.
2.  **Factory Method no Response:** Criamos um método estático `FromEntity` para converter de Entidade para DTO, centralizando essa lógica de tradução.

```csharp
// Exemplo de CreateTaskDto
public class CreateTaskDto
{
    [Required(ErrorMessage = "O título é obrigatório")]
    public string Title { get; set; } = string.Empty;
    // ...
}
```

## 3. Por que foi feito assim?

### Pattern DTO (Data Transfer Object)
Este é um diferencial importante. Muitos iniciantes expõem suas Entidades (`TaskItem`) direto na API.

**Por que NÃO expor Entidades diretamente?**
1.  **Segurança (Mass Assignment):** Um usuário mal-intencionado poderia tentar atualizar o `Id` ou a data de criação (`CreatedAt`), que deveriam ser imutáveis. Com DTOs, só expomos o que pode ser modificado.
2.  **Versionamento:** Se mudarmos o nome de uma coluna no banco ou propriedade na entidade, não quebramos quem consome a API, pois o DTO serve como um "amortecedor" ou contrato estável.
3.  **Performance (Over-fetching):** A entidade pode ter 50 campos, mas numa listagem só precisamos de 3. Um DTO específico economiza banda de rede.

### Diferença entre Input Model e Output Model
Separamos claramente:
*   **Input (Create/Update):** Foca em validação e entrada de dados.
*   **Output (Response):** Foca em formatação para leitura (ex: não mostrar dados sensíveis/internos).

## 4. Glossário

| Termo (PT) | Termo (EN) | Definição |
| :--- | :--- | :--- |
| **Objeto de Transferência** | **DTO** | Um objeto simples (POJO) usado apenas para carregar dados entre processos, sem lógica de negócio complexa. |
| **Atribuição em Massa** | **Mass Assignment** | Vulnerabilidade onde um atacante envia campos extras que a aplicação não esperava e acaba sobrescrevendo dados sensíveis. |
| **Acoplamento** | **Coupling** | Medida de quanto uma classe depende da outra. DTOs reduzem o acoplamento entre a API e o Banco de Dados. |
| **Anotações de Dados** | **Data Annotations** | Atributos (`[Required]`, `[MaxLength]`) usados para validação declarativa em .NET. |
| **Factory Method** | **Factory Method** | Método estático ou classe usado para centralizar a criação de objetos e conversão entre tipos (ex: Entidade para DTO). |
| **Input/Output Model** | **Input/Output Model** | Nomenclatura para DTOs que diferencia claramente dados que entram na API daqueles que saem como resposta. |

## 5. Arquivos Gerados/Modificados

| Tipo | Arquivo | Descrição |
| :--- | :--- | :--- |
| **Código Fonte** | `src/TaskMaster.Core/DTOs/CreateTaskDto.cs` | Contrato de entrada para criação. |
| **Código Fonte** | `src/TaskMaster.Core/DTOs/UpdateTaskDto.cs` | Contrato de entrada para atualização. |
| **Código Fonte** | `src/TaskMaster.Core/DTOs/TaskResponseDto.cs` | Contrato de saída para leitura. |
| **Documentação** | `docs/TODO.md` | Lista mestre atualizada. |
| **Documentação** | `docs/README_PHASE_2_3_PT.md` | Este documento explicativo da fase. |
