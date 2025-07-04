using System.Collections.Specialized;

namespace GiftShop.WebUI.Utils
{
    public class FileHelper
    {
        public static async Task<string> UploadFileAsync(IFormFile FormFile, string filePath="/Img/")
        {

            string fileName = "";
            if(FormFile!=null&&FormFile.Length>0)
            {
                fileName = FormFile.FileName.ToLower();
                string directory=Directory.GetCurrentDirectory() + "/wwwroot"+filePath+fileName;
                using var stream = new FileStream(directory, FileMode.Create);
                {
                    await FormFile.CopyToAsync(stream);
                } 

            }
            return fileName;

        }

        public static bool FileRemover(string FileName, string filePath = "/Img/")
        {
            
            string directory = Directory.GetCurrentDirectory() + "/wwwroot" + filePath + FileName;
            if (File.Exists(directory))
            {
                File.Delete(directory);
                return true;
            }
            return false;
        }
    }
}
