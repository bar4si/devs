# 🏗️ Fase 1: Fundação e Estrutura do Projeto

## Índice

- [1. O que foi feito?](#1-o-que-foi-feito)
- [2. Como foi feito?](#2-como-foi-feito)
    - [2.1 Pacotes NuGet Instalados](#21-pacotes-nuget-instalados)
    - [2.2 Criação dos Projetos](#22-criação-dos-projetos)
    - [2.3 Vinculação na Solução](#23-vinculação-na-solução)
    - [2.4 Configuração de Dependências](#24-configuração-de-dependências)
- [3. Por que foi feito assim?](#3-por-que-foi-feito-assim)
- [4. Glossário](#4-glossário)
- [5. Arquivos Gerados/Modificados](#5-arquivos-geradosmodificados)

---

## 1. O que foi feito?
Nesta primeira fase, estabelecemos a "espinha dorsal" da aplicação. Não escrevemos regras de negócio ainda, mas preparamos o terreno para que o código cresça de forma organizada.

Criamos uma Solução .NET (`.sln`) contendo 4 projetos distintos:
1.  **TaskMaster.Core** (Class Library)
2.  **TaskMaster.Infrastructure** (Class Library)
3.  **TaskMaster.Api** (ASP.NET Core Web API)
4.  **TaskMaster.UnitTests** (xUnit)

## 2. Como foi feito?
Utilizamos a CLI do .NET para garantir reprodutibilidade e instalamos os pacotes necessários para suportar a arquitetura.

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

### 2.2 Criação dos Projetos
```bash
# Criar a solução vazia
dotnet new sln -n TaskMaster

# Criar as camadas
dotnet new classlib -n TaskMaster.Core -o src/TaskMaster.Core
dotnet new classlib -n TaskMaster.Infrastructure -o src/TaskMaster.Infrastructure
dotnet new webapi -n TaskMaster.Api -o src/TaskMaster.Api
dotnet new xunit -n TaskMaster.UnitTests -o tests/TaskMaster.UnitTests
```

### 2.3 Vinculação na Solução
```bash
# Adicionar projetos ao arquivo .sln
dotnet sln add src/TaskMaster.Core/TaskMaster.Core.csproj
dotnet sln add src/TaskMaster.Infrastructure/TaskMaster.Infrastructure.csproj
dotnet sln add src/TaskMaster.Api/TaskMaster.Api.csproj
dotnet sln add tests/TaskMaster.UnitTests/TaskMaster.UnitTests.csproj
```

### 2.4 Configuração de Dependências (O Pulo do Gato 🐱)
```bash
# Infraestrutura conhece o Core
dotnet add src/TaskMaster.Infrastructure/TaskMaster.Infrastructure.csproj reference src/TaskMaster.Core/TaskMaster.Core.csproj

# API conhece o Core e a Infraestrutura (para injeção de dependência)
dotnet add src/TaskMaster.Api/TaskMaster.Api.csproj reference src/TaskMaster.Core/TaskMaster.Core.csproj
dotnet add src/TaskMaster.Api/TaskMaster.Api.csproj reference src/TaskMaster.Infrastructure/TaskMaster.Infrastructure.csproj

# Testes conhecem o Core (para testar a lógica)
dotnet add tests/TaskMaster.UnitTests/TaskMaster.UnitTests.csproj reference src/TaskMaster.Core/TaskMaster.Core.csproj
```

## 3. Por que foi feito assim?

### Princípio da Arquitetura Limpa (Clean Architecture)
A decisão mais crítica aqui foi a **direção das dependências**.

*   **❌ O jeito "Júnior":** A camada de Banco de Dados é o centro, e tudo depende dela. Se mudar o banco, quebra tudo.
*   **✅ O jeito "Pleno/Sênior":** O **Domínio (Core)** é o centro. Ninguém manda no Core.

### Responsabilidade de Cada Projeto
1.  **TaskMaster.Core (Class Library):** O coração. Contém Entidades, Interfaces, DTOs e Regras de Negócio. Totalmente puro (sem dependências de terceiros).
2.  **TaskMaster.Infrastructure (Class Library):** O músculo. Implementa as interfaces do Core. Cuida de Banco de Dados (EF Core), E-mail, Logs, etc.
3.  **TaskMaster.Api (ASP.NET Core Web API):** A face. Recebe requisições HTTP, valida com DTOs e chama o Core/Infra. É o ponto de entrada da aplicação.
4.  **TaskMaster.UnitTests (xUnit):** O guardião. Garante que nada quebrou. Testa isoladamente a lógica do Core usando Mocks.

### Por que separamos em projetos físicos (`.csproj`)?
1.  **Impedir Violações Arquiteturais:** O compilador vai dar erro se você tentar usar uma classe da `Api` dentro do `Core`, porque o `Core` literalmente não tem referência para a `Api`. Isso força a disciplina.
2.  **Testabilidade:** O `Core` não depende de bibliotecas pesadas (como Entity Framework ou ASP.NET Core). Isso torna os testes de unidade extremamente rápidos e fáceis de escrever.
3.  **Manutenibilidade:** Se amanhã quisermos trocar a API REST por um Worker Service ou um App Console, o `Core` e a `Infrastructure` continuam intactos. A regra de negócio não muda só porque a interface mudou.

## 4. Glossário

| Termo (PT) | Termo (EN) | Definição |
| :--- | :--- | :--- |
| **Solução** | **Solution (.sln)** | Um arquivo container que agrupa múltiplos projetos. |
| **Projeto** | **Project (.csproj)** | Arquivo que define propriedades, dependências e itens de um projeto individual. |
| **Biblioteca de Classes** | **Class Library** | Projeto que compila para DLL, usado para compartilhamento de código. |
| **Interface de Linha de Comando** | **CLI** | Interface para executar comandos de texto (ex: `dotnet new`). |
| **Arquitetura Limpa** | **Clean Architecture** | Padrão que isola o domínio de detalhes externos (UI, BD). |
| **Injeção de Dependência** | **Dependency Injection (DI)** | Técnica de inversão de controle onde dependências são fornecidas externamente. |
| **Testes de Unidade** | **Unit Tests (xUnit)** | Framework e prática de testar pequenas partes de código em isolamento para garantir qualidade. |

## 5. Arquivos Gerados/Modificados

| Tipo | Arquivo | Descrição |
| :--- | :--- | :--- |
| **Solution** | `TaskMaster.sln` | Arquivo de solução que agrupa todos os projetos. |
| **Projeto** | `src/TaskMaster.Core/TaskMaster.Core.csproj` | Projeto da camada de Domínio (Class Library). |
| **Projeto** | `src/TaskMaster.Infrastructure/TaskMaster.Infrastructure.csproj` | Projeto da camada de Infraestrutura (Class Library). |
| **Projeto** | `src/TaskMaster.Api/TaskMaster.Api.csproj` | Projeto da camada de Apresentação (Web API). |
| **Projeto** | `tests/TaskMaster.UnitTests/TaskMaster.UnitTests.csproj` | Projeto de Testes Unitários (xUnit). |
| **Documentação** | `docs/task-master-api-implementation-plan.md` | Plano original de implementação. |
| **Documentação** | `docs/README_PHASE_1_PT.md` | Este documento explicativo da fase. |
