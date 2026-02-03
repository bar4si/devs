# 🎯 TaskMaster API

Uma aplicação de gerenciamento de tarefas robusta e escalável, desenvolvida em **.NET 10** seguindo os princípios de **Clean Architecture** e **SOLID**. Este projeto serve como uma demonstração técnica de práticas modernas de backend e design de sistemas.

---

## 🏗️ Arquitetura e Camadas

O sistema foi estruturado para garantir o desacoplamento e a facilidade de manutenção, separando as responsabilidades em 4 camadas principais:

- **`TaskMaster.Core`**: O coração do sistema. Contém entidades ricas, regras de negócio e contratos (interfaces).
- **`TaskMaster.Infrastructure`**: Implementação de acesso a dados (EF Core & Dapper), logs e comunicação externa.
- **`TaskMaster.Api`**: Ponto de entrada RESTful. Gerencia rotas, injeção de dependência e documentação.
- **`TaskMaster.UnitTests`**: Garantia de qualidade através de testes isolados da lógica de domínio.

---

## 💎 Por que este projeto é "Clean"?

A aplicação segue fielmente os círculos da **Clean Architecture** (Arquitetura Limpa) de Uncle Bob:

1.  **Independência de Frameworks:** O Core não depende de bibliotecas externas (como EF Core ou ASP.NET). Ele contém apenas C# puro e lógica de negócio.
2.  **Testabilidade:** Toda a regra de negócio reside no Core e pode ser testada sem a necessidade de um banco de dados real ou servidor web.
3.  **Independência de Banco de Dados:** A camada de dados é um detalhe. Graças às interfaces, o sistema pode alternar entre SQL Server (EF Core) e Dapper sem afetar o domínio.
4.  **Independência da UI:** O motor da aplicação é agnóstico. Se amanhã decidirmos criar um App Desktop ou um Job agendado, o Core e a Infrastructure permanecerão intocados.

---

## 🏗️ Princípios SOLID Aplicados

O código foi escrito com foco em manutenibilidade e escalabilidade, aplicando os 5 princípios SOLID:

- **S (Single Responsibility):** Cada classe tem um objetivo único. O `TasksController` lida apenas com HTTP, enquanto o `TaskRepository` lida apenas com persistência.
- **O (Open/Closed):** O sistema é aberto para extensão e fechado para modificação. Podemos adicionar novas formas de persistência (ex: NoSQL) simplesmente criando uma nova classe que implementa `ITaskRepository`.
- **L (Liskov Substitution):** Tanto o `TaskEfRepository` quanto o `TaskSqlRepository` honram o contrato da interface e podem ser substituídos sem quebrar o comportamento do sistema.
- **I (Interface Segregation):** Nossas interfaces são magras e específicas. O Core define apenas o que realmente precisa para realizar as operações de negócio.
- **D (Dependency Inversion):** Este é o pilar principal. O Core (alto nível) não depende da Infrastructure (baixo nível). Ambos dependem de abstrações (Interfaces), garantindo um desacoplamento total.

---

## 🛠️ Tecnologias Utilizadas

- **Framework**: .NET 10 (ASP.NET Core)
- **Acesso a Dados**: Entity Framework Core & Dapper (Abordagem Híbrida)
- **Banco de Dados**: SQL Server (ou In-Memory para desenvolvimento rápido)
- **Documentação**: Scalar (OpenAPI 3.1)
- **Testes**: xUnit & Coverlet

---

## 🚀 Como Executar o Projeto

### Pré-requisitos
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- SQL Server LocalDB (opcional, por padrão usa In-Memory)

### Comandos CLI
```powershell
# Restaurar dependências
dotnet restore

# Compilar a solução
dotnet build

# Executar a API
dotnet run --project src/TaskMaster.Api
```

### Documetação da API
Após rodar o projeto, acesse a interface interativa em:
`https://localhost:PORTA/scalar/v1`

---

## 🗺️ Roadmap e Documentação Detalhada

O desenvolvimento deste projeto foi dividido em fases incrementais, cada uma documentada detalhadamente para fins de aprendizado e revisão técnica.

> [!IMPORTANT]
> **[Acessar Guia de Fases (Roadmap de Desenvolvimento)](docs/README_PHASES_PT.md)**

Consulte o link acima para entender o "porquê" de cada decisão técnica, desde a modelagem do domínio até a configuração da injeção de dependência.

---

## 📁 Estrutura de Documentos Auxiliares
- [📚 Glossário Geral](docs/GLOSSARIO_GERAL_PT.md)
- [🔗 Fluxos de Processos](docs/FLOWS_PT.md)
- [📦 Pacotes NuGet](docs/PACKAGES_GERAL_PT.md)
- [📝 Lista de Pendências (TODO)](docs/TODO.md)
