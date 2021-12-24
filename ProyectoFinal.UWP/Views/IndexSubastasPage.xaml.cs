using System;
using System.Collections.Generic;
using ProyectoFinal.UWP.Infrastructure;
using ProyectoFinal.UWP.Models;
using ProyectoFinal.UWP.Views;
using ProyectoFinal.Shared.Dto;
using ProyectoFinal.Shared.Models;
using ProyectoFinal.UWP.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace ProyectoFinal.UWP.Views
{
    public sealed partial class IndexSubastasPage : Page
    {
        private SmartSell smartSell = SmartSell.Instance;
        private ICollection<SubastaItem> subastasCargadas;
        private string mode = "TodasSubastas";

        public IndexSubastasViewModel ViewModel { get; } = new IndexSubastasViewModel();

        public IndexSubastasPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var resp = await smartSell.GetSubastas();
            subastasCargadas = await smartSell.ConvertToSubastaItems(resp.Data);
            cargarSubastas();
        }
        public void cargarSubastas()
        {
            subastas.ItemsSource = subastasCargadas;
        }

        private void subastas_SelectionChanged(object sender, ItemClickEventArgs e)
        {
            SubastaItem subasta = e.ClickedItem as SubastaItem;
            this.Frame.Navigate(typeof(DetailsSubasta), subasta.ID);
        }

        private void CrearSubastaHandler(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CrearSubasta), null);
        }

        private async void MisSubastasHandlerBtn(object sender, RoutedEventArgs e)
        {
            todasSubastas.Visibility = Visibility.Visible;
            misSubastas.Visibility = Visibility.Collapsed;
            ocultarMisSubastas.Visibility = Visibility.Collapsed;
            var resp = await smartSell.GetSubastas(showAll:"false",hideMySubastas:"false", searchString: buscarTxt.Text, hideEnded: ocultarFinalizadas.IsChecked.ToString().ToLower());
            subastasCargadas = await smartSell.ConvertToSubastaItems(resp.Data);
            mode = "MisSubastas";
            cargarSubastas();
            
        }

        private async void TodasSubastasHandlerBtn(object sender, RoutedEventArgs e)
        {
            todasSubastas.Visibility = Visibility.Collapsed;
            misSubastas.Visibility = Visibility.Visible;
            ocultarMisSubastas.Visibility = Visibility.Visible;
            ocultarMisSubastas.IsChecked = true;
            var resp = await smartSell.GetSubastas(showAll: "true", hideMySubastas: "true", searchString: buscarTxt.Text, hideEnded: ocultarFinalizadas.IsChecked.ToString().ToLower());
            subastasCargadas = await smartSell.ConvertToSubastaItems(resp.Data);
            mode = "TodasSubastas";
            cargarSubastas();
        }

        private async void BuscarHandlerBtn(object sender, RoutedEventArgs e)
        {
            if(mode == "MisSubastas")
            {
                var resp = await smartSell.GetSubastas(searchString:buscarTxt.Text, showAll: "false", hideMySubastas: "false", hideEnded: ocultarFinalizadas.IsChecked.ToString().ToLower());
                subastasCargadas = await smartSell.ConvertToSubastaItems(resp.Data);
            }
            else
            {
                var resp = await smartSell.GetSubastas(searchString: buscarTxt.Text, hideEnded: ocultarFinalizadas.IsChecked.ToString().ToLower() , showAll: "true", hideMySubastas: ocultarMisSubastas.IsChecked.ToString().ToLower());
                subastasCargadas = await smartSell.ConvertToSubastaItems(resp.Data);
            }
            cargarSubastas();
            
        }
    }
}
