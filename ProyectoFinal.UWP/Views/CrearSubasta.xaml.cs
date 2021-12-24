using ProyectoFinal.UWP.Infrastructure;
using ProyectoFinal.UWP.Infrastructure.Helpers;
using System;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace ProyectoFinal.UWP.Views
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class CrearSubasta : Page
    {
        private StorageFile selectedImage;
        private SmartSell smartsell = SmartSell.Instance;

        public CrearSubasta()
        {
            this.InitializeComponent();
        }

        private async void cargarBtn_Click(object sender, RoutedEventArgs e)
        {
            StorageFile file = await UriImage.CreateImagePicker().PickSingleFileAsync();
            if (file == null)
            {
                return;
            }
            selectedImage = file;
            ImagenProducto.Source = await UriImage.FileToBitMapImage(selectedImage);
        }

        private async void crearBtn_Click(object sender, RoutedEventArgs e)
        {
            string uriImage = await UriImage.FileToUri(selectedImage);
            try
            {
                DateTimeOffset Fecha = fechaSelected.Date ?? default(DateTimeOffset);
                await smartsell.CreateSubasta(nombreTxt.Text, descripcionTxt.Text,uriImage,float.Parse(precioTxt.Text), Fecha.Date);
                this.Frame.Navigate(typeof(IndexSubastasPage), null);
            }
            catch (Exception ex)
            {
                await Dialog.InfoMessage("Error", ex.Message).ShowAsync();
            }
            
        }
    }
}
