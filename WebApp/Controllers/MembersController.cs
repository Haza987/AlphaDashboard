using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class MembersController : Controller
    {
        public IActionResult Members()
        {
            ViewBag.Title = "Members";
            return View();
        }
    }
}
