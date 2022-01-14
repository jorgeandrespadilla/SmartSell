using ProyectoFinal.Mobile.Helpers;
using System;
using System.ComponentModel;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoFinal.Mobile.Views
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();

            btnTomarFoto.Clicked += async(sender, args) =>
            {
                try
                {
                    var imageData = await MediaHelper.CameraInvoker();
                    imagen.Source = imageData.ImageSource;
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
                    imagen.Source = imageData.ImageSource;
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", ex.Message, "Aceptar");
                }
            };
        }
    }
}