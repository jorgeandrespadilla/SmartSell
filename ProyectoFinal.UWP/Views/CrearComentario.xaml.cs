using ProyectoFinal.UWP.Infrastructure;
using ProyectoFinal.UWP.Infrastructure.Helpers;
using ProyectoFinal.Shared.Dto;
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
using ProyectoFinal.UWP.Helpers;


// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace ProyectoFinal.UWP.Views
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class CrearComentario : Page
    {

        private SmartSell smartsell = SmartSell.Instance;
        private SubastaDto subasta; 

        public CrearComentario()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            try
            {
                subasta = await smartsell.GetSubasta(Int32.Parse(e.Parameter.ToString()));
            }
            catch (Exception ex)
            {
                await Dialog.InfoMessage("Error", ex.Message).ShowAsync();
            }
        }
        
        private async void CrearComentarioHandlerBtn(object sender, RoutedEventArgs e)
        {
            try
            {
                await smartsell.CreateComentario(subasta.SubastaID, descripcionTxt.Text);
                this.Frame.Navigate(typeof(DetailsSubasta), subasta.SubastaID);
            }
            catch (Exception ex)
            {
                await Dialog.InfoMessage("Error", ex.Message).ShowAsync();
            }
        }

        private void CancelarHandlerButton(object sender, RoutedEventArgs e)
        {
            ReturnNavHelper.TryGoBack();
        }

        

    }
}
