using Business.Dtos;
using Business.Interfaces;
using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class AuthController(IUserService userService, UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager) : Controller
    {
        private readonly IUserService _userService = userService;
        private readonly UserManager<UserEntity> _userManager = userManager;
        private readonly SignInManager<UserEntity> _signInManager = signInManager;

        #region Local authentication

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
                    var user = await _userManager.FindByEmailAsync(form.Email);
                    var isAdmin = await _userManager.IsInRoleAsync(user!, "Admin");

                    if (isAdmin)
                    {
                        return RedirectToAction("AdminSignIn", "Auth");
                    }

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

        #endregion

        #region External authentication

        [HttpPost]
        public IActionResult ExternalLogin(string provider, string returnUrl = "/projects")
        {
            if (string.IsNullOrEmpty(provider))
            {
                ModelState.AddModelError("", "Invalid provider.");
                return RedirectToAction("SignIn");
            }

            var redirectUrl = Url.Action("ExternalLoginCallback", "Auth", new { returnUrl })!;
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return Challenge(properties, provider);
        }

        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = "/projects", string remoteError = null!)
        {
            returnUrl ??= Url.Content("~/");

            if (!string.IsNullOrEmpty(remoteError))
            {
                ModelState.AddModelError("", $"Error from external provider: {remoteError}");
                return View("SignIn");
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return View("SignIn");
            }

            var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (signInResult.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                string firstName = string.Empty;
                string lastName = string.Empty;

                try
                {
                    firstName = info.Principal.FindFirstValue(ClaimTypes.GivenName)!;
                    lastName = info.Principal.FindFirstValue(ClaimTypes.Surname)!;
                }
                catch { }

                string email = info.Principal.FindFirstValue(ClaimTypes.Email)!;
                string username = $"{info.LoginProvider.ToLower()}_{email}";

                var user = new UserEntity
                {
                    UserName = username,
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName,
                    Role = "User"
                };

                var identityResult = await _userManager.CreateAsync(user);
                if (identityResult.Succeeded)
                {
                    await _userManager.AddLoginAsync(user, info);
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return LocalRedirect(returnUrl);
                }

                return View("SignIn");
            }
        }

        #endregion
    }
}
