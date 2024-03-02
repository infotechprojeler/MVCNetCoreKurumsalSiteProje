using Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebApiUsing.Controllers
{
    public class PostsController : Controller
    {
        private readonly HttpClient _httpClient; // veritabanı contexti yerine apiye bağlanma nesnesi
                                                 // Not : _httpClient nesnesi apiye istek atarken cors engeline takılmaz.
        public PostsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        string _apiAdres = "https://localhost:7125/api/";

        public async Task<IActionResult> Index()
        {
            var model = await _httpClient.GetFromJsonAsync<List<Post>>(_apiAdres + "Posts"); // GetFromJsonAsync metodu _httpClient nesnesinin bir metodudur ve apiadresteki yere istek atar, oradan gelen json türündeki post listesini <List<Post>> yapısıyla mvc deki model nesnesine çevirir.
            return View(model);
        }
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var model = await _httpClient.GetFromJsonAsync<Post>(_apiAdres + "Posts/" + id);
            if (model == null)
            {
                return NotFound();
            }
            var kategori = await _httpClient.GetFromJsonAsync<Category>(_apiAdres + "Categories/" + model.CategoryId);
            if (kategori != null)
            {
                model.Category = kategori;
            }
            return View(model);
        }
    }
}
