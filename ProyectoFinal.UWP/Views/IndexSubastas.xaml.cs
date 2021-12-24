using ProyectoFinal.UWP.Infrastructure;
using ProyectoFinal.UWP.Models;
using ProyectoFinal.Shared.Dto;
using ProyectoFinal.Shared.Models;
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

namespace ProyectoFinal.UWP.Views
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class IndexSubastas : Page
    {
        
        private SmartSell smartSell = SmartSell.Instance;
        private SubastasPagedData subastasCargadas;
        
        public IndexSubastas()
        {
            this.InitializeComponent();
           
        }
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            subastasCargadas = await smartSell.GetSubastas();
            cargarSubastas();
        }
        public void cargarSubastas()
        {
            subastas.ItemsSource = subastasCargadas.Data;
            
        }

        private void subastas_SelectionChanged(object sender, ItemClickEventArgs e)
        {
            SubastaItemDto subasta = e.ClickedItem as SubastaItemDto;
            this.Frame.Navigate(typeof(DetailsSubasta), subasta.ID);
        }

        private void CrearSubastaHandler(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CrearSubasta),null);
        }
    }
}
