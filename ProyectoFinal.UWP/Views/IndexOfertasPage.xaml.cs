using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProyectoFinal.Shared.Dto;
using ProyectoFinal.Shared.Models;
using ProyectoFinal.UWP.Infrastructure;
using ProyectoFinal.UWP.Infrastructure.Helpers;
using ProyectoFinal.UWP.Models;
using ProyectoFinal.UWP.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace ProyectoFinal.UWP.Views
{
    public sealed partial class IndexOfertasPage : Page
    {
        public IndexOfertasViewModel ViewModel { get; } = new IndexOfertasViewModel();


        private SmartSell smartsell = SmartSell.Instance;

        private PagedData<OfertaDto> ofertasCargadas;

        private int page = 1;


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

            // Pagination
            page = ofertasCargadas.Page;
            int totalPages = ofertasCargadas.PageCount;
            cantPaginas.Text = $"Página {page} de {totalPages}";
            PrevButton.IsEnabled = page != 1;
            NextButton.IsEnabled = page != totalPages;
            if (totalPages == 1)
            {
                PrevButton.IsEnabled = false;
                NextButton.IsEnabled = false;
            }
        }

        private void VerSubastaHandlerBtn(object sender, RoutedEventArgs e)
        {
            HyperlinkButton link = e.OriginalSource as HyperlinkButton;
            OfertaDto ofertaSeleccionada = link.DataContext as OfertaDto;
            this.Frame.Navigate(typeof(DetailsSubasta), ofertaSeleccionada.SubastaID);
        }

        private async void BuscarHandlerBtn(object sender, RoutedEventArgs e)
        {
            await ObtenerOfertas();
        }

        private async Task ObtenerOfertas()
        {
            try
            {
                var resp = await smartsell.GetOfertas(page: page, searchString: buscarTxt.Text);
                ofertasCargadas = resp;
                CargarOfertas();
            }
            catch (Exception ex)
            {
                await Dialog.InfoMessage("Error", ex.Message).ShowAsync();
            }

        }

        private async void PrevButton_Click(object sender, RoutedEventArgs e)
        {
            page--;
            await ObtenerOfertas();
        }

        private async void NextButton_Click(object sender, RoutedEventArgs e)
        {
            page++;
            await ObtenerOfertas();
        }
    }
}
