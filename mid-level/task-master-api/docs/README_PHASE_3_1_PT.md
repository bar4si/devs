# 🚀 Fase 3.1: Configuração do Entity Framework Core

## Índice

- [1. O que foi feito?](#1-o-que-foi-feito)
- [2. Como foi feito?](#2-como-foi-feito)
    - [2.1 Pacotes NuGet Instalados](#21-pacotes-nuget-instalados)
    - [2.2 AppDbContext e Reflection](#22-appdbcontext-e-reflection)
    - [2.3 Mapeamento Separado (Classification)](#23-mapeamento-separado-classification)
- [3. Por que foi feito assim?](#3-por-que-foi-feito-assim)
- [4. Glossário](#4-glossário)
- [5. Arquivos Gerados/Modificados](#5-arquivos-geradosmodificados)

---

## 1. O que foi feito?
Preparamos a camada de infraestrutura para conversar com o banco de dados. Instalamos o **Entity Framework Core (EF Core)** e configuramos o "Contexto de Dados" (`AppDbContext`), que funciona como uma ponte entre nossos objetos C# (`TaskItem`) e as tabelas do banco.

Também definimos as regras de banco dados (tamanho de colunas, obrigatoriedade) através de classes de Mapeamento, sem poluir nossas Entidades de Domínio com atributos de banco.

## 2. Como foi feito?
Configuramos a camada de infraestrutura para fornecer persistência de dados de forma desacoplada.

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

### 2.2 AppDbContext e Reflection
Criamos a classe `AppDbContext` e usamos um "truque" muito útil no método `OnModelCreating`:
```csharp
modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
```
Isso diz ao EF Core: *"Varra este projeto inteiro procurando por configurações de mapa e aplique todas automaticamente"*.

### 2.3 Mapeamento Separado (`Classification`)
Em vez de encher a classe `AppDbContext` de código, ou usar *Data Annotations* na Entidade (ex: `[Table("Tasks")]`), criamos arquivos separados na pasta `Mappings`:

```csharp
public class TaskItemMapping : IEntityTypeConfiguration<TaskItem>
{
    public void Configure(EntityTypeBuilder<TaskItem> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Title).IsRequired().HasMaxLength(100);
        // ...
    }
}
```

## 3. Por que foi feito assim?

### Separation of Concerns (Separação de Preocupações)
*   **Entidade (Core):** Só se preocupa com regra de negócio. Não sabe que existe banco de dados.
*   **Mapping (Infra):** Só se preocupa com como salvar no banco (VARCHAR(100), Primary Key, etc).

Se usássemos `[MaxLength(100)]` direto na classe `TaskItem` (no Core), estaríamos "sujando" o domínio com uma dependência de infraestrutura/visualização.

### Fluent API vs Data Annotations
Optamos pela **Fluent API** (classes `IEntityTypeConfiguration`) porque ela é muito mais poderosa e flexível que *Data Annotations* (atributos em cima das propriedades). Ela permite configurar chaves compostas, índices e relacionamentos complexos de forma organizada.

**Exemplo Prático (Configuração Avançada):**
```csharp
// Configurando uma chave composta (Composite Key)
builder.HasKey(t => new { t.Id, t.TenantId });

// Configurando um índice para busca rápida
builder.HasIndex(t => t.Title);

// Configurando relacionamento 1:N
builder.HasOne(t => t.User)
       .WithMany(u => u.Tasks)
       .HasForeignKey(t => t.UserId);
```

## 4. Glossário

| Termo (PT) | Termo (EN) | Definição |
| :--- | :--- | :--- |
| **Mapeamento Objeto-Relacional** | **ORM (Object-Relational Mapper)** | Ferramenta que converte dados entre sistemas de tipos incompatíveis (C# Objects <-> SQL Tables). O EF Core é um ORM. |
| **Contexto de Banco de Dados** | **DbContext** | A classe principal do EF Core. Representa uma sessão com o banco de dados e é usada para consultar e salvar entidades. |
| **API Fluente** | **Fluent API** | Forma de configurar o modelo sobrescrevendo o método `OnModelCreating`, permitindo encadear chamadas de métodos. |
| **Reflection** | **Reflection** | Capacidade do código de examinar sua própria estrutura em tempo de execução (usado aqui para encontrar os Mappings automaticamente). |
| **Banco em Memória** | **Database in-memory** | Banco de dados que armazena informações apenas na RAM, usado para desenvolvimento rápido e testes sem precisar de um servidor SQL. |

## 5. Arquivos Gerados/Modificados

| Tipo | Arquivo | Descrição |
| :--- | :--- | :--- |
| **Código Fonte** | `src/TaskMaster.Infrastructure/Data/AppDbContext.cs` | O contexto do banco de dados. |
| **Código Fonte** | `src/TaskMaster.Infrastructure/Data/Mappings/TaskItemMapping.cs` | Configuração da tabela `TaskItem`. |
| **Configuração** | `src/TaskMaster.Infrastructure/TaskMaster.Infrastructure.csproj` | Adição dos pacotes NuGet do EF Core. |
| **Documentação** | `docs/TODO.md` | Lista mestre atualizada. |
| **Documentação** | `docs/README_PHASE_3_1_PT.md` | Este documento explicativo da fase. |
