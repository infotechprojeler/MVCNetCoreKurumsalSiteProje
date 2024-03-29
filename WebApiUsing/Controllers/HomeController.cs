using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApiUsing.Models;

namespace WebApiUsing.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detail(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }
            return View();
        }
        
        public IActionResult Posts()
        {
            return View();
        }

        public IActionResult PostDetail(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }
            return View();
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
