using IdentityServerAds.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServerAds.Controllers
{
    public class AuthController : Controller
    {
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


    }
}
