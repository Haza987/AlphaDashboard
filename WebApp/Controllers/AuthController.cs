using Business.Dtos;
using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class AuthController(IUserService userService) : Controller
    {
        private readonly IUserService _userService = userService;

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

        [HttpGet]
        public IActionResult SignUp()
        {
            ViewBag.Title = "Create Account";
            return View(new SignUpViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel form)
        {
            if (ModelState.IsValid)
            {
                var user = new UserDto
                {
                    FirstName = form.FirstName,
                    LastName = form.LastName,
                    Email = form.Email,
                    Password = form.Password,
                    Role = "User"
                };

                var result = await _userService.CreateUserAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("SignIn");
                }
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
