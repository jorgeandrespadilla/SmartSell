using System;
using System.Collections.Generic;
using ProyectoFinal.Desktop.Infrastructure;
using ProyectoFinal.Desktop.Models;
using ProyectoFinal.Desktop.Views;
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
    }
}
