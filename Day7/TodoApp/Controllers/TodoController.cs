using Microsoft.AspNetCore.Mvc;

namespace TodoApp.Controllers
{
    public class TodoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
