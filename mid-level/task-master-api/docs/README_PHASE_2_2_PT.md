# 🚀 Fase 2.2: Contratos de Repositório (Dependency Inversion)

## Índice

- [1. O que foi feito?](#1-o-que-foi-feito)
- [2. Como foi feito?](#2-como-foi-feito)
    - [2.1 Pacotes NuGet Instalados](#21-pacotes-nuget-instalados)
    - [2.2 Assinatura dos Métodos (Async/Await)](#22-assinatura-dos-métodos-asyncawait)
- [3. Por que foi feito assim?](#3-por-que-foi-feito-assim)
- [4. Glossário](#4-glossário)
- [5. Arquivos Gerados/Modificados](#5-arquivos-geradosmodificados)

---

## 1. O que foi feito?
Definimos o **Contrato** de como a aplicação acessa e salva os dados das tarefas, criando a interface `ITaskRepository`.

Note que **não escrevemos uma linha de código SQL ou Entity Framework** ainda. Apenas dissemos: *"Quem quiser ser um repositório de tarefas, precisa saber fazer essas 5 coisas (Get, Add, Update, Delete)"*.

## 2. Como foi feito?
Criamos o arquivo `ITaskRepository.cs` pasta `src/TaskMaster.Core/Interfaces`.

### Assinatura dos Métodos (Async/Await)
Definimos o contrato no Core para isolar o domínio das tecnologias de banco de dados.

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

### 2.2 Assinatura dos Métodos (Async/Await)
Todos os métodos retornam `Task`. Isso prepara nosso sistema para ser escalável e não-bloqueante desde o design (I/O Bound).

```csharp
public interface ITaskRepository
{
    // Retorna Task, permitindo async/await
    Task<TaskItem?> GetByIdAsync(Guid id);
    Task AddAsync(TaskItem task);
    // ...
}
```

## 3. Por que foi feito assim?

### Inversão de Dependência (O "D" do SOLID)
Este é o conceito mais importante para um Dev Pleno dominar.

*   **Abordagem Comum:** A Camada de Negócio (`Core`) depende diretamente do Entity Framework (`Infrastructure`).
*   **Abordagem Correta (DIP):**
    1.  O `Core` define uma interface (`ITaskRepository`).
    2.  A `Infrastructure` implementa essa interface.
    3.  O `Core` **não conhece** a `Infrastructure`.

Isso permite que você troque o banco de dados (de SQL Server para Mongo, ou para um Arquivo de Texto) sem tocar em **nenhuma linha** da regra de negócio. O Core está protegido.

### Testabilidade
Como o Core depende de uma Interface, nos Testes Unitários podemos criar um **Mock** (Repositório Falso) muito facilmente. Não precisamos subir um banco de dados real para testar se uma tarefa pode ser concluída.

## 4. Glossário

| Termo (PT) | Termo (EN) | Definição |
| :--- | :--- | :--- |
| **Inversão de Dependência** | **Dependency Inversion** | Módulos de alto nível (Core) não devem depender de módulos de baixo nível (Infra). Ambos devem depender de abstrações (Interfaces). |
| **Repositório** | **Repository Pattern** | Padrão que abstrai a camada de dados, fazendo com que o acesso ao banco pareça uma coleção em memória. |
| **Assíncrono** | **Async/Await** | Modelo de programação que não bloqueia a thread principal enquanto espera operações lentas (como ir ao banco de dados). |
| **Mock** | **Mock Object** | Um objeto simulado que imita o comportamento de objetos reais de forma controlada, usado em testes. |
| **SOLID** | **SOLID** | Acrônimo para cinco princípios de design (Single Responsibility, Open/Closed, Liskov Substitution, Interface Segregation, Dependency Inversion) que tornam o software mais flexível e sustentável. |

## 5. Arquivos Gerados/Modificados

| Tipo | Arquivo | Descrição |
| :--- | :--- | :--- |
| **Código Fonte** | `src/TaskMaster.Core/Interfaces/ITaskRepository.cs` | Contrato do repositório (Interface). |
| **Documentação** | `docs/TODO.md` | Lista mestre atualizada. |
| **Documentação** | `docs/README_PHASE_2_2_PT.md` | Este documento explicativo da fase. |
