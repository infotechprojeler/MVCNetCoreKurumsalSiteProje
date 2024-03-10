using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApiUsing.Tools;

namespace WebApiUsing.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class ProductsController : Controller
    {
        private readonly HttpClient _httpClient;

        public ProductsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        string _apiAdres = "https://localhost:7125/api/";
        // GET: ProductsController
        public async Task<ActionResult> Index()
        {
            var model = await _httpClient.GetFromJsonAsync<List<Product>>(_apiAdres + "Products");
            return View(model);
        }

        // GET: ProductsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProductsController/Create
        public async Task<ActionResult> CreateAsync()
        {
            var kategoriler = await _httpClient.GetFromJsonAsync<List<Category>>(_apiAdres + "Categories");
            ViewData["CategoryId"] = new SelectList(kategoriler, "Id", "Name");
            return View();
        }

        // POST: ProductsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(Product collection, IFormFile? Image)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // apideki klasöre resim yükleme
                    var formDataResponse = await _httpClient.PostAsync(_apiAdres + "Upload", await FileHelper.ApiFileSenderAsync(Image));

                    // bu projedeki klasöre resim yükleme
                    collection.Image = await FileHelper.FileLoaderAsync(Image);

                    var response = await _httpClient.PostAsJsonAsync(_apiAdres + "Products", collection);
                    if (response.IsSuccessStatusCode) // eğer apiye yapılan istek işlem başarılı cevabı döndürdüyse
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    ModelState.AddModelError("", "Kayıt Başarısız!");
                }
                catch
                {
                    ModelState.AddModelError("", "Hata Oluştu!");
                }
            }
            var kategoriler = await _httpClient.GetFromJsonAsync<List<Category>>(_apiAdres + "Categories");
            ViewData["CategoryId"] = new SelectList(kategoriler, "Id", "Name");
            return View(collection);
        }

        // GET: ProductsController/Edit/5
        public async Task<ActionResult> EditAsync(int id)
        {
            var kategoriler = await _httpClient.GetFromJsonAsync<List<Category>>(_apiAdres + "Categories");
            ViewData["CategoryId"] = new SelectList(kategoriler, "Id", "Name");

            var model = await _httpClient.GetFromJsonAsync<Product>(_apiAdres + "Products/" + id);
            return View(model);
        }

        // POST: ProductsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Product collection, IFormFile? Image)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (Image is not null)
                    {
                        // apideki klasöre resim yükleme
                        var formDataResponse = await _httpClient.PostAsync(_apiAdres + "Upload", await FileHelper.ApiFileSenderAsync(Image));

                        collection.Image = await FileHelper.FileLoaderAsync(Image);
                    }
                    var response = await _httpClient.PutAsJsonAsync(_apiAdres + "Products/" + id, collection);
                    if (response.IsSuccessStatusCode) // eğer apiye yapılan istek işlem başarılı cevabı döndürdüyse
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    ModelState.AddModelError("", "Kayıt Başarısız!");
                }
                catch
                {
                    ModelState.AddModelError("", "Hata Oluştu!");
                }
            }
            var kategoriler = await _httpClient.GetFromJsonAsync<List<Category>>(_apiAdres + "Categories");
            ViewData["CategoryId"] = new SelectList(kategoriler, "Id", "Name");
            return View(collection);
        }

        // GET: ProductsController/Delete/5
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var model = await _httpClient.GetFromJsonAsync<Product>(_apiAdres + "Products/" + id);
            return View(model);
        }

        // POST: ProductsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAsync(int id, Product collection)
        {
            try
            {
                var response = await _httpClient.DeleteAsync(_apiAdres + "Products/" + id);
                if (response.IsSuccessStatusCode)
                    return RedirectToAction(nameof(Index));
                ModelState.AddModelError("", "Kayıt Silme Başarısız!");
            }
            catch
            {
                ModelState.AddModelError("", "Hata Oluştu!");
            }
            return View(collection);
        }
    }
}
