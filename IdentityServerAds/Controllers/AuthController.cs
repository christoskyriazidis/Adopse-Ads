using IdentityServer4.Services;
using IdentityServerAds.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServerAds.Controllers
{
    public class AuthController : Controller
    {

        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IIdentityServerInteractionService _interactionService;
        private readonly IConfiguration _config;

        public AuthController(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IIdentityServerInteractionService interactionService,
            IConfiguration config
            )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _interactionService = interactionService;
            _config = config;
        }


        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            //var externalProvidres = await _signInManager.GetExternalAuthenticationSchemesAsync();
            //vazoume kai tous external sto get
            return View(new LoginViewModel { ReturnUrl = returnUrl});
        }

        [HttpGet]
        public IActionResult Register(string returnUrl)
        {
            return View(new RegisterViewModel { ReturnUrl = returnUrl });
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel lvm)
        {
            if (!ModelState.IsValid)
            {
                return View(lvm);
            }
            //vlepoume an einai valid to request (model)
            if (lvm.Password == null || lvm.Username == null)
            {
                ViewBag.Message = "You must fill the form dumb kid!";
                return View(lvm);
            }

            //koitaw an uparxei to email PRWTA
            var user = await _userManager.FindByEmailAsync(lvm.Username);
            if (user == null)
            {
                user = await _userManager.FindByNameAsync(lvm.Username);
                if (user == null)
                {
                    ViewBag.Message = $"Wrong username or password (den uparxei o xrhsths/email gia ekpedeutikous logous oxi asfaleia)";
                    return View(lvm);
                }
            }
            //var emailConfirmation = await _userManager.IsEmailConfirmedAsync(user);
            //if (!emailConfirmation)
            //{
            //    ViewBag.Message = "Email not verified";
            //    return View(lvm);
            //}
            var loginResult = await _signInManager.PasswordSignInAsync(user, lvm.Password, false, true);
            if (loginResult.IsLockedOut)
            {
                ViewBag.Message = $"You have been lockedOut wait 1 minute. ";
                return View(lvm);
            }
            else if (loginResult.Succeeded)
            {
                return Redirect(lvm.ReturnUrl);
            }
            var failedTimes = await _userManager.GetAccessFailedCountAsync(user);
            ViewBag.Message = $"Wrong password.Sou menoun akoma {4 - failedTimes} prospa8ies";
            return View(lvm);
        }


        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            await _signInManager.SignOutAsync();

            var logoutRequest = await _interactionService.GetLogoutContextAsync(logoutId);

            if (string.IsNullOrEmpty(logoutRequest.PostLogoutRedirectUri))
            {
                return RedirectToAction("Index", "Home");
            }

            return Redirect(logoutRequest.PostLogoutRedirectUri);
        }

    }
}
