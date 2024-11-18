using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RemindWebApp.Extension_GenerateImage
{
    public static  class IFormFileExtension
    {
        public static bool IsImage(this IFormFile formFile)
        {
            return formFile.ContentType.Contains(@"image/");
        }

        public static bool CheckSize(this IFormFile formFile, int maxsize)
        {
            return formFile.Length / 1024 / 1024 < maxsize;
        }

        public async static Task<string> CopyImage(this IFormFile formFile, string root, string folder)
        {
            try
            {
                string path = Path.Combine(root, "img");
                string filename = Path.Combine(folder, Guid.NewGuid().ToString() + formFile.FileName);
                string resultPath = Path.Combine(path, filename);


                using (FileStream fileStream = new FileStream(resultPath, FileMode.Create))
                {
                    await formFile.CopyToAsync(fileStream);
                }
                string replaced_filename = filename.Replace(@"\", "/");
                return replaced_filename;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
    }
}
