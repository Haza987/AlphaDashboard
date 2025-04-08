using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class ProjectsController : Controller
    {
        public IActionResult Projects()
        {
            return View();
        }
    }
}
