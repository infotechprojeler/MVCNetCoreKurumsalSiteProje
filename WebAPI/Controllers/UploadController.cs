using Microsoft.AspNetCore.Mvc;
using WebAPI.Araclar;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        // dosya yükleme işini merkezi bir controller da toplama
        [HttpPost]
        public async Task<IActionResult> Upload([FromForm] IFormFile? formFile, string path = "")
        {
            var result = await FileHelper.FileLoaderAsync(formFile, path);
            if (string.IsNullOrEmpty(result))
            {
                return Problem("Dosya Yüklenemedi!"); // IActionResult türlerinden biri olan problem ile geriye bir problem oluştuğu bilgisini dönüyoruz.
            }
            return Ok(result);
        }
    }
}
