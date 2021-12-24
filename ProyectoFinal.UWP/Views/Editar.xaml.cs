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

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace ProyectoFinal.UWP.Views
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class Editar : Page
    {
        private SmartSell smartsell = SmartSell.Instance;
        

        public Editar()
        {
            this.InitializeComponent();
            CargarInformacion();
        }

        private async void CargarInformacion()
        {
            PerfilDto usuario = await smartsell.GetPerfil();
            nombresTxt.Text = usuario.Nombres;
            apellidosTxt.Text = usuario.Apellidos;
            correoTxt.Text = usuario.Correo;
        }

        private async void ConfirmarBtnHandler(object sender, RoutedEventArgs e)
        {
            //Arreglar API para que acepte un valor de contraseña null
            try
            {
                await smartsell.EditPerfil(
                    nombresTxt.Text,
                    apellidosTxt.Text,
                    correoTxt.Text,
                    pwdTxt.Password
                );
                await Dialog.InfoMessage("Registro exitoso", "Cambio de información con éxito.").ShowAsync();
                this.Frame.Navigate(typeof(Perfil), smartsell.CurrentUser.ID);
            }
            catch (Exception ex)
            {
                await Dialog.InfoMessage("Registro fallido", ex.Message).ShowAsync();
            }
            
        }

        private void CancelarHandleButton(object sender, RoutedEventArgs e)
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
