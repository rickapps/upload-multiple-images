using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RickApps.UploadFilesMVC.Models;
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

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password, string ReturnUrl)
        {
            if ((username == _admin.UserName) && (password == _admin.PassWord))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username)
                };
                var claimsIdentity = new ClaimsIdentity(claims, "Login");

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                return Redirect(ReturnUrl == null ? "/Admin" : ReturnUrl);
            }
            else
            {
                ModelState.AddModelError("", "Incorrect username or password");
                return View();
            }
        }

    }
}
