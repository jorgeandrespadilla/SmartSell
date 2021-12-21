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

        private SubastaDto subasta;

        private SmartSell smartsell = SmartSell.Instance;

        public DetailsSubasta()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            try
            {
                subasta = await smartsell.GetSubasta(Int32.Parse(e.Parameter.ToString()));
            }
            catch (Exception ex)
            {
                await Dialog.InfoMessage("Registro fallido", ex.Message).ShowAsync();
            }
            CargarInformacion();
        }

        public async void CargarInformacion()
        {
            try
            {
                if (subasta.Vigente)
                {
                    if (subasta.UsuarioID == smartsell.CurrentUser.ID)
                    {
                        buttonSubastadorWrapper.Visibility = Visibility.Visible;
                        if (DateTime.Compare(subasta.Fecha.AddDays(-1), DateTime.Now) >= 0)
                        {
                            eliminarSubastabtn.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            eliminarSubastabtn.Visibility = Visibility.Collapsed;
                        }

                        buttonOfertanteWrapper.Visibility = Visibility.Collapsed;

                    }
                    else
                    {
                        buttonSubastadorWrapper.Visibility = Visibility.Collapsed;
                        buttonOfertanteWrapper.Visibility = Visibility.Visible;
                        if (subasta.Ofertas.Count != 0)
                        {
                            if (subasta.Ofertas.FirstOrDefault().UsuarioID == smartsell.CurrentUser.ID)
                            {
                                eliminarOfertaBtn.Visibility = Visibility.Visible;
                            }
                            else
                            {
                                eliminarOfertaBtn.Visibility = Visibility.Collapsed;
                            }
                        }
                        else
                        {
                            eliminarOfertaBtn.Visibility = Visibility.Collapsed;
                        }

                    }
                }
                else
                {
                    buttonSubastadorWrapper.Visibility = Visibility.Collapsed;
                    buttonOfertanteWrapper.Visibility = Visibility.Collapsed;
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
            }
            catch (Exception ex)
            {
                await Dialog.InfoMessage("Registro fallido", ex.Message).ShowAsync();
            } 
        }

   
        private void NavigatePerfilVendedor(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Perfil), subasta.UsuarioID);
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

        private void EditarSubastaHandlerButton(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(EditarSubasta), subasta.SubastaID);
        }

        private async void EliminarSubastaHandlerButton(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = await Dialog.ConfirmationMessage("Eliminar Subasta", "¿Seguro que desea eliminar la subasta?").ShowAsync();
                if ((int)result.Id == 1)
                {
                    await smartsell.DeleteSubasta(subasta.SubastaID);
                    this.Frame.Navigate(typeof(IndexSubastas), null);
                }               
            }
            catch (Exception ex)
            {
                await Dialog.InfoMessage("Registro fallido", ex.Message).ShowAsync();
            }
        }
    }
}
