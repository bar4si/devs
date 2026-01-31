using System;
using System.ComponentModel.DataAnnotations;
using TaskMaster.Core.Entities;

namespace TaskMaster.Core.DTOs;

/// <summary>
/// Objeto de Transferência de Dados (DTO) para atualização de uma tarefa existente.
/// </summary>
public class UpdateTaskDto
{
    /// <summary>
    /// Novo título da tarefa.
    /// </summary>
    [Required(ErrorMessage = "O título é obrigatório")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Nova descrição detalhada.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Nova data de vencimento.
    /// </summary>
    public DateTime? DueDate { get; set; }

    /// <summary>
    /// Nova prioridade.
    /// </summary>

    public TaskPriority Priority { get; set; }

    /// <summary>
    /// Define se a tarefa foi concluída.
    /// </summary>
    public bool IsCompleted { get; set; }
}
