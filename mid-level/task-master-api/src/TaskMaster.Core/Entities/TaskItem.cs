using System;

namespace TaskMaster.Core.Entities;

/// <summary>
/// Entidade de Domínio representando uma Tarefa.
/// Implementa princípios de Modelo Rico (Rich Domain Model - Fase 2.1) encapsulando lógica e validação.
/// </summary>
public class TaskItem
{
    // Propriedades com 'private set' para forçar o Encapsulamento.
    // Elas só podem ser modificadas através de métodos específicos (UpdateDetails, MarkAsCompleted).
    public Guid Id { get; private set; }
    public string Title { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public bool IsCompleted { get; private set; }
    public DateTime? DueDate { get; private set; }
    public TaskPriority Priority { get; private set; }
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Construtor privado exigido pelo EF Core para materialização.
    /// Código externo não pode criar uma tarefa vazia/inválida.
    /// </summary>
    private TaskItem() { }

    /// <summary>
    /// Cria um novo TaskItem corretamente inicializado.
    /// Garante que a entidade esteja sempre válida desde a criação (Conceito de Factory method).
    /// </summary>
    public TaskItem(string title, string description, DateTime? dueDate, TaskPriority priority)
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        IsCompleted = false;
        
        // Reutiliza a lógica de validação para evitar duplicação (DRY).
        UpdateDetails(title, description, dueDate, priority);
    }

    /// <summary>
    /// Atualiza os detalhes da tarefa aplicando regras de domínio.
    /// Exemplo de Modelo Rico: A própria entidade se valida.
    /// </summary>
    public void UpdateDetails(string title, string description, DateTime? dueDate, TaskPriority priority)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("O título não pode estar vazio", nameof(title));

        Title = title;
        Description = description;
        DueDate = dueDate;
        Priority = priority;
    }

    /// <summary>
    /// Marca a tarefa como concluída.
    /// Encapsula a lógica de transição de estado.
    /// </summary>
    public void MarkAsCompleted()
    {
        if (IsCompleted) return;
        
        IsCompleted = true;
    }
}

public enum TaskPriority
{
    Low,
    Medium,
    High,
    Critical
}
