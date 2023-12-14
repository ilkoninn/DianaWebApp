namespace DianaWebApp.Helper
{
    public static class FileManager
    {
        public static bool CheckType(this IFormFile Img, string type)
        {
            return Img.ContentType.Contains(type);
        }
        public static bool CheckLength(this IFormFile Img, int length)
        {
            return Img.Length <= length;
        }
        public static string Upload(this IFormFile Img, string web, string folderName)
        {
            if (!Directory.Exists(web + folderName))
            {
                Directory.CreateDirectory(web + folderName);
            }

            string fileName = Img.FileName;

            if (fileName.Length > 64)
            {
                fileName = fileName.Substring(fileName.Length - 64);
            }

            fileName = Guid.NewGuid().ToString() + fileName;

            string path = web + folderName + fileName;
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                Img.CopyTo(stream);
            }
            return fileName;
        }

        public static void Delete(this string ImgUrl, string web, string folderName)
        {
            string path = web + folderName + ImgUrl;

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
