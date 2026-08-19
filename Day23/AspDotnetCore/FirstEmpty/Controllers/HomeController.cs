using Microsoft.AspNetCore.Mvc;

namespace FirstEmpty.Controllers
{
    public class HomeController : Controller
    {
        
        public IActionResult Index()
        {
           ViewBag.Title = "Home";
            return View();
        }
        public IActionResult Name()
        {
            ViewBag.Title = "Home";
            return View();
        }
    }
}
