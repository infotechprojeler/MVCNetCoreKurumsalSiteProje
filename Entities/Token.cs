namespace Entities
{
    public class Token
    {
        public string AccessToken { get; set; } // erişim işlemini sağlayacak ana token
        public DateTime Expiration { get; set; } // Token süresini belirlememizi sağlayan prop
        public string RefreshToken { get; set; } // Ana token süresi bittiğinde token ı yenileyebilmemizi sağlayacak prop
        public string? UserGuid { get; set; } // Kullanıcıyı bulmamız için kullanabileceğimiz prop
        public bool IsAdmin { get; set; } // kullanıcı admin mi bilgisini taşıyabilmemizi sağlayan prop
    }
}
