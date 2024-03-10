using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiUsing.Tools;

namespace WebApiUsing.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class SlidesController : Controller
    {
        private readonly HttpClient _httpClient;

        public SlidesController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        string _apiAdres = "https://localhost:7125/api/Slides/";
        string _apiAdres2 = "https://localhost:7125/api/Upload";
        // GET: SlidesController
        public async Task<ActionResult> Index()
        {
            var model = await _httpClient.GetFromJsonAsync<List<Slide>>(_apiAdres);
            return View(model);
        }

        // GET: SlidesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SlidesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SlidesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(Slide collection, IFormFile? Image)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // bu projedeki klasöre resim yükleme
                    // collection.Image = await FileHelper.FileLoaderAsync(Image);

                    // api sunucusundaki klasöre resim yükleme

                    // var formDataresponse = await _httpClient.PostAsync(_apiAdres2 + "?path=", await FileHelper.ApiFileSenderAsync(Image));
                    // api deki upload controller a post isteği yapıyoruz ve dosya olarak da yazdığımız ApiFileSenderAsync metodundan gelen nesneyi yolluyoruz
                    /* sorun çözümü için yazdığımız
                     * MultipartFormDataContent formData = new MultipartFormDataContent();

                    var stream = new MemoryStream();
                    await Image.CopyToAsync(stream);

                    var bytes = stream.ToArray();

                    ByteArrayContent content = new ByteArrayContent(bytes);
                    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(Image.ContentType);
                    
                    formData.Add(content, "formFile", Image.FileName);*/
                    var formDataResponse = await _httpClient.PostAsync(_apiAdres2, await FileHelper.ApiFileSenderAsync(Image));

                    collection.Image = await FileHelper.FileLoaderAsync(Image);

                    var response = await _httpClient.PostAsJsonAsync(_apiAdres, collection);
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
            return View(collection);
        }

        // GET: SlidesController/Edit/5
        public async Task<ActionResult> EditAsync(int id)
        {
            var model = await _httpClient.GetFromJsonAsync<Slide>(_apiAdres + id);
            return View(model);
        }

        // POST: SlidesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(int id, Slide collection, IFormFile? Image)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (Image is not null)
                        collection.Image = await FileHelper.FileLoaderAsync(Image);
                    var response = await _httpClient.PutAsJsonAsync(_apiAdres + id, collection);
                    if (response.IsSuccessStatusCode)
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
            return View(collection);
        }

        // GET: SlidesController/Delete/5
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var model = await _httpClient.GetFromJsonAsync<Slide>(_apiAdres + id);
            return View(model);
        }

        // POST: SlidesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAsync(int id, Slide collection)
        {
            try
            {
                var response = await _httpClient.DeleteAsync(_apiAdres + id);
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
