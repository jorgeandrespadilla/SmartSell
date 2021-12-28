using ProyectoFinal.UWP.Infrastructure;
using ProyectoFinal.UWP.Infrastructure.Helpers;
using ProyectoFinal.UWP.Models;
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
using ProyectoFinal.UWP.Helpers;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace ProyectoFinal.UWP.Views
{
    /*
     * 
     * Es necesario dar condicional para comprbar si un comentario le pertence y si ese es el caso, mostrar permitir editar
     * 
     */
    public sealed partial class DetailsSubasta : Page
    {

        private SubastaDto subasta;

        private ICollection<Comentario> comentarios;

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
                CargarInformacion(Int32.Parse(e.Parameter.ToString()));
            }
            catch (Exception ex)
            {
                await Dialog.InfoMessage("Error", ex.Message).ShowAsync();
            }

        }

        public async void CargarInformacion(int id)
        {
            try
            {

                subasta = await smartsell.GetSubasta(id);
                comentarios = smartsell.ComentariosDtoToComentarios(subasta.Comentarios);
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
                ComentariosGrid.ItemsSource = comentarios;

            }
            catch (Exception ex)
            {
                await Dialog.InfoMessage("Error", ex.Message).ShowAsync();
            }
        }




        private void NavigatePerfilVendedor(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(PerfilPage), subasta.UsuarioID);
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
                    this.Frame.Navigate(typeof(IndexSubastasPage), null);
                }
            }
            catch (Exception ex)
            {
                await Dialog.InfoMessage("Error", ex.Message).ShowAsync();
            }
        }

        private void OfertarHandlerBtn(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CrearOferta), subasta.SubastaID);
        }

        private async void EliminarOfertaHandlerBtn(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = await Dialog.ConfirmationMessage("Eliminar oferta", "¿Seguro que desea eliminar su oferta?").ShowAsync();
                if ((int)result.Id == 1)
                {
                    await smartsell.DeleteOferta(subasta.Ofertas.FirstOrDefault().OfertaID);
                    CargarInformacion(subasta.SubastaID);
                }
            }
            catch (Exception ex)
            {
                await Dialog.InfoMessage("Error", ex.Message).ShowAsync();
            }
        }

        private void CrearComentarioHandlerBtn(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CrearComentario), subasta.SubastaID);
        }


        private void Volver(object sender, RoutedEventArgs e)
        {
            ReturnNavHelper.TryGoBack();
        }

        private async void EliminarComentarioHandlerBtn(object sender, RoutedEventArgs e)
        {
            HyperlinkButton link = e.OriginalSource as HyperlinkButton;
            Comentario comentario = link.DataContext as Comentario;
            try
            {
                var result = await Dialog.ConfirmationMessage("Eliminar comentario", "¿Seguro que desea eliminar el comentario?").ShowAsync();
                if ((int)result.Id == 1)
                {
                    await smartsell.DeleteComentario(comentario.ComentarioID);
                    CargarInformacion(subasta.SubastaID);
                }
            }
            catch (Exception ex)
            {
                await Dialog.InfoMessage("Error", ex.Message).ShowAsync();
            }

        }

        private void EditarComentarioHandlerBtn(object sender, RoutedEventArgs e)
        {
            HyperlinkButton link = e.OriginalSource as HyperlinkButton;
            Comentario comentario = link.DataContext as Comentario;
            this.Frame.Navigate(typeof(EditarComentario), comentario.ComentarioID);
        }
    }




}
