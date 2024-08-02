using Azure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;

namespace Assignment.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            bool IsAuthenticated = false;
            bool IsAdmin = false;
            ClaimsIdentity identity = null;
            if(email == "admin123@gmail.com" && password == "123")
            {
                identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name,"admin1"),
                    new Claim(ClaimTypes.Role,"admin"),

                }
                ,CookieAuthenticationDefaults.AuthenticationScheme);
                IsAuthenticated = true;
                IsAdmin = true;

            }
            else if(email == "user123@gmail.com" && password == "123")
            {
                identity = new ClaimsIdentity(new[]
               {
                    new Claim(ClaimTypes.Name,"user1"),
                    new Claim(ClaimTypes.Role,"user"),

                }
               , CookieAuthenticationDefaults.AuthenticationScheme);
                IsAuthenticated = true;
                IsAdmin = false;

            }
            

            if (IsAuthenticated && IsAdmin)
            {
                var principal = new ClaimsPrincipal(identity);
                var login = HttpContext.SignInAsync
                    (CookieAuthenticationDefaults.AuthenticationScheme,principal);
                return RedirectToAction("Index", "Admin");
            }
            else if (IsAuthenticated){
                var principal = new ClaimsPrincipal(identity);
                var login = HttpContext.SignInAsync
                    (CookieAuthenticationDefaults.AuthenticationScheme, principal);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }

        } 
            public IActionResult Logout()
            {

                var login = HttpContext.SignOutAsync
                    (CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Login", "Auth");
            }
    }
}
