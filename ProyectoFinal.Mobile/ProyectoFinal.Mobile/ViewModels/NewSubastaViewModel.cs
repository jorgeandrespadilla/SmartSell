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
    public class NewSubastaViewModel : BaseViewModel
    {
        public Command SaveCommand { get; }
        public Command OpenGalleryCommand { get; }
        public Command OpenCameraCommand { get; }

        private bool canDelete;
        public bool CanDelete
        {
            get => canDelete;
            set => SetProperty(ref canDelete, value);
        }

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

        private float precio;
        public float Precio
        {
            get => precio;
            set => SetProperty(ref precio, value);
        }

        private DateTime fecha;
        public DateTime Fecha
        {
            get => fecha;
            set => SetProperty(ref fecha, value);
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

        public NewSubastaViewModel()
        {
            Title = "Crear subasta";
            SaveCommand = new Command(OnSaveClicked);
            OpenGalleryCommand = new Command(OnOpenGalleryClicked);
            OpenCameraCommand = new Command(OnOpenCameraClicked);

        }

        private async void OnSaveClicked()
        {
            try
            {
                if(Imagen != null)
                {
                    float PrecioRedondeado = (float)(Math.Round(Precio,2));
                    await SmartSell.CreateSubasta(Nombre, Descripcion, Imagen.Base64Uri, PrecioRedondeado, Fecha);
                    await Shell.Current.GoToAsync($"..");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Seleccione una imagen", "Aceptar");
                }
                
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Aceptar");
            }
        }

        private async void OnOpenCameraClicked()
        {
            try
            {
                ImageData imageData = await MediaHelper.CameraInvoker();
                if (imageData == null)
                {
                    return;
                }
                Imagen = imageData;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Aceptar");
            }
        }

        private async void OnOpenGalleryClicked()
        {
            try
            {
                ImageData imageData = await MediaHelper.StorageInvoker();
                if (imageData == null)
                {
                    return;
                }
                Imagen = imageData;

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Aceptar");
            }
        }

    }
}
