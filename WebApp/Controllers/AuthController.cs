using Business.Dtos;
using Business.Interfaces;
using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class AuthController(IUserService userService, UserManager<UserEntity> userManager) : Controller
    {
        private readonly IUserService _userService = userService;
        private readonly UserManager<UserEntity> _userManager = userManager;

        [HttpGet]
        public IActionResult SignIn()
        {
            ViewBag.Title = "Sign In";
            ViewBag.FormAction = "SignIn";
            return View(new SignInViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel form, string returnUrl = "/projects")
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

        [HttpGet]
        public IActionResult AdminSignIn()
        {
            ViewBag.Title = "Admin sign in";
            ViewBag.FormAction = "AdminSignIn";
            return View(new SignInViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> AdminSignIn(SignInViewModel form, string returnUrl = "/members")
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.SignInAsync(form.Email, form.Password);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(form.Email);

                    var isAdmin = await _userManager.IsInRoleAsync(user!, "Admin");
                    if (isAdmin)
                    {
                        return LocalRedirect(returnUrl); 
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "You are not authorized to access this page.");
                        await _userService.SignOutAsync();
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
            }
            return View(form);
        }


        [Authorize]
        [Route("SignOut")]
        public new async Task<IActionResult> SignOut()
        {
            await _userService.SignOutAsync();
            return RedirectToAction("Homepage", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
