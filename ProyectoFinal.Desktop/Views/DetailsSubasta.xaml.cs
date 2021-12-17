using ProyectoFinal.Desktop.Infrastructure;
using ProyectoFinal.Desktop.Infrastructure.Helpers;
using ProyectoFinal.Desktop.Models;
using ProyectoFinal.Shared.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace ProyectoFinal.Desktop.Views
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    /// 

    

    public sealed partial class DetailsSubasta : Page
    {

        private int UsuarioSubasta;
        
        private SmartSell smartsell = SmartSell.Instance;

        public DetailsSubasta()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            SubastaDto subasta = await smartsell.GetSubasta(Int32.Parse(e.Parameter.ToString()));
            CargarInformacion(subasta);
        }

        public async void CargarInformacion(SubastaDto subasta)
        {
            if (subasta.UsuarioID == smartsell.CurrentUser.ID) {
                buttonEliminarWrapper.Visibility = Visibility.Visible;
                buttonWrapper.Visibility = Visibility.Collapsed;
            }
            else
            {
                buttonEliminarWrapper.Visibility = Visibility.Collapsed;
                buttonWrapper.Visibility = Visibility.Visible;
            }
            BitmapImage image = await UriImage.UriToBitmapImage(subasta.UriImagen);
            imagenProducto.Source = image;
            nombreTxt.Text = subasta.NombreProducto;
            nombreVendedor.Text = $"{subasta.NombreVendedor}";
            precioTxt.Text = subasta.MontoActual.ToString();

            descripcionTxt.Text = subasta.DescripcionProducto;
            if (subasta.Vigente == true)
            {
                vigenteTxt.Text = "Sí";
            }
            else
            {
                vigenteTxt.Text = "No";
            }
            fechaTxt.Text = subasta.Fecha.ToString();
            OfertasSubasta.ItemsSource = subasta.Ofertas;
            ComentariosGrid.ItemsSource = subasta.Comentarios;
            UsuarioSubasta = subasta.UsuarioID;
        }

   
        private void NavigatePerfilVendedor(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Perfil), UsuarioSubasta);
        }

        private void Volver(object sender, RoutedEventArgs e)
        {
            TryGoBack();
        }

        public static bool TryGoBack()
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame.CanGoBack)
            {
                rootFrame.GoBack();
                return true;
            }
            return false;
        }
    }
}
