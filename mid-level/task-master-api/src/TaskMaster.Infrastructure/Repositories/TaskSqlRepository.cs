using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TaskMaster.Core.Entities;
using TaskMaster.Core.Interfaces;

namespace TaskMaster.Infrastructure.Repositories;

/// <summary>
/// Exemplo de Implementação alternativa usando Dapper e SQL Puro.
/// Demonstra o poder da Injeção de Dependência: poderíamos trocar EF por Dapper mudando 1 linha no Program.cs.
/// </summary>
public class TaskSqlRepository : ITaskRepository
{
    private readonly string _connectionString;

    public TaskSqlRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

    public async Task AddAsync(TaskItem task)
    {
        using var connection = CreateConnection();
        var sql = @"
            INSERT INTO Tasks (Id, Title, Description, IsCompleted, DueDate, Priority, CreatedAt)
            VALUES (@Id, @Title, @Description, @IsCompleted, @DueDate, @Priority, @CreatedAt)";
        
        await connection.ExecuteAsync(sql, task);
    }

    public async Task DeleteAsync(Guid id)
    {
        using var connection = CreateConnection();
        var sql = "DELETE FROM Tasks WHERE Id = @Id";
        await connection.ExecuteAsync(sql, new { Id = id });
    }

    public async Task<IEnumerable<TaskItem>> GetAllAsync()
    {
        using var connection = CreateConnection();
        var sql = "SELECT * FROM Tasks";
        return await connection.QueryAsync<TaskItem>(sql);
    }

    public async Task<TaskItem?> GetByIdAsync(Guid id)
    {
        using var connection = CreateConnection();
        var sql = "SELECT * FROM Tasks WHERE Id = @Id";
        return await connection.QuerySingleOrDefaultAsync<TaskItem>(sql, new { Id = id });
    }

    public async Task UpdateAsync(TaskItem task)
    {
        using var connection = CreateConnection();
        var sql = @"
            UPDATE Tasks 
            SET Title = @Title, 
                Description = @Description, 
                IsCompleted = @IsCompleted, 
                DueDate = @DueDate, 
                Priority = @Priority
            WHERE Id = @Id";
            
        await connection.ExecuteAsync(sql, task);
    }
}
