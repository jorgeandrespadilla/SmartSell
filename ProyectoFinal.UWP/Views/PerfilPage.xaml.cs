using ProyectoFinal.UWP.Infrastructure;
using ProyectoFinal.UWP.Models;
using ProyectoFinal.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using ProyectoFinal.UWP.Infrastructure.Helpers;
using ProyectoFinal.UWP.Helpers;

namespace ProyectoFinal.UWP.Views
{
    /*Necesario implementar que le diseño sea reactivo*/
    public sealed partial class PerfilPage : Page
    {
        //Usuario usuarioActual;
        private SmartSell smartSell = SmartSell.Instance;
        

        public PerfilPage()
        {
            this.InitializeComponent();

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            CargarInformacion();
        }



        private async void CargarInformacion()
        {
            PerfilDto usuarioActual = await smartSell.GetPerfil();
            var ofertasQuery = await smartSell.GetPerfilOfertas("PARTICIPACION");
            MisOfertas.ItemsSource = ofertasQuery;

            nombreCompletoTxt.Text = usuarioActual.Nombres + " " + usuarioActual.Apellidos;
            nombresTxt.Text = usuarioActual.Nombres;
            apellidosTxt.Text = usuarioActual.Apellidos;
            correoTxt.Text = usuarioActual.Correo;
            calificacionTxt.Text = usuarioActual.AvgRating.ToString();
        }

        private async void ActualizarTabla(object sender, SelectionChangedEventArgs e)
        {
            int op = opSelected.SelectedIndex;
            if (op == 0)
            {
                var ofertasQuery = await smartSell.GetPerfilOfertas("PARTICIPACION");
                MisOfertas.ItemsSource = ofertasQuery;
            }
            else if (op == 1)
            {
                var ofertasQuery = await smartSell.GetPerfilOfertas("GANADAS");
                MisOfertas.ItemsSource = ofertasQuery;
            }
        }

        private void Volver(object sender, RoutedEventArgs e)
        {
            ReturnNavHelper.TryGoBack();
        }



        private void EditarButtonHandler(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Editar), null);
        }

        private async void EliminarHandlerBtn(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = await Dialog.ConfirmationMessage("Eliminar usuario", "¿Seguro que desea eliminar el usuario?").ShowAsync();
                if ((int)result.Id == 1)
                {
                    await smartSell.DeletePerfil();
                    this.Frame.Navigate(typeof(Login), null);
                }
            }
            catch (Exception ex)
            {
                await Dialog.InfoMessage("Error", ex.Message).ShowAsync();
            }
        }

        private void VerSubastaHandlerBtn(object sender, RoutedEventArgs e)
        {
            HyperlinkButton link = e.OriginalSource as HyperlinkButton;
            OfertaDto oferta = link.DataContext as OfertaDto;

            this.Frame.Navigate(typeof(DetailsSubasta), oferta.SubastaID);
        }
    }
}
