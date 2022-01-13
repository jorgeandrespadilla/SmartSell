using System;
using System.ComponentModel;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoFinal.Mobile.Views
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();

            btnTomarFoto.Clicked += async (sender, args) =>
            {
                var result = await MediaPicker.CapturePhotoAsync();
                if (result != null)
                {
                    var stream = await result.OpenReadAsync();
                    imagen.Source = ImageSource.FromStream(() => stream);
                }
            };

            btnSeleccionarFoto.Clicked += async (sender, args) =>
            {
                var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
                {
                    Title = "Please pick a photo"
                });
                if (result != null)
                {
                    var stream = await result.OpenReadAsync();
                    imagen.Source = ImageSource.FromStream(() => stream);
                }
            };
        }
    }
}