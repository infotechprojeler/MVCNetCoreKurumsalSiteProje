using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApiUsing.Tools;

namespace WebApiUsing.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PostsController : Controller
    {
        private readonly HttpClient _httpClient;

        public PostsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        string _apiAdres = "https://localhost:7125/api/";
        // GET: PostsController
        public async Task<ActionResult> IndexAsync()
        {
            var model = await _httpClient.GetFromJsonAsync<List<Post>>(_apiAdres + "Posts");
            return View(model);
        }

        // GET: PostsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PostsController/Create
        public async Task<ActionResult> CreateAsync()
        {
            var kategoriler = await _httpClient.GetFromJsonAsync<List<Category>>(_apiAdres + "Categories");
            ViewData["CategoryId"] = new SelectList(kategoriler, "Id", "Name");
            return View();
        }

        // POST: PostsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(Post collection, IFormFile? Image)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    collection.Image = await FileHelper.FileLoaderAsync(Image);
                    var response = await _httpClient.PostAsJsonAsync(_apiAdres + "Posts", collection);
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

        // GET: PostsController/Edit/5
        public async Task<ActionResult> EditAsync(int id)
        {
            var kategoriler = await _httpClient.GetFromJsonAsync<List<Category>>(_apiAdres + "Categories");
            ViewData["CategoryId"] = new SelectList(kategoriler, "Id", "Name");

            var model = await _httpClient.GetFromJsonAsync<Post>(_apiAdres + "Posts/" + id);
            return View(model);
        }

        // POST: PostsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Post collection, IFormFile? Image)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    collection.Image = await FileHelper.FileLoaderAsync(Image);
                    var response = await _httpClient.PutAsJsonAsync(_apiAdres + "Posts/" + id, collection);
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

        // GET: PostsController/Delete/5
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var model = await _httpClient.GetFromJsonAsync<Post>(_apiAdres + "Posts/" + id);
            return View(model);
        }

        // POST: PostsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAsync(int id, Post collection)
        {
            try
            {
                var response = await _httpClient.DeleteAsync(_apiAdres + "Posts/" + id);
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
