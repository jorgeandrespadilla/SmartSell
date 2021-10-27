using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

// https://www.c-sharpcorner.com/article/upload-files-in-Asp-Net-mvc/

namespace ProyectoFinal.Web.Helpers
{
    public static class Uploader
    {
        private static string ImageDirectory = "UploadedImages";
        private static string ParentPath = Directory.GetParent(Directory.GetParent(HttpContext.Current.Server.MapPath("~")).FullName).FullName;
        private static string UploadedImagesPath = Path.Combine(ParentPath, ImageDirectory);

        // Retorna la imagen en formato Base64.
        public static string GetImageUrl(HttpPostedFileBase Image)
        {
            // string fileName = String.Concat(Guid.NewGuid().ToString(), Path.GetExtension(Image.FileName));
            Stream fileStream = Image.InputStream;

            byte[] bytes;
            using (var memoryStream = new MemoryStream())
            {
                fileStream.CopyTo(memoryStream);
                bytes = memoryStream.ToArray();
            }

            return String.Concat("data:image/jpeg;charset=utf-8;base64, ", Convert.ToBase64String(bytes));
        }
    }
}