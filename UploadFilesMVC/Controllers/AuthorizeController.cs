using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RickApps.UploadFilesMVC.Models;
using RickApps.UploadFilesMVC.ViewModels;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RickApps.UploadFilesMVC.Controllers
{
    public class AuthorizeController : Controller
    {
        Credentials _admin;
        public AuthorizeController(IOptions<Credentials> admin)
        {
            _admin = admin.Value;
        }

        public IActionResult Login(string returnUrl = "/Admin")
        {
            // Note that returnUrl will never be null
            AuthorizeLoginViewModel vm = new AuthorizeLoginViewModel();
            vm.ReturnUrl = returnUrl;
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Login(AuthorizeLoginViewModel vm)
        {
            if (ModelState.IsValid)
            {
                if ((vm.UserName == _admin.UserName) && (vm.Password == _admin.PassWord))
                {
                    var claims = new List<Claim>
                    {
                        // You can add all kinds of crap here to set up roles and stuff...
                        new Claim(ClaimTypes.Name, vm.UserName)
                    };
                    var claimsIdentity = new ClaimsIdentity(claims, "Login");

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                    return Redirect(vm.ReturnUrl);
                }
                else
                {
                    ModelState.AddModelError("", "Incorrect username or password");
                }
            }
            return View(vm);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}
