using BTL_TKWeb.ModelView;
using Microsoft.EntityFrameworkCore;

namespace BTL_TKWeb.Helper
{
    public class Tool
    {
        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static void UploadAnh(IFormFile? Anh,string filename)
        {
            var upload = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "hang");
            var filepath = Path.Combine(upload, filename);
            FileStream temp = new FileStream(filepath, FileMode.Create);
            Anh.CopyTo(temp);
            temp.Close();
        }
        public static void DeleteAnh(string tenanh)
        {
            var upload = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "hang");
            System.IO.File.Delete(Path.Combine(upload,tenanh) );
        }
    }
}
