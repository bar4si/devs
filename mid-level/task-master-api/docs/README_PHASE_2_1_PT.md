# 🚀 Fase 2.1: Modelagem do Domínio (Rich Domain Model)

## Índice

- [1. O que foi feito?](#1-o-que-foi-feito)
- [2. Como foi feito?](#2-como-foi-feito)
    - [2.1 Pacotes NuGet Instalados](#21-pacotes-nuget-instalados)
    - [2.2 Principais Características do Código](#22-principais-características-do-código)
- [3. Por que foi feito assim?](#3-por-que-foi-feito-assim)
- [4. Glossário](#4-glossário)
- [5. Arquivos Gerados/Modificados](#5-arquivos-geradosmodificados)

---

## 1. O que foi feito?
Iniciamos a construção do "coração" do sistema: a camada **Core**.

Criamos a entidade `TaskItem`, que representa a nossa tarefa. Porém, ao invés de criar uma classe simples apenas para guardar dados ("saco de dados"), criamos uma **Entidade Rica**, que protege suas próprias regras de negócio.

## 2. Como foi feito?
Escrevemos a classe `TaskItem.cs` dentro do projeto `TaskMaster.Core`.

### Principais Características do Código:
1.  **Propriedades com `private set`**: Bloqueamos a alteração direta de fora da classe.
2.  **Construtor Validado**: Obrigamos quem criar a tarefa a fornecer os dados mínimos (Título, Prioridade).
3.  **Métodos de Negócio**: Métodos como `MarkAsCompleted()` e `UpdateDetails()` centralizam a lógica de alteração.

```csharp
public class TaskItem
{
    // Ninguém muda o ID depois de criado
    public Guid Id { get; private set; } 
    
    // Construtor garante estado válido inicial
    public TaskItem(string title, ...) 
    {
        Id = Guid.NewGuid(); // Criação automática do ID
        // ...
    }

    // Regra de negócio explícita
    public void MarkAsCompleted()
    {
        if (IsCompleted) return;
        IsCompleted = true;
    }
}
```

## 3. Por que foi feito assim?

### Anemic vs Rich Domain Model
A maior diferença entre um código Júnior e Pleno/Sênior está aqui.

*   **❌ Modelo Anêmico (Júnior):**
    ```csharp
    var task = new TaskItem();
    task.Title = ""; // Permite título vazio?
    task.IsCompleted = true; // Permite completar sem validar?
    ```
    *Problema:* A regra de negócio fica espalhada pelos Controllers e Services. Se você tiver 5 lugares que atualizam uma tarefa, terá que repetir a validação em 5 lugares.

*   **✅ Modelo Rico (Nosso):**
    ```csharp
    var task = new TaskItem("Estudar", ...); // Obriga título
    task.UpdateDetails("", ...); // Lança Exceção: "Title cannot be empty"
    ```
    *Vantagem:* A regra está **dentro** da entidade. É impossível deixar o sistema em um estado inválido. Onde quer que você use `TaskItem`, a regra vai junto.

### Encapsulamento
Protegemos propriedades críticas como `Id` e `CreatedAt`. Não faz sentido permitir que o Controller ou a Interface alterem a data de criação da tarefa. Isso garante a **integridade dos dados**.

## 4. Glossário

| Termo (PT) | Termo (EN) | Definição |
| :--- | :--- | :--- |
| **Modelo Rico de Domínio** | **Rich Domain Model** | Classes que contêm dados E comportamento, protegendo suas regras. |
| **Modelo Anêmico** | **Anemic Domain Model** | Anti-pattern onde classes são apenas "sacos de dados" sem lógica. |
| **Encapsulamento** | **Encapsulation** | Ocultar detalhes internos e expor apenas o necessário/seguro. |
| **Imutabilidade** | **Immutability** | Capacidade de um objeto (ou propriedade) não ser alterado após criado. |
| **private set** | **private set** | Modificador que permite que o valor de uma propriedade seja alterado apenas dentro da própria classe. |
| **Guid** | **Guid** | Identificador Global Único (128 bits) usado para garantir IDs exclusivos para cada entidade. |

## 5. Arquivos Gerados/Modificados

| Tipo | Arquivo | Descrição |
| :--- | :--- | :--- |
| **Código Fonte** | `src/TaskMaster.Core/Entities/TaskItem.cs` | Definição da Entidade com regras de negócio ricas. |
| **Documentação** | `docs/TODO.md` | Lista mestre de tarefas (WBS) e status. |
| **Documentação** | `docs/README_PHASE_2_1_PT.md` | Este documento explicativo da fase. |
| **Documentação** | `docs/task-master-api-implementation-plan.md` | Plano original atualizado (WBS movido para TODO). |
