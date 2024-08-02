using Assignment.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Assignment.Controllers
{
    public class HomeController : Controller
    {
        private readonly AssignmentdotnetContext db;
        public HomeController(AssignmentdotnetContext _db)
        {
            db = _db;
        }
        [Authorize (Roles="user")]
        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        public IActionResult About()
        {
            return View();
        }
        [Authorize]
        public IActionResult Events()
        {
            return View();
        }
        [Authorize]
        public IActionResult Chefs()
        {
            return View();
        }
        [Authorize]
        public IActionResult Gallery()
        {
            return View();
        }
        [Authorize]
        public IActionResult Contact()
        {
            return View();
        }
        [Authorize]

        public IActionResult Product()
        {
            var itemdata = db.Items.Include(it => it.Cat);
            return View(itemdata.ToList());
        }
    }
}
