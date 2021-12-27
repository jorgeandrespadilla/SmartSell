using System;
using System.Collections.Generic;
using ProyectoFinal.Shared.Dto;
using ProyectoFinal.Shared.Models;
using ProyectoFinal.UWP.Infrastructure;
using ProyectoFinal.UWP.Infrastructure.Helpers;
using ProyectoFinal.UWP.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace ProyectoFinal.UWP.Views
{
    public sealed partial class IndexOfertasPage : Page
    {
        public IndexOfertasViewModel ViewModel { get; } = new IndexOfertasViewModel();


        private SmartSell smartsell = SmartSell.Instance;

        private PagedData<OfertaDto> ofertasCargadas;



        public IndexOfertasPage()
        {
            InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            try
            {
                ofertasCargadas = await smartsell.GetOfertas();
                CargarOfertas();
            }
            catch (Exception ex)
            {
                await Dialog.InfoMessage("Error", ex.Message).ShowAsync();
            }
        }

        private void CargarOfertas()
        {
            ofertas.ItemsSource = ofertasCargadas.Data;
        }

        private void VerSubastaHandlerBtn(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            
        }

        private async void BuscarHandlerBtn(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ofertasCargadas = await smartsell.GetOfertas(searchString:buscarTxt.Text);
            CargarOfertas();
        }
    }
}
