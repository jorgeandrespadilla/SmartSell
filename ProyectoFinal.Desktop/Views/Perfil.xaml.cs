using ProyectoFinal.Desktop.Infrastructure;
using ProyectoFinal.Desktop.Models;
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

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace ProyectoFinal.Desktop.Views
{
    /*Necesario implementar que le diseño sea reactivo*/
    public sealed partial class Perfil : Page
    {
        //Usuario usuarioActual;
        private SmartSell smartSell = SmartSell.Instance;


        public Perfil()
        {
            this.InitializeComponent();

        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter != null)
            {

                if (int.Parse(e.Parameter.ToString()) == smartSell.CurrentUser.ID)
                {
                    PerfilDto usuarioActual = await smartSell.GetPerfil();
                    CargarInformacion(usuarioActual);
                }
                else
                {
                    PerfilVendedorDto usuarioActual = await smartSell.GetPerfilVendedor(int.Parse(e.Parameter.ToString()));
                    CargarInformacionVendedor(usuarioActual);
                }
                
            }
        }



        private async void CargarInformacion(PerfilDto usuarioActual)
        {
            buttonWrapper.Visibility = Visibility.Visible;
            ratingWrapper.Visibility = Visibility.Collapsed;
            var ofertasQuery = await smartSell.GetPerfilOfertas("PARTICIPACION");
            MisOfertas.ItemsSource = ofertasQuery;
                       
            nombreCompletoTxt.Text = usuarioActual.Nombres + " " + usuarioActual.Apellidos;
            nombresTxt.Text = usuarioActual.Nombres;
            apellidosTxt.Text = usuarioActual.Apellidos;
            correoTxt.Text = usuarioActual.Correo;
            calificacionTxt.Text = usuarioActual.AvgRating.ToString();
        }


        private void CargarInformacionVendedor(PerfilVendedorDto usuarioActual)
        {

            opSelected.Visibility = Visibility.Collapsed;
            buttonWrapper.Visibility = Visibility.Collapsed;
            ratingWrapper.Visibility = Visibility.Visible;
            tableWrapper.Visibility = Visibility.Collapsed;

            nombreCompletoTxt.Text = usuarioActual.Nombres + " " + usuarioActual.Apellidos;
            nombresTxt.Text = usuarioActual.Nombres;
            apellidosTxt.Text = usuarioActual.Apellidos;
            correoTxt.Text = usuarioActual.Correo;
            calificacionTxt.Text = usuarioActual.AvgRating.ToString();
            ratingUsuarioBtn.Value = usuarioActual.Rating;

        }

        private async void ActualizarTabla(object sender, SelectionChangedEventArgs e)
        {
            int op = opSelected.SelectedIndex;
            if (op == 0)
            {
                var ofertasQuery = await smartSell.GetPerfilOfertas("PARTICIPACION");
                MisOfertas.ItemsSource = ofertasQuery;
            }
            else if(op==1)
            {
                var ofertasQuery = await smartSell.GetPerfilOfertas("GANADAS");
                MisOfertas.ItemsSource = ofertasQuery;
            }
        }


        private void ratingUsuarioBtn_DataContextChanged(RatingControl sender, object args)
        {
            double RatingControl = ratingUsuarioBtn.Value;
            calificacionTxt.Text = RatingControl.ToString();
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
