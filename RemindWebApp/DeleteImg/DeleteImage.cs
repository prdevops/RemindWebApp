using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RemindWebApp.DeleteImg
{
    public static class DeleteImage
    {
        public static bool DeleteFromFolder(string root, string image)
        {
            string path = Path.Combine(root, "img", image);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
                return true;
            }
            return false;
        }
    }
}
