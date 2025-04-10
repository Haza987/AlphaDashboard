using Business.Dtos;
using Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly MemberService _memberService;

        public AuthController(MemberService memberService)
        {
            _memberService = memberService;
        }

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
                var result = await _memberService.SignInAsync(form.Email, form.Password);
                if (result.Succeeded)
                {
                    return LocalRedirect(returnUrl);
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            return View(form);
        }

        [HttpPost]
        public async Task<IActionResult> SignOut()
        {
            await _memberService.LogoutAsync();
            return RedirectToAction("Homepage", "Home");
        }
    }
}
