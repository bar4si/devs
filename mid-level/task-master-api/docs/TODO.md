# 🛠️ Checklist de Desenvolvimento (WBS)

> **Status Global:** 🚧 Fase 2.0 em Andamento
> **Última Atualização:** Fase 1.0 Concluída

---

### ✅ **1.0 Fundação e Estrutura do Projeto**
*(Fase Concluída)*
- [x] **1.1 Inicialização da Solução**
    - Criar solução vazia `TaskMaster`.
    - Criar projetos: `Api`, `Core`, `Infrastructure`, `UnitTests`.
- [x] **1.2 Configuração de Dependências**
    - Adicionar referências seguindo a *Dependency Rule* (Infra -> Core, Api -> Core/Infra).

### 🚀 **2.0 Modelagem do Domínio (Core Layer)**
- [x] **2.1 Entidades de Domínio**
    - Criar `TaskItem` com propriedades privadas e métodos públicos de negócio *(Rich Model)*.
- [x] **2.2 Contratos de Repositório**
    - Definir interface `ITaskRepository` *(Dependency Inversion)*.
- [x] **2.3 Objetos de Transferência (DTOs)**
    - Criar `CreateTaskDto`, `UpdateTaskDto` e `TaskResponseDto` *(Pattern DTO)*.

### 🧱 **3.0 Infraestrutura e Persistência de Dados**
- [x] **3.1 Configuração do EF Core**
    - Instalar pacotes NuGet (EF Core, InMemory).
    - Configurar `AppDbContext` e Mapeamentos *(ORM)*.
- [x] **3.2 Implementação de Repositórios**
    - Implementar `TaskEfRepository` seguindo o contrato definido no Core.

### 🔌 **4.0 API e Exposição de Serviços**
- [x] **4.1 Injeção de Dependência**
    - Configurar o container IoC em `Program.cs` (Wiring up).
- [x] **4.2 Controladores e Endpoints**
    - Criar `TasksController` com operações CRUD e verbos HTTP corretos.
    - Implementar Mapeamento Manual ou com AutoMapper.

### 🛡️ **5.0 Qualidade, Validação e Testes**
- [ ] **5.1 Validação de Dados**
    - Integrar `FluentValidation` para sanitização de input.
- [ ] **5.2 Tratamento de Erros**
    - Implementar Middleware Global para tratamento exceções.
- [ ] **5.3 Testes Unitários**
    - Escrever testes para Entidades e Serviços usandos xUnit.
