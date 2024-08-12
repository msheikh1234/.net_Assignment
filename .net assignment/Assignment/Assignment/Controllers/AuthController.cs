using Assignment.Models;
using Azure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;

namespace Assignment.Controllers
{
    public class AuthController : Controller
    {
        private readonly AssignmentdotnetContext db;
        public AuthController( AssignmentdotnetContext _db)
        {
            db = _db;
        }
        public IActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Signup(User user)
        {
            var checkUser = db.Users.FirstOrDefault(u => u.Email == user.Email);
            if(checkUser == null)
            {
                var hasher = new PasswordHasher<string>();
                var hashpassword = hasher.HashPassword(user.Email, user.Password);
                user.Password = hashpassword;
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Login");

            }
            else
            {
                ViewBag.msg = "User already registered. Please Login.";
            return View();
            }
        }

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
