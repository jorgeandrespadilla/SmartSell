using ProyectoFinal.UWP.Infrastructure;
using ProyectoFinal.Shared.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Imaging;
using ProyectoFinal.UWP.Infrastructure.Helpers;
using Windows.Storage;
using ProyectoFinal.UWP.Helpers;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace ProyectoFinal.UWP.Views
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class EditarSubasta : Page
    {
        private SubastaDto subasta;
        private SmartSell smartsell = SmartSell.Instance;
        private StorageFile selectedImage;

        public EditarSubasta()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            try
            {
                subasta = await smartsell.GetSubasta(Int32.Parse(e.Parameter.ToString()));
                CargarInformacion();
            }
            catch(Exception ex)
            {
                await Dialog.InfoMessage("Error", ex.Message).ShowAsync();
            }

        }

        private async void CargarInformacion()
        {
            try
            {
                BitmapImage image = await UriImage.UriToBitmapImage(subasta.UriImagen);
                imagenProducto.Source = image;
                nombreTxt.Text = subasta.NombreProducto;
                descripcionTxt.Text = subasta.DescripcionProducto;
            }
            catch (Exception ex)
            {
                await Dialog.InfoMessage("Error", ex.Message).ShowAsync();

            }

        }

        private async void cargarBtn_Click(object sender, RoutedEventArgs e)
        {
            StorageFile file = await UriImage.CreateImagePicker().PickSingleFileAsync();
            if (file == null)
            {
                return;
            }
            selectedImage = file;
            imagenProducto.Source = await UriImage.FileToBitMapImage(selectedImage);
        }

        private async void EditarHandlerBtn(object sender, RoutedEventArgs e)
        {
            string uriImage;
            if (selectedImage != null)
            {
                uriImage = await UriImage.FileToUri(selectedImage);
            }
            else
            {
                uriImage = subasta.UriImagen;
            }
            try
            {
                await smartsell.EditSubasta(subasta.SubastaID, nombreTxt.Text, descripcionTxt.Text, uriImage);
                this.Frame.Navigate(typeof(DetailsSubasta), subasta.SubastaID);
            }
            catch (Exception ex)
            {
                await Dialog.InfoMessage("Error", ex.Message).ShowAsync();
            }
        }

        private void CancelarHandlerBtn(object sender, RoutedEventArgs e)
        {
            ReturnNavHelper.TryGoBack();
        }
    }
}
