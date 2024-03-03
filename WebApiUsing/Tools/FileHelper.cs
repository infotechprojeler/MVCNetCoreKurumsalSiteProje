using System.IO;

namespace WebApiUsing.Tools
{
    public class FileHelper
    {
        public static async Task<string> FileLoaderAsync(IFormFile? formFile, string klasorYolu = "/wwwroot/Images/")
        {
            string dosyaAdi = "";

            if (formFile is not null)
            {
                dosyaAdi = formFile.FileName;
                string dizin = Directory.GetCurrentDirectory() + klasorYolu + dosyaAdi;
                using var stream = new FileStream(dizin, FileMode.Create);
                await formFile.CopyToAsync(stream);
            }

            return dosyaAdi;
        }
        public static bool FileRemover(string fileName, string filePath = "/wwwroot/Images/")
        {
            string directory = Directory.GetCurrentDirectory() + filePath + fileName; // dosyayı sileceğimiz konum
            if (Directory.Exists(directory)) // eğer belirtilen dizinde bir dosya varsa
            {
                File.Delete(directory); // verilen dizindeki dosyayı sil
                return true; // işlem sonucunun başarılı olduğunu dönüyoruz
            }
            return false; // buraya geldiyse dosya silinememiştir
        }
        public static async Task<MultipartFormDataContent> ApiFileSenderAsync(IFormFile? formFile)
        {
            MultipartFormDataContent formData = new MultipartFormDataContent(); // apiye dosya yollayabilmemizi sağlayan nesne oluşturduk
            if (formFile != null && formFile.Length > 0) // apiye gönderilmek istenen dosyayı kontrol ettik
            {
                var stream = new MemoryStream(); // pc den servere doğru bir akış nesnesi oluşturduk
                await formFile.CopyToAsync(stream); // gönderilen dosyayı kopyaladık

                var bytes = stream.ToArray(); // içine yükleme yapılan akışı diziye çevirdik
                ByteArrayContent content = new ByteArrayContent(bytes);// diziye çevirdiğimiz nesneyi byte lar haline getirdik 
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(formFile.ContentType); // content nesnesinin http başlık istek kısmına yüklemek istediğimiz dosya tipini ekledik
                formData.Add(content, "formFile", formFile.FileName); // sunucuya yüklemek istediğimiz byte veri tipini content nesnesine ekledik, yüklenen dosyaya formFile ismini verdik ve dosya adını ekledik
            }
            return formData; // content nesnemizi api isteğinde kullanılmak üzere metodun çağrıldığı yere geri gönderdik
        }
    }
}
