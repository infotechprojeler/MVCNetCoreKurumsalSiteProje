using Microsoft.AspNetCore.Mvc;

namespace WebApiUsing.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult JqueryCrud()
        {
            return View();
        }
    }
}
