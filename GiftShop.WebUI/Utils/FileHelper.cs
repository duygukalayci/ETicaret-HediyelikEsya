using System.Collections.Specialized;

namespace GiftShop.WebUI.Utils
{
    public class FileHelper
    {
        public static async Task<string> UploadFileAsync(IFormFile FormFile, string filePath = "/Img/")
        {
            if (FormFile == null || FormFile.Length == 0)
                return "";

            string extension = Path.GetExtension(FormFile.FileName);
            string fileName = Guid.NewGuid().ToString() + extension;

            string directory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", filePath.Trim('/'), fileName);

            using var stream = new FileStream(directory, FileMode.Create);
            await FormFile.CopyToAsync(stream);

            return fileName;
        }


        public static bool FileRemover(string FileName, string filePath = "/Img/")
        {

            string directory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", filePath.Trim('/'), FileName);
            if (File.Exists(directory))
            {
                File.Delete(directory);
                return true;
            }
            return false;
        }
    }
}
