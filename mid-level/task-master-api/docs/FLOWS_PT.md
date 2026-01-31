# 📊 Fluxos do Projeto - TaskMaster API

Este documento utiliza diagramas **Mermaid** para explicar visualmente a arquitetura, o fluxo de dados e a evolução do projeto.

---

## 1. Arquitetura de Camadas (Clean Architecture)

O projeto segue os princípios da Arquitetura Limpa, onde a dependência sempre aponta para o centro (Core).


```mermaid
graph TD
    subgraph Presentation ["Camada de Apresentação (API)"]
        API["TaskMaster.Api"]
    end

    subgraph Infrastructure ["Camada de Infraestrutura"]
        EF["Entity Framework Core"]
        Dapper["Dapper (SQL Puro)"]
        Repo["Repositories"]
    end

    subgraph Core ["Camada de Domínio (Core)"]
        Entities["Entidades (TaskItem)"]
        Interfaces["Interfaces (ITaskRepository)"]
        DTOs["DTOs (Data Transfer Objects)"]
    end

    API --> Core
    API --> Infrastructure
    Infrastructure --> Core
    
    style Core fill:#f9f,stroke:#333,stroke-width:4px
    style Presentation fill:#bbf,stroke:#333
    style Infrastructure fill:#dfd,stroke:#333
```

---

## 2. Ciclo de Vida de uma Requisição (Request Flow)

O caminho que um dado percorre desde a chamada do cliente até o armazenamento no banco de dados.


```mermaid
sequenceDiagram
    participant Client as Cliente (Postman/Web)
    participant Controller as TasksController
    participant DTO as CreateTaskDto
    participant Entity as TaskItem (Entity)
    participant Repo as ITaskRepository (DI)
    participant DB as Banco de Dados (EF/Dapper)

    Client->>Controller: POST /api/tasks (JSON)
    Controller->>DTO: Model Binding & Validação
    DTO-->>Controller: Dados Válidos
    Controller->>Entity: Fabricar Entidade (new TaskItem)
    Entity-->>Controller: Entidade com Regras de Negócio
    Controller->>Repo: AddAsync(entity)
    Repo->>DB: INSERT INTO Tasks...
    DB-->>Repo: Sucesso
    Repo-->>Controller: Task salva
    Controller->>Client: 201 Created (TaskResponseDto)
```

---

## 3. Linha do Tempo de Desenvolvimento (Fases)

Evolução do projeto organizada por entregas incrementais.


```mermaid
timeline
    title Evolução TaskMaster API
    Fase 1 : Fundação da Solução : Projetos .csproj : Dependências e Clean Architecture
    Fase 2 : Domínio e Contratos : Entidades Ricas : Interfaces de Repositório : DTOs e Validações
    Fase 3 : Persistência : EF Core (Mappings) : Implementação de Repositórios : Dapper (Proof of Concept)
    Fase 4 : Exposição e Docs : Controllers (Endpoints) : Injeção de Dependência : Scalar (API Docs)
```

---

## 4. Injeção de Dependência (DI) e Plugins

Nossa arquitetura permite trocar o "motor" de persistência sem alterar a lógica de negócio ou os controladores.


```mermaid
graph TD
    Controller["TasksController"] -- solicita --> Interface["ITaskRepository"]
    
    subgraph DIContainer ["Contêiner IoC (Program.cs)"]
        Interface -. "injeta" .-> EF["TaskEfRepository"]
        Interface -. "ou injeta" .-> SQL["TaskSqlRepository"]
    end
    
    EF -- usa --> EFCore["EF Core (In-Memory/SQL)"]
    SQL -- usa --> Dapper["Dapper (SQL Server)"]
```

> [!TIP]
> Para entender os termos usados nestes fluxos, consulte o [Glossário Geral](GLOSSARIO_GERAL_PT.md).
