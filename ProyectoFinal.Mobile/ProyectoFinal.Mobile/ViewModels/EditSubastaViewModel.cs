using ProyectoFinal.Mobile.Helpers;
using ProyectoFinal.Mobile.Models;
using ProyectoFinal.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ProyectoFinal.Mobile.ViewModels
{
    public class EditSubastaViewModel : BaseViewModel
    {
        public Command GuardarInformacionCommand { get; }
        public Command OpenGalleryCommand { get; }
        public Command OpenCameraCommand { get; }

        private string nombre;
        public string Nombre
        {
            get => nombre;
            set => SetProperty(ref nombre, value);
        }

        private string descripcion;
        public string Descripcion
        {
            get => descripcion;
            set => SetProperty(ref descripcion, value);
        }

        private SubastaDto subasta;
        public SubastaDto Subasta
        {
            get => subasta;
            set => SetProperty(ref subasta, value);
        }

        private ImageData imagen;
        public ImageData Imagen
        {
            get => imagen;
            set => SetProperty(ref imagen, value);
        }

        public EditSubastaViewModel()
        {
            Title = "Editar subasta";
            GuardarInformacionCommand = new Command(OnGuardarInformacionClicked);
            OpenGalleryCommand = new Command(OnOpenGalleryClicked);
            OpenCameraCommand = new Command(OnOpenCameraClicked);

        }

        public async Task CargarSubasta(int subastaID)
        {
            Subasta = await SmartSell.GetSubasta(subastaID);
            Nombre = Subasta.NombreProducto;
            Descripcion = Subasta.DescripcionProducto;
            Imagen = new ImageData(MediaHelper.UriToImageSource(Subasta.UriImagen), Subasta.UriImagen);
        }


        private async void OnGuardarInformacionClicked()
        {
            //await SmartSell.EditSubasta(Subasta.SubastaID, Nombre, Descripcion, Imagen.Base64Uri);
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        private async void OnOpenCameraClicked()
        {
            try
            {
                var imageData = await MediaHelper.CameraInvoker();
                if (imageData == null)
                {
                    return;
                }
                Imagen = imageData;
            }
            catch (Exception ex)
            {
                Console.WriteLine();
            }
        }

        private async void OnOpenGalleryClicked()
        {
            try
            {
                var imageData = await MediaHelper.StorageInvoker();
                if (imageData == null)
                {
                    return;
                }
                Imagen = imageData;
                
            }
            catch (Exception ex)
            {
                Console.WriteLine();
            }
        }
    }
}
