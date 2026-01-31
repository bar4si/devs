# 🚀 Fase 4.2: Controladores e Endpoints

## Índice

- [1. O que foi feito?](#1-o-que-foi-feito)
- [2. Como foi feito?](#2-como-foi-feito)
    - [2.1 Pacotes NuGet Instalados](#21-pacotes-nuget-instalados)
    - [2.2 Mapeamento HTTP para CRUD](#22-mapeamento-http-para-crud)
    - [2.3 Uso de DTOs no Controlador](#23-uso-de-dtos-no-controlador)
- [3. Por que foi feito assim?](#3-por-que-foi-feito-assim)
- [4. Fluxo de Dados Completo](#4-fluxo-de-dados-completo)
- [5. Glossário](#5-glossário)
- [6. Arquivos Gerados/Modificados](#6-arquivos-geradosmodificados)
- [7. Documentação (Swagger/OpenAPI)](#7-documentacao-swaggeropenapi)

---

## 1. O que foi feito?
Criamos o **Ponto de Entrada** da nossa API: o `TasksController`.
Agora, sistemas externos podem **Listar**, **Criar**, **Atualizar** e **Excluir** tarefas fazendo requisições HTTP (GET, POST, PUT, DELETE).

## 2. Como foi feito?
Criamos os pontos de entrada da API, mapeando as intenções do usuário para as operações do nosso domínio.

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

### 2.2 Mapeamento HTTP para CRUD
Implementamos os seguintes endpoints:

| Verbo HTTP | Rota | Ação | Retorno Sucesso |
| :--- | :--- | :--- | :--- |
| **GET** | `/api/tasks` | Listar todas | 200 OK (Lista) |
| **GET** | `/api/tasks/{id}` | Obter uma | 200 OK (Objeto) |
| **POST** | `/api/tasks` | Criar nova | 201 Created |
| **PUT** | `/api/tasks/{id}` | Atualizar | 204 No Content |
| **DELETE** | `/api/tasks/{id}` | Excluir | 204 No Content |

### 2.3 Uso de DTOs no Controlador
O Controlador **nunca** expõe a Entidade de Domínio (`TaskItem`) diretamente.
*   **Entrada:** Recebe `CreateTaskDto` ou `UpdateTaskDto`.
*   **Processamento:** Converte para `TaskItem` (usando Factory/Construtor).
*   **Saída:** Converte o resultado para `TaskResponseDto`.

```csharp
// Exemplo: Método POST
public async Task<IActionResult> Create([FromBody] CreateTaskDto dto)
{
    // 1. Converte DTO -> Entidade
    var task = new TaskItem(dto.Title, dto.Description...);
    
    // 2. Chama o Repositório
    await _repository.AddAsync(task);
    
    // 3. Converte Entidade -> DTO de Resposta
    return CreatedAtAction(..., TaskResponseDto.FromEntity(task));
}
```

## 3. Por que foi feito assim?

### Separation of Concerns (Separação de Preocupações)
*   O **Controller** só se preocupa com HTTP (Status Codes, Rotas, JSON).
*   O **Repository** só se preocupa com Banco de Dados.
*   O **Domain** só se preocupa com Regras de Negócio.

### Tratamento de Erros HTTP
Usamos os Status Codes corretos para cada situação, não apenas "200 OK" para tudo.
*   Retornamos `404 Not Found` se o ID não existir.
*   Retornamos `201 Created` ao criar recurso.
*   Retornamos `400 Bad Request` se a validação falhar (`ModelState.IsValid`).

## 4. Fluxo de Dados Completo

Para entender melhor como tudo se conecta, veja o caminho que um dado percorre quando você faz um `POST /api/tasks`:

1.  **Cliente (Postman/Browser):** Realiza uma requisição `POST` para o endpoint `/api/tasks` enviando um JSON: `{"title": "Estudar", "description": "Estudar .net com AI", "dueDate": "2026-01-31", "priority": "High"}`.
2.  **API (Controller):** [TasksController.cs](../src/TaskMaster.Api/Controllers/TasksController.cs)
    *   O ASP.NET Core roteia a chamada para o método `Create` do controlador.
    *   Recebe o JSON e **converte** (via Model Binding) para [CreateTaskDto.cs](../src/TaskMaster.Core/DTOs/CreateTaskDto.cs).
    *   Verifica se é válido (ex: Título não pode ser vazio).
    *   Chama a fábrica: `new TaskItem(dto.Title...)`.
3.  **Core (Domain):** [TaskItem.cs](../src/TaskMaster.Core/Entities/TaskItem.cs)
    *   O construtor do `TaskItem` roda regras de negócio (validações extras, define Data de Criação).
    *   Retorna uma Entidade válida e íntegra.
4.  **Infrastructure (Repository):** [TaskEfRepository.cs](../src/TaskMaster.Infrastructure/Repositories/TaskEfRepository.cs)
    *   O Controller passa a entidade para `_repository.AddAsync(task)`.
    *   O Repositório avisa o [AppDbContext.cs](../src/TaskMaster.Infrastructure/Data/AppDbContext.cs): "Vigie este novo objeto".
5.  **Database (EF Core):** [TaskItemMapping.cs](../src/TaskMaster.Infrastructure/Data/Mappings/TaskItemMapping.cs)
    *   Ao chamar `SaveChangesAsync()`, o EF Core traduz o objeto para SQL: `INSERT INTO Tasks...`.
    *   O banco salva os dados e gera o ID (já gerado no Guid do C#).

### Como uma camada "conhece" a outra? (O segredo da DI)
Você deve ter reparado que o `TasksController` não usa o `TaskEfRepository` diretamente. Ele usa a interface `ITaskRepository`.

*   **Contrato (O "O Quê"):** A API pede qualquer classe que saiba fazer o que a `ITaskRepository` (no Core) manda.
*   **Injeção (O "Quem"):** O ASP.NET Core olha no [Program.cs](../src/TaskMaster.Api/Program.cs) para ver quem foi "contratado" para essa interface.
*   **Implementação (O "Como"):** Como registramos `builder.Services.AddScoped<ITaskRepository, TaskEfRepository>()`, o framework cria o `TaskEfRepository` e "entrega" (injeta) no construtor do controlador.

Isso é o que chamamos de **Desacoplamento**. Se amanhã trocarmos para Dapper, o controlador continua funcionando sem mudar uma linha, pois ele continua recebendo o mesmo "contrato".

#### Exemplo Prático: Mudando para Dapper
Para mudar o fluxo de dados do **Entity Framework** para o **Dapper**, basta alterar UMA linha no [Program.cs](../src/TaskMaster.Api/Program.cs):

```diff
- builder.Services.AddScoped<ITaskRepository, TaskEfRepository>();
+ builder.Services.AddScoped<ITaskRepository, TaskSqlRepository>();
```

Ao fazer isso:
1.  O Controlador continua pedindo `ITaskRepository`.
2.  O ASP.NET Core passa a instanciar o `TaskSqlRepository`.
3.  O fluxo de dados muda dos métodos do EF (`_context.Tasks.Add`) para o SQL Puro do Dapper (`INSERT INTO Tasks...`).
4.  **Tudo isso acontece sem que o Controlador ou o Core precisem ser recompilados ou alterados.**

#### Exemplo Prático: EF Core com SQL Server (Sem ser In-Memory)
Se você quiser continuar usando o **Entity Framework**, mas gravando em um banco SQL real em vez da memória:

No [Program.cs](../src/TaskMaster.Api/Program.cs):
```diff
- options.UseInMemoryDatabase("TaskMasterDb"));
+ options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
```

Diferente do Dapper, ao usar o EF com SQL Server, você terá acesso ao sistema de **Migrations** para criar as tabelas automaticamente via terminal (`dotnet ef migrations add...`).

## 5. Glossário

| Termo (PT) | Termo (EN) | Definição |
| :--- | :--- | :--- |
| **Endpoint** | **Endpoint** | Um endereço URL específico (ex: `/api/tasks`) onde a API aceita requisições. |
| **Verbo HTTP** | **HTTP Verb** | A ação desejada na requisição (GET, POST, PUT, DELETE). |
| **Status Code** | **Status Code** | Código numérico que indica o resultado (200=Sucesso, 404=Não Encontrado, 500=Erro Interno). |
| **IActionResult** | **IActionResult** | Interface do ASP.NET Core que representa o resultado de uma ação, permitindo retornar JSON, Views ou Status Codes. |
| **Scalar** | **Scalar** | Interface interativa moderna para documentação e teste de endpoints que substitui o Swagger UI. |
| **OpenAPI** | **OpenAPI** | Especificação padrão para descrever APIs RESTful, permitindo que humanos e máquinas entendam as capacidades do serviço. |
| **Model Binding** | **Model Binding** | Recurso que extrai dados de requisições HTTP (corpo, query strings) e os mapeia para parâmetros de métodos no Controller. |
| **RESTful** | **RESTful** | Estilo arquitetural que utiliza as restrições do protocolo HTTP (verbos, recursos, status codes) para criar APIs padronizadas. |

## 6. Arquivos Gerados/Modificados

| Tipo | Arquivo | Descrição |
| :--- | :--- | :--- |
| **Código Fonte** | `src/TaskMaster.Api/Controllers/TasksController.cs` | Implementação dos endpoints. |
| **Documentação** | `docs/TODO.md` | Lista mestre atualizada. |
| **Documentação** | `docs/README_PHASE_4_2_PT.md` | Este documento explicativo da fase. |

## 7. Documentação Interativa (Scalar)

Em vez do Swagger UI tradicional, este projeto utiliza o **Scalar**, uma interface moderna e "premium" para documentação de APIs que se integra perfeitamente ao .NET 10.

### Como Acessar
Ao rodar a aplicação em ambiente de desenvolvimento (`IsDevelopment`), a documentação estará disponível em:
- **URL Interativa:** `https://localhost:PORTA/scalar/v1`
- **JSON OpenAPI:** `https://localhost:PORTA/openapi/v1.json`

### Principais Recursos
1.  **Try it Out:** Teste os endpoints diretamente pelo navegador.
2.  **Code Snippets:** Gera automaticamente exemplos de código para chamar a API em várias linguagens (C#, JavaScript, Python, etc.).
3.  **Busca Global:** Encontre rotas e modelos rapidamente.

> [!IMPORTANT]
> O Scalar consome o documento gerado nativamente pelo `Microsoft.AspNetCore.OpenApi`. Isso significa que a especificação é sempre fiel ao código atualizado.
