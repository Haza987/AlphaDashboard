using Business.Dtos;
using Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class AuthController(UserService userService) : Controller
    {
        private readonly UserService _userService = userService;

        [HttpGet]
        public IActionResult SignIn()
        {
            ViewBag.Title = "Sign In";
            return View(new SignInViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel form, string returnUrl = "~/")
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.SignInAsync(form.Email, form.Password);
                if (result.Succeeded)
                {
                    return LocalRedirect(returnUrl);
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            return View(form);
        }


        [Authorize]
        public new async Task<IActionResult> SignOut()
        {
            await _userService.SignOutAsync();
            return RedirectToAction("Homepage", "Home");
        }
    }
}
