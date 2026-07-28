using Microsoft.AspNetCore.Mvc;

namespace TodoApp.Controllers;

using TodoApp.Models;
using TodoApp.Services;

public class HomeController : Controller
{
    private readonly ITodoService _todoService;
    private readonly ILogger<HomeController> _logger;
    public HomeController(ITodoService todoService,ILogger<HomeController> logger)
    {
        _todoService = todoService;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var todos = await _todoService.GetAllAsync();
        var vm=new TodoViewModel { Todos= todos.ToList()};
        return View(vm);
    }

    public IActionResult Create() => View(new TodoItem());
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TodoItem todo)
    {
        if (ModelState.IsValid)
        {
            await _todoService.CreateAsync(todo);
            return RedirectToAction(nameof(Index));
        }
        return View(todo);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var todo = await _todoService.GetByIdAsync(id);
        if (todo == null) return NotFound();
        return View(todo);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]

    public async Task<IActionResult> Edit(int id, TodoItem todo)
    {
        if (id != todo.Id) return BadRequest();
        if (ModelState.IsValid)
        {
            await _todoService.UpdateAsync(id, todo);
            return RedirectToAction(nameof(Index));
        }
        return View(todo);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var todo = await _todoService.GetByIdAsync(id);
        if (todo == null) return NotFound();
        return View(todo);
    }
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _todoService.DeleteAsync(id);
         return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> ToggleComplete(int id)
    {
        var todo = await _todoService.ToggleCompleteAsync(id);
        if (todo == null) return NotFound();

        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            return Json(new { success = true, isCompleted = todo.IsCompleted });

        return RedirectToAction(nameof(Index));
    }
}

