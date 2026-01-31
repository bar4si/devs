using Microsoft.EntityFrameworkCore;
using TaskMaster.Core.Entities;
using TaskMaster.Core.Interfaces;
using TaskMaster.Infrastructure.Data;

namespace TaskMaster.Infrastructure.Repositories;

/// <summary>
/// Implementação concreta do Repositório de Tarefas usando Entity Framework Core.
/// </summary>
public class TaskEfRepository : ITaskRepository
{
    private readonly AppDbContext _context;

    public TaskEfRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Recupera uma tarefa pelo ID.
    /// </summary>
    public async Task<TaskItem?> GetByIdAsync(Guid id)
    {
        return await _context.Tasks.FindAsync(id);
    }

    /// <summary>
    /// Recupera todas as tarefas.
    /// Observação: Em produção, seria ideal ter paginação aqui.
    /// </summary>
    public async Task<IEnumerable<TaskItem>> GetAllAsync()
    {
        return await _context.Tasks.AsNoTracking().ToListAsync();
    }

    /// <summary>
    /// Adiciona uma nova tarefa e persiste no banco.
    /// </summary>
    public async Task AddAsync(TaskItem task)
    {
        await _context.Tasks.AddAsync(task);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Atualiza uma tarefa existente.
    /// </summary>
    public async Task UpdateAsync(TaskItem task)
    {
        _context.Tasks.Update(task);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Remove uma tarefa pelo ID.
    /// </summary>
    public async Task DeleteAsync(Guid id)
    {
        var task = await GetByIdAsync(id);
        if (task != null)
        {
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
        }
    }
}
