# 🚀 Fase 3.2: Implementação de Repositórios

## Índice

- [1. O que foi feito?](#1-o-que-foi-feito)
- [2. Como foi feito?](#2-como-foi-feito)
    - [2.1 Pacotes NuGet Instalados](#21-pacotes-nuget-instalados)
    - [2.2 A Classe Concreta](#22-a-classe-concreta)
    - [2.3 Uso de AsNoTracking](#23-uso-de-asnotracking)
    - [2.4 Bônus: Repositório SQL com Dapper](#24-bônus-repositório-sql-com-dapper)
- [3. Por que foi feito assim?](#3-por-que-foi-feito-assim)
- [4. Glossário](#4-glossário)
- [5. Arquivos Gerados/Modificados](#5-arquivos-geradosmodificados)

---

## 1. O que foi feito?
Implementamos a "cola" entre o contrato definido no Core (`ITaskRepository`) e o banco de dados configurado na Infraestrutura (`AppDbContext`).

Criamos a classe `TaskRepository`, que é responsável por executar os comandos reais no banco de dados (INSERT, SELECT, UPDATE, DELETE) usando o Entity Framework Core.

## 2. Como foi feito?
Implementamos a "cola" entre o contrato definido no Core e o banco de dados real na Infraestrutura.

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

### 2.2 A Classe Concreta
Criamos a classe `TaskRepository` na pasta `src/TaskMaster.Infrastructure/Repositories`.

Ela implementa a interface `ITaskRepository` exigida pelo `Core`.

```csharp
public class TaskRepository : ITaskRepository
{
    private readonly AppDbContext _context;

    public TaskRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(TaskItem task)
    {
        await _context.Tasks.AddAsync(task);
        await _context.SaveChangesAsync();
    }
    // ... outros métodos
}
```

### 2.3 Uso de AsNoTracking
No método `GetAllAsync`, utilizamos `.AsNoTracking()`:
```csharp
return await _context.Tasks.AsNoTracking().ToListAsync();
```
Isso melhora a **performance** em operações de leitura, pois diz ao EF Core para não ficar vigiando alterações nesses objetos, já que só vamos exibi-los na tela.

### 2.4 Bônus: Repositório SQL com Dapper
Como prova de conceito da Injeção de Dependência, criamos também o `TaskSqlRepository`.
Ele usa **Dapper** e **SQL Puro** para fazer as mesmas operações.

```csharp
public class TaskSqlRepository : ITaskRepository
{
    // ...
    public async Task<IEnumerable<TaskItem>> GetAllAsync()
    {
        using var connection = CreateConnection();
        // SQL na veia: Performance máxima e zero abstração
        return await connection.QueryAsync<TaskItem>("SELECT * FROM Tasks");
    }
}
```

Isso demonstra que nossa arquitetura permite trocar o "motor" de persistência (EF Core <-> Dapper) sem tocar em nenhuma outra parte do sistema.

#### Como Alternar ou Usar Ambos?
Na **Fase 4 (API)**, faremos a configuração em `Program.cs`. 

*   **Opção A (Padrão):** Usar a implementação do EF Core.
    `builder.Services.AddScoped<ITaskRepository, TaskEfRepository>();`

*   **Opção B (Performance):** Trocar tudo para usar Dapper.
    `builder.Services.AddScoped<ITaskRepository, TaskSqlRepository>();`

*   **Opção C (Híbrido):** Registrar ambos usando *Keyed Services* (Recurso novo do .NET 8+).
    ```csharp
    builder.Services.AddKeyedScoped<ITaskRepository, TaskEfRepository>("ef");
    builder.Services.AddKeyedScoped<ITaskRepository, TaskSqlRepository>("sql");
    ```

## 3. Por que foi feito assim?

### Injeção de Dependência no Construtor
O repositório recebe dependências via construtor.
*   `TaskEfRepository`: Recebe `AppDbContext` (EF Core).
*   `TaskSqlRepository`: Recebe `IConfiguration` (para pegar a Connection String).

### Implementação Explícita
Ao implementar `ITaskRepository`, garantimos que o Repositório tenha exatamente os métodos que a Lógica de Negócio precisa. Se o Core mudar e precisar de um método `GetByDate`, o compilador vai acusar erro no Repositório até implementarmos, garantindo consistência.

## 4. Glossário

| Termo (PT) | Termo (EN) | Definição |
| :--- | :--- | :--- |
| **Implementação Concreta** | **Concrete Implementation** | O código real que faz o trabalho, em contraste com a Interface (que apenas define o que deve ser feito). |
| **Tracking** | **Change Tracking** | Recurso do EF Core que monitora mudanças nos objetos para salvar automaticamente. Desativar (`AsNoTracking`) economiza memória. |
| **Assíncrono** | **Async/Await** | Os métodos usam `await _context.SaveChangesAsync()` para liberar a thread enquanto o banco grava os dados. |
| **Micro-ORM** | **Micro-ORM** | Framework mais leve que um ORM completo (como EF Core). Mapeia resultados SQL para objetos, mas não gerencia estados. Ex: Dapper. |
| **AsNoTracking** | **AsNoTracking** | Extensão do EF Core que melhora a performance em consultas de leitura ao ignorar o sistema de monitoramento de mudanças. |

## 5. Arquivos Gerados/Modificados

| Tipo | Arquivo | Descrição |
| :--- | :--- | :--- |
| **Código Fonte** | `src/TaskMaster.Infrastructure/Repositories/TaskRepository.cs` | Implementação do acesso a dados. |
| **Código Fonte** | `src/TaskMaster.Infrastructure/Repositories/TaskSqlRepository.cs` | Implementação alternativa com Dapper (SQL Puro). |
| **Documentação** | `docs/TODO.md` | Lista mestre atualizada. |
| **Documentação** | `docs/README_PHASE_3_2_PT.md` | Este documento explicativo da fase. |
