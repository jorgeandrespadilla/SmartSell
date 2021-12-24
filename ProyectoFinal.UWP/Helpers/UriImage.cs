using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace ProyectoFinal.UWP.Infrastructure.Helpers
{
    public class UriImage
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

        public static async Task<BitmapImage> FileToBitMapImage(StorageFile file)
        {
            BitmapImage image = new BitmapImage();
            using (IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read))
            {
                await image.SetSourceAsync(fileStream);
            }
            return image;
        }

        // https://stackoverflow.com/a/34583333
        public async static Task<string> FileToUri(StorageFile file)
        {
            byte[] bytes;
            using (IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read))
            {
                var dataReader = new DataReader(fileStream.GetInputStreamAt(0));
                bytes = new byte[fileStream.Size];
                await dataReader.LoadAsync((uint)fileStream.Size);
                dataReader.ReadBytes(bytes);
            }
            return string.Concat("data:image/jpeg;charset=utf-8;base64, ", Convert.ToBase64String(bytes));
        }
        // https://www.py4u.net/discuss/754119 -> Could be donde directly when image is uploaded
        // https://github.com/sunteenwu/PersonalDemo/blob/master/Cuploadpicture/CuploadpictureClient/MainPage.xaml.cs

        public static FileOpenPicker CreateImagePicker()
        {
            var picker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");

            return picker;
        }
    }
}
