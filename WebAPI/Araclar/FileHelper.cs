namespace WebAPI.Araclar
{
    public class FileHelper
    {
        public static async Task<string> FileLoaderAsync(IFormFile? formFile, string klasorYolu = "")
        {
            string dosyaAdi = "";

            if (formFile is not null)
            {
                dosyaAdi = formFile.FileName;
                string dizin = Directory.GetCurrentDirectory() + "\\wwwroot\\Images\\" + klasorYolu + dosyaAdi;
                using var stream = new FileStream(dizin, FileMode.Create);
                await formFile.CopyToAsync(stream);
            }

            return dosyaAdi;
        }
    }
}
