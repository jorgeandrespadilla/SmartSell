using ProyectoFinal.Mobile.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ProyectoFinal.Mobile.Helpers
{
    public class MediaHelper
    {
        public static async Task<ImageData> CameraInvoker()
        {
            try
            {
                await Permissions.RequestAsync<Permissions.Camera>();
                await Permissions.RequestAsync<Permissions.StorageRead>();
                await Permissions.RequestAsync<Permissions.StorageWrite>();
                var result = await MediaPicker.CapturePhotoAsync();
                if (result == null)
                {
                    return null;
                }
                using (Stream stream = await result.OpenReadAsync())
                {
                    return new ImageData(StreamToImageSource(stream), StreamToUri(stream));
                }
            }
            catch (FeatureNotSupportedException)
            {
                throw new Exception("La función no es compatible con el dispositivo.");
            }
            catch (PermissionException)
            {
                throw new Exception("Permisos no concedidos. Habilite todos los permisos desde las configuraciones del teléfono.");
            }
            catch (Exception)
            {
                throw new Exception("No fue posible cargar la imagen.");
            }
        }

        public static async Task<ImageData> StorageInvoker()
        {
            try
            {
                await Permissions.RequestAsync<Permissions.Camera>();
                await Permissions.RequestAsync<Permissions.StorageRead>();
                await Permissions.RequestAsync<Permissions.StorageWrite>();
                var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
                {
                    Title = "Seleccione una imagen"
                });
                if (result == null)
                {
                    return null;
                }
                using (Stream stream = await result.OpenReadAsync())
                {
                    return new ImageData(StreamToImageSource(stream), StreamToUri(stream));
                }
            }
            catch (FeatureNotSupportedException)
            {
                throw new Exception("La función no es compatible con el dispositivo.");
            }
            catch (PermissionException)
            {
                throw new Exception("Permisos no concedidos. Habilite todos los permisos desde las configuraciones del teléfono.");
            }
            catch (Exception)
            {
                throw new Exception("No fue posible cargar la imagen.");
            }
        }

        public static ImageSource UriToImageSource(string imageUri)
        {
            string imgString = imageUri.Split(',').Last().Trim();
            byte[] bytes = Convert.FromBase64String(imgString);
            return ImageSource.FromStream(() => new MemoryStream(bytes));
        }

        private static string StreamToUri(Stream stream)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                stream.Seek(0, SeekOrigin.Begin);
                byte[] bytes = memoryStream.ToArray();
                return string.Concat("data:image/jpeg;charset=utf-8;base64, ", Convert.ToBase64String(bytes));
            }
        }

        private static ImageSource StreamToImageSource(Stream stream)
        {
            MemoryStream memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            stream.Seek(0, SeekOrigin.Begin);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return ImageSource.FromStream(() => memoryStream);
        }
    }
}
