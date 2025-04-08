using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class MembersController : Controller
    {
        public IActionResult Members()
        {
            return View();
        }
    }
}
