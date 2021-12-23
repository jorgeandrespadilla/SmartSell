using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace ProyectoFinal.Desktop.Infrastructure.Helpers
{
    class UriImage
    {
        public static async Task<BitmapImage> UriToBitmapImage(string uri)
        {
            string imgString = uri.Split(", ").Last();
            byte[] bytes = Convert.FromBase64String(imgString);
            BitmapImage image = new BitmapImage();
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                await image.SetSourceAsync(stream.AsRandomAccessStream());
            }
            return image;
        }

        // https://bit.ly/3Ir0RUV
        public static async Task<string> BitmapImageToUri(BitmapImage bitmapImage)
        {
            RandomAccessStreamReference streamRef = RandomAccessStreamReference.CreateFromUri(bitmapImage.UriSource);
            IRandomAccessStreamWithContentType stream = await streamRef.OpenReadAsync();
            byte[] bytes = new byte[stream.Size];
            await stream.ReadAsync(bytes.AsBuffer(), (uint)stream.Size, InputStreamOptions.None);
            return String.Concat("data:image/jpeg;charset=utf-8;base64, ", Convert.ToBase64String(bytes));
        }

        // https://www.py4u.net/discuss/754119 -> Could be donde directly when image is uploaded
        // https://github.com/sunteenwu/PersonalDemo/blob/master/Cuploadpicture/CuploadpictureClient/MainPage.xaml.cs
    }
}
