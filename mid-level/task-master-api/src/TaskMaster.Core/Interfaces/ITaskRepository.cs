using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskMaster.Core.Entities;

namespace TaskMaster.Core.Interfaces;

/// <summary>
/// Interface de Repositório para o agregado TaskItem.
/// Define o contrato para acesso a dados, permitindo Inversão de Dependência (Infraestrutura depende do Core).
/// </summary>
public interface ITaskRepository
{
    /// <summary>
    /// Recupera uma tarefa pelo seu identificador único.
    /// </summary>
    Task<TaskItem?> GetByIdAsync(Guid id);

    /// <summary>
    /// Recupera todas as tarefas.
    /// </summary>
    Task<IEnumerable<TaskItem>> GetAllAsync();

    /// <summary>
    /// Adiciona uma nova tarefa ao repositório.
    /// </summary>
    Task AddAsync(TaskItem task);

    /// <summary>
    /// Atualiza uma tarefa existente no repositório.
    /// </summary>
    Task UpdateAsync(TaskItem task);

    /// <summary>
    /// Remove uma tarefa do repositório.
    /// </summary>
    Task DeleteAsync(Guid id);
}
