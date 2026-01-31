using System;
using TaskMaster.Core.Entities;

namespace TaskMaster.Core.DTOs;

/// <summary>
/// Objeto de Transferência de Dados (DTO) para resposta de operações de tarefa.
/// Exclui dados sensíveis ou internos que não devem ser expostos diretamente.
/// </summary>
public class TaskResponseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public DateTime? DueDate { get; set; }
    public TaskPriority Priority { get; set; }
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Construtor padrão.
    /// </summary>
    public TaskResponseDto() { }

    /// <summary>
    /// Mapeia uma entidade TaskItem para TaskResponseDto.
    /// </summary>
    public static TaskResponseDto FromEntity(TaskItem task)
    {
        return new TaskResponseDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            IsCompleted = task.IsCompleted,
            DueDate = task.DueDate,
            Priority = task.Priority,
            CreatedAt = task.CreatedAt
        };
    }
}
