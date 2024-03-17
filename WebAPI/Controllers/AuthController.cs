using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens; // jwt kütüphanesi
using Service;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IService<User> _userService;
        private readonly IConfiguration _configuration;

        public AuthController(IService<User> userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync(LoginViewModel loginViewModel)
        {
            var account = await _userService.GetAsync(user => user.Email == loginViewModel.Email && user.Password == loginViewModel.Password && user.IsActive);
            if (account == null)
            {
                return NotFound();
            }
            // Security keyin simetriğini alıyoru
            SymmetricSecurityKey symmetricSecurity = new(Encoding.UTF8.GetBytes(_configuration["Token:SecurityKey"]));

            // Şifrelenmiş kimliği oluşturuyoruz
            SigningCredentials signingCredentials = new(symmetricSecurity, SecurityAlgorithms.HmacSha256);

            // Eğer rol sistemi kullanacaksak
            var claims = new List<Claim>()
            {
                new(ClaimTypes.Name, account.Name),
                new(ClaimTypes.Email, account.Email),
                new(ClaimTypes.Role, account.IsAdmin ? "Admin" : "User"),
                new("UserId", account.Id.ToString())
            };
            // Giriş yapan kullanıcıya içini doldurup göndereceğimiz token nesnesi
            Token token = new Token();
            // Oluşturacağımız token ayarlarını yapıyoruz
            token.Expiration = DateTime.Now.AddMinutes(30); // burada dk, saat gün vb süreyi verebiliriz.
            JwtSecurityToken securityToken = new JwtSecurityToken(
                issuer: _configuration["Token:Issuer"],
                audience: _configuration["Token:Audience"],
                expires: DateTime.Now.AddMinutes(10),
                notBefore: DateTime.Now,
                signingCredentials: signingCredentials,
                claims: claims
                );
            // Token oluştururcu sınıfından örnek oluştuuyoruz
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            // Token üretiyoruz
            token.AccessToken = tokenHandler.WriteToken(securityToken); // bizim token nesnesine jwt nin ürettiği token ı atıyoruz

            // Refresh token üretiyoruz
            token.RefreshToken = Guid.NewGuid().ToString();
            // token UserGuid i giriş yapan kullanıcınınkiyle eşitliyoruz
            token.UserGuid = account.UserGuid;
            token.IsAdmin = account.IsAdmin; // kullanıcı admin mi bilgisini setledik.

            // refresh token ile sonra işlem yapabilmek için kullanıcıyı güncelliyoruz
            account.RefreshToken = token.RefreshToken;
            account.RefreshTokenExpireDate = token.Expiration.AddMinutes(30);
            _userService.Update(account);
            await _userService.SaveAsync();

            return Ok(token); // işimiz bittiğinde oluşan token ı kullanıcıya dönüyoruz
        }
        [HttpPost("CreateAppUser")]
        public async Task<ActionResult<User>> CreateAppUser(User user) // Uygulamaya dışarıdan kaydolunabilmeyi sağlayan metot
        {
            try
            {
                var kullanici = await _userService.GetAsync(x => x.Email == user.Email);
                if (kullanici != null)
                {
                    return Conflict(new { errMes = user.Email + " ile daha önce sisteme kayıt yapılmış!" }); // Conflict geriye çakışma mesajı döndüren metot.
                }
                else
                {
                    user.IsActive = true;
                    user.IsAdmin = false; // yeni üye olan kullanıcı kesinlikle admin olmasın, sonradan panelden biz istersek adminlik verebiliriz.
                    await _userService.AddAsync(user);
                    await _userService.SaveAsync();
                    // burada yazdığımız mail servisi ile kaydolan kullanıcıya userguid değerini querystring olarak ekleyip mail gönderip üyeliğini aktif etmek için gönderilen maile tıklayın sistemi yapılabilir. Kullanıcı maildeki linke tıkladığında bunu kontrol eden bir ekran yapılır, orada adres çubuğundaki guid değer alınıp db de sorgulanır kayıt varsa bu kullanıcının isactive değeri true yapılır ve ancak böyle uygulamaya giriş yapması sağlanabilir.
                    return Ok(user); // ok metodu işlem başarılı mesajı döner ve içinde json olarak oluşan kullanıcıyı geri götürebilir.
                }
            }
            catch (Exception)
            {
                return Problem("Hata Oluştu!"); // eğer hata oluşursa
            }
        }
        [HttpGet("GetUserByUserGuid/{id}")]
        public async Task<ActionResult<User>> GetUserByUserGuid(string id)
        {
            // metoda gönderilecek kullanıcı guid değerine göre db de arama yapan metot
            var user = await _userService.GetAsync(x => x.UserGuid == id);
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }
    }
}
