# 🚀 Fase 4.1: Injeção de Dependência

## Índice

- [1. O que foi feito?](#1-o-que-foi-feito)
- [2. Como foi feito?](#2-como-foi-feito)
    - [2.1 Pacotes NuGet Instalados](#21-pacotes-nuget-instalados)
    - [2.2 Registro do Banco de Dados](#22-registro-do-banco-de-dados)
    - [2.3 Registro dos Repositórios](#23-registro-dos-repositórios)
- [3. Por que foi feito assim?](#3-por-que-foi-feito-assim)
- [4. Glossário](#4-glossário)
- [5. Arquivos Gerados/Modificados](#5-arquivos-geradosmodificados)

---

## 1. O que foi feito?
Configuramos o **Container de Injeção de Dependência (IoC)** na classe `Program.cs`.
Isso ensina o ASP.NET Core a criar instâncias de nossas classes (`AppDbContext`, `TaskEfRepository`) automaticamente sempre que alguém precisar delas.

## 2. Como foi feito?
Configuramos o Container de Injeção de Dependência para orquestrar a criação de objetos em toda a aplicação.

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

### 2.2 Registro do Banco de Dados
```csharp
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("TaskMasterDb"));
```

#### Onde fica a Connection String?
No caso do **EF Core InMemory**, não precisamos de uma.
Mas para o **SQL Server (Dapper)**, ela fica no arquivo `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TaskMasterDb;..."
}
```
Para ler esse valor, usamos a interface `IConfiguration` que injetamos no `TaskSqlRepository`.

### 2.3 Registro dos Repositórios
Usamos dois tipos de registro:

1.  **Registro Padrão (Default):**
    Quem pedir `ITaskRepository` vai receber `TaskEfRepository`.
    ```csharp
    builder.Services.AddScoped<ITaskRepository, TaskEfRepository>();
    ```

2.  **Registro com Chave (Keyed Services):**
    Quem pedir explicitamente "sql" vai receber `TaskSqlRepository`.
    ```csharp
    builder.Services.AddKeyedScoped<ITaskRepository, TaskEfRepository>("ef");
    builder.Services.AddKeyedScoped<ITaskRepository, TaskSqlRepository>("sql");
    ```

3.  **Troca Total (Performance):**
    Para usar Dapper em tudo, basta trocar a linha do registro padrão (comentei no código para referência).
    ```csharp
    // Opção C: Performance Total (Dapper)
    // builder.Services.AddScoped<ITaskRepository, TaskSqlRepository>();
    ```

## 3. Por que foi feito assim?

### Ciclo de Vida: Scoped
Usamos `.AddScoped` porque queremos criar uma conexão com o banco **por requisição HTTP**.
*   **Singleton:** Criaria uma única conexão para sempre (perigoso para DBContext).
*   **Transient:** Criaria uma conexão nova a cada injeção (desperdício de recursos).
*   **Scoped:** O equilíbrio perfeito. Cria quando o request chega, descarta quando o request termina.

### Quem conhece quem? (Responsabilidades)
Uma dúvida comum é: *"Onde eu acesso o banco?"*

1.  **Core:** NÃO SABE nada sobre bancos. Só define interfaces (`ITaskRepository`).
2.  **Infrastructure:** SABE COMO acessar (EF Core ou SQL), mas não decide quando usar.
3.  **API (Program.cs):** DECIDE qual implementação usar. É aqui que ligamos o "plugue" da Infra na "tomada" do Core.

Se quisermos mudar de SQL Server para MongoDB, só mexemos na **Infrastructure** e trocamos uma linha no **Program.cs**. O resto do sistema nem percebe.

### Injeção de Dependência vs. `new Class()`
Em vez de fazer `var repo = new TaskRepository(new AppDbContext(...))` manualmente em cada Controller, deixamos o Framework resolver isso. Isso facilita testes unitários, pois podemos injetar Mocks no lugar das classes reais.

## 4. Glossário

| Termo (PT) | Termo (EN) | Definição |
| :--- | :--- | :--- |
| **Inversão de Controle** | **IoC (Inversion of Control)** | Princípio onde o fluxo do programa é controlado por um framework externo, não pelo seu código. |
| **Contêiner DI** | **DI Container** | O componente do ASP.NET Core que gerencia a criação e destruição dos objetos (Serviços). |
| **Tempo de Vida** | **Lifetime** | A duração de um serviço (Singleton, Scoped ou Transient) dentro do contêiner. |
| **Serviços Chaveados** | **Keyed Services** | Recurso do .NET 8+ que permite ter múltiplas implementações para a mesma interface, diferenciadas por uma chave (string/enum). |
| **Connection String** | **Connection String** | Cadeia de caracteres que contém as informações necessárias (servidor, banco, credenciais) para conectar a um banco de dados real. |

## 5. Arquivos Gerados/Modificados

| Tipo | Arquivo | Descrição |
| :--- | :--- | :--- |
| **Código Fonte** | `src/TaskMaster.Api/Program.cs` | Configuração do pipeline e serviços. |
| **Configuração** | `src/TaskMaster.Api/appsettings.json` | Definição da Connection String (SQL). |
| **Documentação** | `docs/TODO.md` | Lista mestre atualizada. |
| **Documentação** | `docs/README_PHASE_4_1_PT.md` | Este documento explicativo da fase. |
