using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class MembersController : Controller
    {
        [Route("members")]
        public IActionResult Members()
        {
            ViewBag.Title = "Members";
            return View();
        }
    }
}
