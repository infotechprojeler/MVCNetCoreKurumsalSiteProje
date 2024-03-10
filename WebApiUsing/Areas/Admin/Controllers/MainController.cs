using Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApiUsing.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class MainController : Controller
    {
        private readonly HttpClient _httpClient;
        string _apiAdres = "https://localhost:7125/api/Users/";
        public MainController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public IActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [AllowAnonymous, HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            try
            {
                var kullanicilar = await _httpClient.GetFromJsonAsync<List<User>>(_apiAdres);

                var kullanici = kullanicilar.FirstOrDefault(u => u.Email == email && u.Password == password && u.IsActive);
                if (kullanici == null)
                {
                    TempData["Message"] = "<p class='alert alert-danger mt-3'>Giriş Başarısız!</p>";
                }
                else
                {
                    var haklar = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Email, kullanici.Email),
                        new Claim(ClaimTypes.Name, kullanici.Name),
                        new Claim(ClaimTypes.Role, kullanici.IsAdmin ? "Admin" : "Person")
                    };
                    var kullaniciKimligi = new ClaimsIdentity(haklar, "Login");
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(kullaniciKimligi);
                    await HttpContext.SignInAsync(claimsPrincipal);
                    string donusAdresi = HttpContext?.Request?.Query?["ReturnUrl"];
                    if (!string.IsNullOrEmpty(donusAdresi))
                    {
                        return Redirect(donusAdresi);
                    }
                    return Redirect("/Admin");
                }
            }
            catch (Exception hata)
            {
                TempData["Message"] = hata?.InnerException?.Message;
            }
            return View();
        }
        public async Task<IActionResult> LogoutAsync()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index");
        }
    }
}
