using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class AuthController : Controller
    {
        #region Sign Up

        public IActionResult SignUp()
        {
            ViewBag.Title = "Sign Up";
            return View();
        }
        #endregion

        #region Sign In
        public IActionResult SignIn()
        {
            ViewBag.Title = "Sign In";
            return View();
        }

        #endregion

        #region Sign Out
        public new IActionResult SignOut()
        {
            return View();
        }
        #endregion
    }
}
