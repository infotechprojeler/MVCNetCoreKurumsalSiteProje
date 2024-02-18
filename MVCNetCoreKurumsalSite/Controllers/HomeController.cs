using Data;
using Microsoft.AspNetCore.Mvc;
using MVCNetCoreKurumsalSiteProje.Models;
using System.Diagnostics;

namespace MVCNetCoreKurumsalSiteProje.Controllers
{
    public class HomeController : Controller
    {
        private readonly DatabaseContext _context;

        public HomeController(DatabaseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var model = new HomePageViewModel()
            {
                Slides = _context.Slides.ToList(),
                Categories = _context.Categories.ToList(),
                Posts = _context.Posts.Where(p => p.IsActive && p.IsHome).ToList()
            };
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
