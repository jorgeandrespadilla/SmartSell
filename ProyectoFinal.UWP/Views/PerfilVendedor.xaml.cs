using ProyectoFinal.Shared.Dto;
using ProyectoFinal.UWP.Infrastructure;
using ProyectoFinal.UWP.Infrastructure.Helpers;
using ProyectoFinal.UWP.Services;
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

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace ProyectoFinal.UWP.Views
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class PerfilVendedor : Page
    {

        private SmartSell smartSell = SmartSell.Instance;
        private int usuarioCalificadoID = -1;


        public PerfilVendedor()
        {
            this.InitializeComponent();
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            usuarioCalificadoID = int.Parse(e.Parameter.ToString());
            CargarInformacionVendedor(usuarioCalificadoID);
        }

        private async void CargarInformacionVendedor(int id)
        {
            try
            {
                PerfilVendedorDto usuarioActual = await smartSell.GetPerfilVendedor(id);

                nombreCompletoTxt.Text = usuarioActual.Nombres + " " + usuarioActual.Apellidos;
                nombresTxt.Text = usuarioActual.Nombres;
                apellidosTxt.Text = usuarioActual.Apellidos;
                correoTxt.Text = usuarioActual.Correo;
                calificacionTxt.Text = $"{Math.Round(usuarioActual.AvgRating, 2).ToString("F2")}/{5:F2}";
                ratingUsuarioBtn.Value = usuarioActual.Rating;
            }
            catch (Exception ex)
            {
                await Dialog.InfoMessage("Error", ex.Message).ShowAsync();
                NavigationService.GoBack();
            }


        }

        private async void EnviarRatingHandlerButton(object sender, RoutedEventArgs e)
        {
            int RatingControl = (int)ratingUsuarioBtn.Value;
            try
            {
                await smartSell.SetRatingUsuario(usuarioCalificadoID, RatingControl);
                CargarInformacionVendedor(usuarioCalificadoID);
            }
            catch (Exception ex)
            {
                await Dialog.InfoMessage("Error", ex.Message).ShowAsync();
            }
        }
    }
}
