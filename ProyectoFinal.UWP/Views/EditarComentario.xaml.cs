using ProyectoFinal.Shared.Dto;
using ProyectoFinal.UWP.Helpers;
using ProyectoFinal.UWP.Infrastructure;
using ProyectoFinal.UWP.Infrastructure.Helpers;
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
    public sealed partial class EditarComentario : Page
    {

        private SmartSell smartSell = SmartSell.Instance;
        private ComentarioDto comentario;

        public EditarComentario()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            try
            {
                CargarInformacion(Int32.Parse(e.Parameter.ToString()));
            }
            catch (Exception ex)
            {
                await Dialog.InfoMessage("Error", ex.Message).ShowAsync();
            }

        }


        private async void CargarInformacion(int id)
        {
            try
            {
                comentario = await smartSell.GetComentario(id);
                descripcionTxt.Text = comentario.Descripcion;
            }catch(Exception ex)
            {
                await Dialog.InfoMessage("Error", ex.Message).ShowAsync();
            }
            
        }



        private void CancelarHandlerBtn(object sender, RoutedEventArgs e)
        {
            ReturnNavHelper.TryGoBack();
        }

        private async void EditarHandlerBtn(object sender, RoutedEventArgs e)
        {
            try
            {
                await smartSell.EditComentario(comentario.ComentarioID, descripcionTxt.Text);
                await Dialog.InfoMessage("Registro exitoso", "Cambio de información con éxito.").ShowAsync();
                this.Frame.Navigate(typeof(DetailsSubasta), comentario.SubastaID);
            }
            catch(Exception ex)
            {
                await Dialog.InfoMessage("Error", ex.Message).ShowAsync();
            }
        }
    }
}
