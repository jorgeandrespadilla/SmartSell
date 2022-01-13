using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ProyectoFinal.Mobile.Helpers
{
    public class MediaHelper
    {
        public static async Task<ImageSource> CameraInvoker()
        {
            try
            {
                await Permissions.RequestAsync<Permissions.Camera>();
                await Permissions.RequestAsync<Permissions.StorageRead>();
                await Permissions.RequestAsync<Permissions.StorageWrite>();
                var result = await MediaPicker.CapturePhotoAsync();
                if (result != null)
                {
                    var stream = await result.OpenReadAsync();
                    return ImageSource.FromStream(() => stream);
                }
                return null;
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

        public static async Task<ImageSource> StorageInvoker()
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
                if (result != null)
                {
                    var stream = await result.OpenReadAsync();
                    return ImageSource.FromStream(() => stream);
                }
                return null;
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
    }
}
