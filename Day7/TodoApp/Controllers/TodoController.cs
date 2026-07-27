using Microsoft.AspNetCore.Mvc;
using TodoApp.Models;
using TodoApp.Services;

namespace TodoApp.Controllers;


[Route("api/[controller]")]
[ApiController]
public class TodoController : ControllerBase
{

    private readonly ITodoService _todoService;
    private readonly ILogger<TodoController> _logger;

    public TodoController(ITodoService todoService, ILogger<TodoController> logger)
    {
        _todoService = todoService;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]

    public async Task<ActionResult<IEnumerable<TodoItem>>> GetAll()
    {
        var todos = await _todoService.GetAllAsync();
        return Ok(todos);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TodoItem>> GetById(int id)
    {
        var todo = await _todoService.GetByIdAsync(id);
        if (todo == null)
            return NotFound(new { message = $"Todo with ID {id} not found" });
        return Ok(todo);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TodoItem>> Create([FromBody] TodoItem todo)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var created = await _todoService.CreateAsync(todo);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public async Task<ActionResult<TodoItem>> Update(int id , [FromBody] TodoItem todo)
    {
        if (id != todo.Id)
            return BadRequest(new { message = "Id mismatch" });

        var updated = await _todoService.UpdateAsync(todo);
        if(updated==null)
            return NotFound(new { message = $"Todo with ID {id} not found" });

        return Ok(updated);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]

    public async Task<IActionResult>
        {
        var deleted = await _todoService.DeleteAsync(id);
        if (!deleted)
            return NotFound(new { message = $"Todo with ID {id} not found" });

        return NoContent();
    }


    [HttpPatch("{id}/toggle")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TodoItem>> ToggleComplete(int id)
    {
        var todo = await _todoService.ToggleCompleteAsync(id);
        if (todo == null)
            return NotFound(new { message = $"Todo with ID {id} not found" });
        return Ok(todo);
    }
}
