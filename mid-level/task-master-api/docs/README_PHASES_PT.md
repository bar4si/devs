# 🗺️ Roadmap de Desenvolvimento: TaskMaster API

Bem-vindo ao guia mestre do projeto **TaskMaster API**. Este documento serve como um índice central para navegar por todas as fases de desenvolvimento, desde a fundação da arquitetura até a exposição dos endpoints.

Cada fase foi documentada com foco em boas práticas de engenharia de software, separação de preocupações e design patterns modernos.

---

## 📌 Índice de Fases

| Fase | Título | Foco Principal | Documento |
| :--- | :--- | :--- | :--- |
| **1** | [Fundação e Estrutura](#fase-1-fundação-e-estrutura-do-projeto) | Solução, Projetos e Camadas. | [ver PDF/MD](README_PHASE_1_PT.md) |
| **2.1** | [Modelagem do Domínio](#fase-21-modelagem-do-domínio-rich-domain-model) | Entidades Ricas e Regras de Negócio. | [ver PDF/MD](README_PHASE_2_1_PT.md) |
| **2.2** | [Contratos de Repositório](#fase-22-contratos-de-repositório-dependency-inversion) | Interfaces e Inversão de Dependência. | [ver PDF/MD](README_PHASE_2_2_PT.md) |
| **2.3** | [Objetos de Transferência (DTOs)](#fase-23-objetos-de-transferência-dtos) | Contratos de Entrada/Saída e Validação. | [ver PDF/MD](README_PHASE_2_3_PT.md) |
| **3.1** | [Configuração do EF Core](#fase-31-configuração-do-entity-framework-core) | Mapeamento e Contexto de Dados. | [ver PDF/MD](README_PHASE_3_1_PT.md) |
| **3.2** | [Implementação de Repositórios](#fase-32-implementação-de-repositórios) | Acesso a Dados (EF Core & Dapper). | [ver PDF/MD](README_PHASE_3_2_PT.md) |
| **4.1** | [Injeção de Dependência](#fase-41-injeção-de-dependência) | Ciclo de Vida e Registro de Serviços. | [ver PDF/MD](README_PHASE_4_1_PT.md) |
| **4.2** | [Controladores e Endpoints](#fase-42-controladores-e-endpoints) | HTTP, Rotas e Documentação Scalar. | [ver PDF/MD](README_PHASE_4_2_PT.md) |

---

## 📖 Detalhes das Fases

### Fase 1: Fundação e Estrutura do Projeto
Estabelecimento da "espinha dorsal" da aplicação utilizando **Clean Architecture**. Criação da solução .NET e dos projetos `Core`, `Infrastructure`, `Api` e `UnitTests`.

### Fase 2.1: Modelagem do Domínio (Rich Domain Model)
Construção do coração do sistema. Implementação de entidades ricas com encapsulamento e proteção de invariantes, evitando o anti-padrão de "Modelos Anêmicos".

### Fase 2.2: Contratos de Repositório (Dependency Inversion)
Definição dos contratos de abstração para persistência de dados. Aplicação do princípio de **Inversão de Dependência (DIP)** do SOLID.

### Fase 2.3: Objetos de Transferência (DTOs)
Desacoplamento entre as entidades internas e a interface externa. Implementação de validações via *Data Annotations* e padrões de entrada/saída.

### Fase 3.1: Configuração do Entity Framework Core
Configuração da ponte entre C# e SQL. Uso de **Fluent API** para mapeamentos limpos e configuração do `AppDbContext`.

### Fase 3.2: Implementação de Repositórios
Implementação concreta do acesso a dados. Demonstração de flexibilidade arquitetural com repositórios baseados em **EF Core** e também em **Dapper (SQL Puro)** para alta performance.

### Fase 4.1: Injeção de Dependência
Configuração do contêiner de IoC (Inversion of Control). Gerenciamento de ciclos de vida (`Scoped`) e registro de serviços dinâmicos.

### Fase 4.2: Controladores e Endpoints
Exposição da API para o mundo exterior. Mapeamento de verbos HTTP para operações de domínio e integração com a documentação interativa **Scalar**.

---

## 🛠️ Documentos Auxiliares

Além das fases de desenvolvimento, consulte estes documentos para uma visão transversal do projeto:

- [📚 **Glossário Geral**](GLOSSARIO_GERAL_PT.md): Definição de termos técnicos e arquiteturais.
- [🔗 **Fluxos de Dados**](FLOWS_PT.md): Visualização de como os dados percorrem as camadas.
- [📦 **Pacotes e Dependências**](PACKAGES_GERAL_PT.md): Lista completa de ferramentas NuGet utilizadas.
- [📝 **Lista de Tarefas (TODO)**](TODO.md): Status atualizado de cada subitem do projeto.

---

> [!TIP]
> Para uma compreensão completa, recomenda-se seguir a leitura na ordem numérica das fases.
