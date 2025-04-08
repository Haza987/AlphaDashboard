using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Homepage()
        {
            ViewBag.Title = "Home";
            return View();
        }
    }
}
