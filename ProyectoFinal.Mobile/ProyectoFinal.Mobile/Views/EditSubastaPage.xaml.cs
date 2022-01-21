using ProyectoFinal.Mobile.Helpers;
using ProyectoFinal.Mobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoFinal.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty(nameof(SubastaID), "id")]

    public partial class EditSubastaPage : ContentPage
    {
        public int SubastaID { get; set; }
        public EditSubastaPage()
        {
            InitializeComponent();
            BindingContext = new EditSubastaViewModel();
            btnTomarFoto.Clicked += async (sender, args) =>
            {
                try
                {
                    var imageData = await MediaHelper.CameraInvoker();
                    if (imageData == null)
                    {
                        return;
                    }
                    imagen.Source = imageData.ImageSource;
                    ((EditSubastaViewModel)BindingContext).Imagen = imageData;
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", ex.Message, "Aceptar");
                }
            };

            btnSeleccionarFoto.Clicked += async (sender, args) =>
            {
                try
                {
                    var imageData = await MediaHelper.StorageInvoker();
                    if (imageData == null)
                    {
                        return;
                    }
                    imagen.Source = imageData.ImageSource;
                    ((EditSubastaViewModel)BindingContext).Imagen = imageData;
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", ex.Message, "Aceptar");
                }
            };
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await ((EditSubastaViewModel)BindingContext).CargarSubasta(SubastaID);
        }


    }
}          