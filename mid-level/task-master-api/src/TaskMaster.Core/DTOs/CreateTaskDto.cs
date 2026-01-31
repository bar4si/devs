using System;
using System.ComponentModel.DataAnnotations;
using TaskMaster.Core.Entities;

namespace TaskMaster.Core.DTOs;

/// <summary>
/// Objeto de Transferência de Dados (DTO) para criação de uma nova tarefa.
/// Encapsula os dados necessários enviados pelo cliente.
/// </summary>
public class CreateTaskDto
{
    /// <summary>
    /// Título da tarefa. Obrigatório.
    /// </summary>
    [Required(ErrorMessage = "O título é obrigatório")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Descrição detalhada da tarefa.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Data de vencimento opcional.
    /// </summary>
    public DateTime? DueDate { get; set; }

    /// <summary>
    /// Prioridade da tarefa.
    /// </summary>
    public TaskPriority Priority { get; set; }
}
