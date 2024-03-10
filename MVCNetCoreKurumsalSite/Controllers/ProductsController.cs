using Microsoft.AspNetCore.Mvc;
using Service;

namespace MVCNetCoreKurumsalSiteProje.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ICategoryService _categoryService;

        public ProductsController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public IActionResult Index(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }
            var kategori = _categoryService.GetCategoryByProducts(id.Value);
            if (kategori is null)
            {
                return NotFound();
            }
            return View(kategori);
        }
    }
}
