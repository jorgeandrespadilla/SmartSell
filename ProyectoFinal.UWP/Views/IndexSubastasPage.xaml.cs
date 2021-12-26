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
using Windows.UI.Xaml.Media;
using System.Threading.Tasks;

namespace ProyectoFinal.UWP.Views
{
    /*
     * 
     * Es necesario dar condiocional para comprbar si una subasta ha finalizado y si ese es el caso, mostrar "Finalizado"
     * Devolver la cantidad total de subastas encontradas
     */
    public sealed partial class IndexSubastasPage : Page
    {
        private SmartSell smartSell = SmartSell.Instance;
        private SubastasPagedData results;
        private string mode = "TodasSubastas";
        private string filtroSeleccionado = "none";

        public IndexSubastasViewModel ViewModel { get; } = new IndexSubastasViewModel();

        public IndexSubastasPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            filtroActualTxt.Text = "Filtro: Ninguno";
            await ObtenerSubastas();
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

            mode = "MisSubastas";

            await ObtenerSubastas();
        }

        private async void TodasSubastasHandlerBtn(object sender, RoutedEventArgs e)
        {
            todasSubastas.Visibility = Visibility.Collapsed;
            misSubastas.Visibility = Visibility.Visible;
            ocultarMisSubastas.Visibility = Visibility.Visible;

            ocultarMisSubastas.IsChecked = true;

            mode = "TodasSubastas";

            await ObtenerSubastas();
        }

        private async void BuscarHandlerBtn(object sender, RoutedEventArgs e)
        {
            await ObtenerSubastas();
        }

        private async void FilterHandlerBtn(object sender, RoutedEventArgs e)
        {
            var opSeleccionada = e.OriginalSource;

            if (opSeleccionada.Equals(price_asc))
            {
                filtroSeleccionado = "price_asc";
                filtroActualTxt.Text = "Filtro: Precio ascendente";
            }else if (opSeleccionada.Equals(price_desc))
            {
                filtroSeleccionado = "price_desc";
                filtroActualTxt.Text = "Filtro: Precio descendente";
            }
            else if (opSeleccionada.Equals(name_asc))
            {
                filtroSeleccionado = "name_asc";
                filtroActualTxt.Text = "Filtro: Nombre ascendente";
            }
            else if (opSeleccionada.Equals(name_desc))
            {
                filtroSeleccionado = "name_desc";
                filtroActualTxt.Text = "Filtro: Nombre descendente";
            }
            else if (opSeleccionada.Equals(noneFilter))
            {
                filtroSeleccionado = "none";
                filtroActualTxt.Text = "Filtro: Ninguno";
            }

            await ObtenerSubastas();
        }

        private async void CargarSubastas()
        {
            subastas.ItemsSource = await smartSell.ConvertToSubastaItems(results.Data);
            cantSubastasTxt.Text = $"{results.TotalResults} resultados encontrados";
        }

        private async Task ObtenerSubastas()
        {
            if (mode == "MisSubastas")
            {
                var resp = await smartSell.GetSubastas(
                    searchString: buscarTxt.Text,
                    showAll: "false",
                    hideMySubastas: "false",
                    hideEnded: ocultarFinalizadas.IsChecked.ToString().ToLower(),
                    sortOrder: filtroSeleccionado
                );
                results = resp;
            }
            else
            {
                var resp = await smartSell.GetSubastas(
                    searchString: buscarTxt.Text,
                    hideEnded: ocultarFinalizadas.IsChecked.ToString().ToLower(),
                    showAll: "true",
                    hideMySubastas: ocultarMisSubastas.IsChecked.ToString().ToLower(),
                    sortOrder: filtroSeleccionado
                );
                results = resp;
            }
            CargarSubastas();
        }
    }
}
