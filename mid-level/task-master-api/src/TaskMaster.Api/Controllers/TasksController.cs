using Microsoft.AspNetCore.Mvc;
using TaskMaster.Core.DTOs;
using TaskMaster.Core.Entities;
using TaskMaster.Core.Interfaces;

namespace TaskMaster.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskRepository _repository;

    // Injeção de Dependência: O container vai fornecer a implementação configurada (EF ou Dapper)
    public TasksController(ITaskRepository repository)
    {
        _repository = repository;
    }

    /* 
    // Exemplo de como usar Keyed Services para forçar o uso do SQL (Dapper):
    public TasksController([FromKeyedServices("sql")] ITaskRepository repository)
    {
        _repository = repository;
    }
    */

    /// <summary>
    /// Recupera todas as tarefas cadastradas.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TaskResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var tasks = await _repository.GetAllAsync();
        
        // Mapeamento Entidade -> DTO
        var dtos = tasks.Select(TaskResponseDto.FromEntity);
        
        return Ok(dtos);
    }

    /// <summary>
    /// Recupera uma tarefa específica pelo ID.
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TaskResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var task = await _repository.GetByIdAsync(id);

        if (task == null)
            return NotFound();

        return Ok(TaskResponseDto.FromEntity(task));
    }

    /// <summary>
    /// Cria uma nova tarefa.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateTaskDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Mapeamento DTO -> Entidade (Factory)
        var task = new TaskItem(dto.Title, dto.Description, dto.DueDate, dto.Priority);

        await _repository.AddAsync(task);

        // Retorna 201 Created com o cabeçalho Location apontando para o novo recurso
        return CreatedAtAction(nameof(GetById), new { id = task.Id }, TaskResponseDto.FromEntity(task));
    }

    /// <summary>
    /// Atualiza uma tarefa existente.
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTaskDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existingTask = await _repository.GetByIdAsync(id);
        if (existingTask == null)
            return NotFound();

        // Atualiza a entidade com dados do DTO
        existingTask.UpdateDetails(dto.Title, dto.Description, dto.DueDate, dto.Priority);

        if (dto.IsCompleted)
            existingTask.MarkAsCompleted();
            // Nota: Se houver lógica para "descompletar", precisaríamos de um método na entidade.
            // Por enquanto, assumimos que só marca como feito.

        await _repository.UpdateAsync(existingTask);

        return NoContent();
    }

    /// <summary>
    /// Remove uma tarefa.
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var existingTask = await _repository.GetByIdAsync(id);
        if (existingTask == null)
            return NotFound();

        await _repository.DeleteAsync(id);

        return NoContent();
    }
}
