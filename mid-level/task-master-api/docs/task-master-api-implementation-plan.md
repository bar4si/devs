# 🏗️ Projeto 1: TaskMaster API - Portfólio Nível Pleno

> **Status do Projeto:** 🚧 Em Desenvolvimento (Fase 2.0)
> **Stack:** .NET 10, EF Core, REST API
> **Focus:** Clean Architecture, SOLID, Patterns

---

## 📋 Índice

- [1. Descrição do Objetivo](#1-%EF%B8%8F-descri%C3%A7%C3%A3o-do-objetivo)
- [2. Contexto do Projeto](#2-contexto-do-projeto)
- [3. Arquitetura Proposta](#3-%EF%B8%8F-arquitetura-proposta)
  - [3.1. Estrutura da Solução](#31-estrutura-da-solu%C3%A7%C3%A3o)
  - [3.2. Detalhamento das Camadas](#32-detalhamento-das-camadas)
- [4. Checklist de Desenvolvimento (WBS)](#4-%EF%B8%8F-checklist-de-desenvolvimento-wbs)
- [5. Estratégia de Verificação](#5-estrat%C3%A9gia-de-verifica%C3%A7%C3%A3o)

---

## 1. 🎯 Descrição do Objetivo

Desenvolver uma **Web API RESTful robusta** para gerenciamento de tarefas ("TaskMaster"). 

O foco principal ultrapassa a funcionalidade básica de CRUD; o objetivo é demonstrar competências de um **Desenvolvedor Pleno**, evidenciando:
- **Design Patterns** (Repository, DTO, Unit of Work)
- **Qualidade de Código** (Clean Code, fail-fast validations)
- **Arquitetura Escalável** (Separação estrita de responsabilidades)

### Conceitos Chave

| Conceito | Aplicação no Projeto |
| :--- | :--- |
| **Clean Architecture** | Separação física e lógica (API, Core, Infra). |
| **Dependency Injection** | Uso extensivo do container IoC do .NET. |
| **Repository Pattern** | Abstração da lógica de acesso a dados. |
| **Rich Domain Model** | Entidades com comportamento e validação de negócio interna. |
| **Fail-Fast** | Uso de FluentValidation para rejeitar dados inválidos precocemente. |
| **Testabilidade** | Design orientado a testes desde o início. |

---

## 2. Contexto do Projeto

> [!NOTE]
> Este projeto segue o "Caminho Dourado" (Golden Path) para monólitos modulares em .NET, priorizando clareza e manutenção sobre otimizações prematuras.

---

## 3. 🏗️ Arquitetura Proposta

### 3.1. Estrutura da Solução

```text
TaskMaster.sln
├── 📂 src
│   ├── 📦 TaskMaster.Api           # Entry Point & Presentation
│   ├── 📦 TaskMaster.Core          # Domain Logic (Dependency-Free)
│   └── 📦 TaskMaster.Infrastructure # Data Access & External Integrations
└── 📂 tests
    └── 🧪 TaskMaster.UnitTests     # Testes Isolados
```

### 3.2. Detalhamento das Camadas

#### 🔹 [Core Layer] (O Coração)
*   **Entidades:** `TaskItem` (Regras de negócio encapsuladas).
*   **Interfaces:** `ITaskRepository` (Contratos para inversão de dependência).
*   **DTOs:** `CreateTaskDto`, `UpdateTaskDto` (Contratos de dados externos).

#### 🔹 [Infrastructure Layer] (O Músculo)
*   **DbContext:** `AppDbContext` (EF Core).
*   **Repositórios:** `TaskRepository` (Implementação concreta de I/O).

#### 🔹 [API Layer] (A Face)
*   **Controllers:** Endpoints RESTful.
*   **Middleware:** Global Exception Handling.
*   **Validadores:** Regras de entrada com FluentValidation.

---

## 4. 🛠️ Checklist de Desenvolvimento (WBS)

> [!TIP]
> O acompanhamento detalhado do progresso e tarefas (To-Do) foi movido para um documento dedicado.
>
> 👉 **[Ver Checklist de Desenvolvimento (TODO.md)](./TODO.md)**

---

---

## 5. Estratégia de Verificação

### 🤖 5.1. Testes Automatizados
*   **Unit Tests:** Rodar `dotnet test` para garantir a lógica do Core.
*   **Integration Tests (Manual):** Validar fluxos completos via `.http` file.

### 👁️ 5.2. Verificação Manual
*   **Swagger/OpenAPI:** Validar documentação e testar endpoints interativamente.
*   **Code Review:** Revisar conformidade com SOLID e Clean Architecture.
