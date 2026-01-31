# 📖 Glossário Geral - TaskMaster API

Este documento consolida todos os termos técnicos, padrões e conceitos introduzidos ao longo das fases de desenvolvimento do projeto TaskMaster, organizados alfabeticamente.

| Fase | Termo (PT) | Termo (EN) | Definição |
| :--- | :--- | :--- | :--- |
| 2.3 | **Acoplamento** | **Coupling** | Grau de dependência entre dois módulos. O objetivo é sempre buscar **baixo acoplamento**. |
| 2.3 | **Anotações de Dados** | **Data Annotations** | Atributos (ex: `[Required]`) usados para validação declarativa diretamente nas classes C#. |
| 3.1 | **API Fluente** | **Fluent API** | Forma de configurar o modelo via código (encadeamento de métodos), alternativa a atributos. |
| 1 | **Arquitetura Limpa** | **Clean Architecture** | Padrão arquitetural que isola a lógica de domínio de detalhes externos (Banco, UI). |
| 3.2 | **AsNoTracking** | **AsNoTracking** | Comando que desativa o tracking do EF Core, aumentando a performance em consultas de leitura. |
| 2.2 | **Assíncrono** | **Async/Await** | Modelo de programação que libera a thread enquanto espera por operações de I/O (Banco, API). |
| 2.3 | **Atribuição em Massa** | **Mass Assignment** | Vulnerabilidade de segurança onde campos extras enviados por um atacante são salvos indevidamente. |
| 3.1 | **Banco em Memória** | **In-memory DB** | Banco de dados que vive apenas na memória RAM, ideal para testes rápidos e desenvolvimento. |
| 1 | **Biblioteca de Classes** | **Class Library** | Projeto que compila para uma DLL, usado para compartilhamento de lógica entre aplicações. |
| 4.1 | **Connection String** | **Connection String** | Texto que contém o endereço, nome do banco e credenciais para conexão com o DB. |
| 3.1 | **Contexto de Dados** | **DbContext** | Classe central do EF Core que representa uma sessão com o banco de dados. |
| 4.1 | **Contêiner DI** | **DI Container** | Motor do ASP.NET Core que gerencia a injeção de dependências e os serviços registrados. |
| 2.1 | **Encapsulamento** | **Encapsulation** | Princípio de esconder detalhes de implementação e expor apenas o necessário para o uso seguro do objeto. |
| 4.2 | **Endpoint** | **Endpoint** | Ponto de acesso de uma URL na API onde uma funcionalidade é exposta. |
| 2.3 | **Factory Method** | **Factory Method** | Método/Padrão usado para encapsular e centralizar a criação de objetos complexos ou conversões (ex: `FromEntity`). |
| 2.1 | **Guid** | **Guid** | Identificador Único Global (128 bits) usado para garantir IDs exclusivos sem depender de auto-incremento do banco. |
| 4.2 | **IActionResult** | **IActionResult** | Tipo de retorno flexível do Controller que pode representar JSON, Status Codes ou erros. |
| 3.2 | **Implementação Concreta** | **Concrete Implementation** | O código real que realiza uma tarefa definida por uma interface. |
| 2.1 | **Imutabilidade** | **Immutability** | Característica de um objeto ou propriedade que não pode ser alterado após sua criação. |
| 1 | **Injeção de Dependência** | **Dependency Injection (DI)** | Técnica onde as dependências de uma classe são fornecidas externamente, facilitando testes e desacoplamento. |
| 2.3 | **Input/Output Model** | **Input/Output Model** | Diferenciação entre DTOs usados na entrada (Input) e na resposta (Output/Response) da API. |
| 1 | **Interface de Linha de Comando** | **CLI** | Interface para executar comandos via texto (ex: `dotnet new`, `dotnet build`). |
| 4.1 | **Inversão de Controle** | **IoC** | Conceito onde o framework (e não seu código) controla a criação e o ciclo de vida dos objetos. |
| 2.2 | **Inversão de Dependência** | **Dependency Inversion** | O "D" do SOLID: Módulos de alto nível não devem depender de módulos de baixo nível; ambos devem depender de abstrações. |
| 3.1 | **Mapeamento Objeto-Relacional** | **ORM** | Ferramenta que traduz objetos C# para tabelas de bancos relacionais (ex: EF Core). |
| 3.2 | **Micro-ORM** | **Micro-ORM** | Ferramenta de acesso a dados mais leve que um ORM completo, focada em performance (ex: Dapper). |
| 2.2 | **Mock** | **Mock Object** | Objeto simulado usado em testes para representar uma dependência sem precisar da implementação real. |
| 4.2 | **Model Binding** | **Model Binding** | Mecanismo que converte dados brutos do HTTP (JSON, URL) em objetos C# automaticamente. |
| 2.1 | **Modelo Anêmico** | **Anemic Domain Model** | Anti-padrão onde classes de domínio são apenas "sacos de dados" (GET/SET) sem lógica interna. |
| 2.1 | **Modelo Rico de Domínio** | **Rich Domain Model** | Classes de domínio que contêm dados e comportamento (regras de negócio), protegendo seu estado. |
| 2.3 | **Objeto de Transferência** | **DTO** | Data Transfer Object: Objeto simples usado para transportar dados entre processos (ex: JSON da API). |
| 4.2 | **OpenAPI** | **OpenAPI** | Especificação padrão para descrever e documentar APIs de forma que máquinas possam ler. |
| 2.1 | **private set** | **private set** | Modificador de acesso que permite a alteração de uma propriedade apenas dentro da própria classe. |
| 1 | **Projeto** | **Project (.csproj)** | Arquivo que define as propriedades, dependências e itens de um projeto individual. |
| 3.1 | **Reflection** | **Reflection** | Técnica que permite que o programa inspecione sua própria estrutura em tempo de execução. |
| 2.2 | **Repositório** | **Repository Pattern** | Abstração que faz com que o acesso a dados pareça uma coleção em memória (ex: `ITaskRepository`). |
| 4.2 | **RESTful** | **RESTful** | Uma API que segue fielmente os princípios e restrições da arquitetura REST. |
| 4.2 | **Scalar** | **Scalar** | Interface interativa moderna para documentar e testar APIs, sucessora moderna do Swagger UI. |
| 4.1 | **Serviços Chaveados** | **Keyed Services** | Recurso do .NET 8+ para registrar múltiplas implementações de uma mesma interface usando chaves. |
| 2.2 | **SOLID** | **SOLID** | Conjunto de cinco princípios de design (SRP, OCP, LSP, ISP, DIP) para software orientado a objetos mais robusto. |
| 1 | **Solução** | **Solution (.sln)** | Um arquivo container que agrupa múltiplos projetos no ecossistema .NET. |
| 4.2 | **Status Code** | **Status Code** | Código numérico da resposta HTTP (200=Sucesso, 400=Erro do Cliente, 500=Erro do Servidor). |
| 4.1 | **Tempo de Vida** | **Lifetime** | Define por quanto tempo um serviço vive no contêiner (Singleton, Scoped ou Transient). |
| 1 | **Testes de Unidade** | **Unit Tests (xUnit)** | Testes focados em pequenas unidades de código (métodos/classes) em completo isolamento. |
| 3.2 | **Tracking** | **Change Tracking** | Mecanismo do EF Core que monitora se as entidades foram alteradas para salvar no DB. |
| 4.2 | **Verbo HTTP** | **HTTP Verb** | Indica a ação desejada: GET (ler), POST (criar), PUT (atualizar), DELETE (excluir). |
